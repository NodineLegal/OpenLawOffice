namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using ServiceStack.OrmLite;
    using OpenLawOffice.Server.Core;
    using DBOs = OpenLawOffice.Server.Core.DBOs;
    using AutoMapper;

    public class MatterTagsController : BaseController
    {
        //
        // GET: /MatterTags/Details/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Matters.MatterTagViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.MatterTag dbo = db.QuerySingle<DBOs.Matters.MatterTag>(
                    "SELECT * FROM \"matter_tag\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id});

                DBOs.Matters.Matter dboMatter = db.GetById<DBOs.Matters.Matter>(dbo.MatterId);

                model = Mapper.Map<ViewModels.Matters.MatterTagViewModel>(dbo);
                model.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(dboMatter);

                if (dbo.TagCategoryId.HasValue)
                {
                    DBOs.Tagging.TagCategory dboTagCat = db.GetById<DBOs.Tagging.TagCategory>(dbo.TagCategoryId.Value);
                    model.TagCategory = Mapper.Map<ViewModels.Tagging.TagCategoryViewModel>(dboTagCat);
                }                

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }

        //
        // GET: /MatterTags/Create/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create(Guid id)
        {
            ViewModels.Matters.MatterViewModel matter = null;

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                DBOs.Matters.Matter matterDbo = db.QuerySingle<DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matterDbo);
            }

            return View(new ViewModels.Matters.MatterTagViewModel() { Matter = matter });
        } 

        //
        // POST: /MatterTags/Create/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Matters.MatterTagViewModel model)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                model.Id = Guid.NewGuid();
                model.Matter = new ViewModels.Matters.MatterViewModel()
                {
                    Id = Guid.Parse(RouteData.Values["Id"].ToString()),
                    IsStub = true
                };

                DBOs.Matters.MatterTag dboMatterTag = Mapper.Map<DBOs.Matters.MatterTag>(model);
                dboMatterTag.CreatedByUserId = dboMatterTag.ModifiedByUserId = currentUser.Id.Value;
                dboMatterTag.UtcCreated = dboMatterTag.UtcModified = DateTime.UtcNow;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    DBOs.Tagging.TagCategory tagCat = db.QuerySingle<DBOs.Tagging.TagCategory>(
                        "SELECT * FROM \"tag_category\" WHERE \"name\"=@Name AND \"utc_disabled\" is null",
                        new { Name = model.TagCategory.Name });

                    if (tagCat != null)
                    {
                        dboMatterTag.TagCategoryId = tagCat.Id;
                    }
                    else
                    {
                        // TODO : Check permissions
                        tagCat = new DBOs.Tagging.TagCategory()
                        {
                            CreatedByUserId = currentUser.Id.Value,
                            ModifiedByUserId = currentUser.Id.Value,
                            UtcCreated = DateTime.UtcNow,
                            UtcModified = DateTime.UtcNow,
                            Name = model.TagCategory.Name
                        };

                        db.Insert<DBOs.Tagging.TagCategory>(tagCat);
                        tagCat.Id = (int)db.GetLastInsertId();

                        dboMatterTag.TagCategoryId = tagCat.Id;
                    }

                    // Insert MatterTag
                    db.Insert<DBOs.Matters.MatterTag>(dboMatterTag);
                }

                return RedirectToAction("Tags", "Matters", new { Id = model.Matter.Id });
            }
            catch (Exception e)
            {
                ViewModels.Matters.MatterViewModel matter = null;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    DBOs.Matters.Matter matterDbo = db.QuerySingle<DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                        new { Id = Guid.Parse(RouteData.Values["Id"].ToString()) });

                    matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matterDbo);
                }

                return View(new ViewModels.Matters.MatterTagViewModel() { Matter = matter });
            }
        }
        
        //
        // GET: /MatterTags/Edit/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Matters.MatterTagViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.MatterTag dbo = db.QuerySingle<DBOs.Matters.MatterTag>(
                    "SELECT * FROM \"matter_tag\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Matters.Matter dboMatter = db.GetById<DBOs.Matters.Matter>(dbo.MatterId);

                model = Mapper.Map<ViewModels.Matters.MatterTagViewModel>(dbo);
                model.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(dboMatter);

                if (dbo.TagCategoryId.HasValue)
                {
                    DBOs.Tagging.TagCategory dboTagCat = db.GetById<DBOs.Tagging.TagCategory>(dbo.TagCategoryId.Value);
                    model.TagCategory = Mapper.Map<ViewModels.Tagging.TagCategoryViewModel>(dboTagCat);
                }

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }

        //
        // POST: /MatterTags/Edit/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Matters.MatterTagViewModel model)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                ViewModels.Matters.MatterTagViewModel currentModel = null;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    DBOs.Matters.MatterTag dbo = db.QuerySingle<DBOs.Matters.MatterTag>(
                        "SELECT * FROM \"matter_tag\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                        new { Id = id });

                    currentModel = Mapper.Map<ViewModels.Matters.MatterTagViewModel>(dbo);

                    dbo.Tag = model.Tag;
                    dbo.ModifiedByUserId = currentUser.Id.Value;
                    dbo.UtcModified = DateTime.UtcNow;

                    DBOs.Tagging.TagCategory tagCat = db.QuerySingle<DBOs.Tagging.TagCategory>(
                        "SELECT * FROM \"tag_category\" WHERE \"name\"=@Name AND \"utc_disabled\" is null",
                        new { Name = model.TagCategory.Name });

                    if (tagCat != null)
                    {
                        dbo.TagCategoryId = tagCat.Id;
                    }
                    else
                    {
                        // TODO : Check permissions
                        tagCat = new DBOs.Tagging.TagCategory()
                        {
                            CreatedByUserId = currentUser.Id.Value,
                            ModifiedByUserId = currentUser.Id.Value,
                            UtcCreated = DateTime.UtcNow,
                            UtcModified = DateTime.UtcNow,
                            Name = model.TagCategory.Name
                        };

                        db.Insert<DBOs.Tagging.TagCategory>(tagCat);
                        tagCat.Id = (int)db.GetLastInsertId();

                        dbo.TagCategoryId = tagCat.Id;
                    }

                    db.UpdateOnly(dbo,
                        fields => new
                        {
                            fields.TagCategoryId,
                            fields.Tag,
                            fields.ModifiedByUserId,
                            fields.UtcModified
                        },
                        where => where.Id == dbo.Id);
                }

                return RedirectToAction("Tags", "Matters", new { Id = model.Matter.Id });
            }
            catch
            {
                return View(model);
            }
        }

        //
        // GET: /MatterTags/Delete/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        //
        // POST: /MatterTags/Delete/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(Guid id, ViewModels.Matters.MatterTagViewModel model)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    DBOs.Matters.MatterTag dbo = db.QuerySingle<DBOs.Matters.MatterTag>(
                        "SELECT * FROM \"matter_tag\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                        new { Id = id });

                    dbo.UtcDisabled = DateTime.UtcNow;
                    dbo.DisabledByUserId = currentUser.Id;

                    db.UpdateOnly(dbo,
                        fields => new
                        {
                            fields.UtcDisabled,
                            fields.DisabledByUserId
                        },
                        where => where.Id == dbo.Id);

                    return RedirectToAction("Tags", "Matters", new { Id = dbo.MatterId });
                }
            }
            catch
            {
                return Details(id);
            }
        }
    }
}
