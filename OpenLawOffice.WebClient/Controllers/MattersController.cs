namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using ServiceStack.OrmLite;
    using AutoMapper;

    public class MattersController : BaseController
    {
        //
        // GET: /Matter/
        [SecurityFilter(SecurityAreaName="Matters.Matter", IsSecuredResource=true, 
            Permission=Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            string title = null;
            string filterClause = "";
            List<ViewModels.Matters.MatterViewModel> modelList = new List<ViewModels.Matters.MatterViewModel>();

            //if (!string.IsNullOrEmpty(request.Title))
            //    filterClause += " LOWER(\"title\") like '%' || LOWER(@Title) || '%' AND";
            
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Matters.Matter> list = db.SqlList<DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" JOIN \"secured_resource_acl\" ON " +
                    "\"matter\".\"id\"=\"secured_resource_acl\".\"secured_resource_id\" " +
                    "WHERE " + filterClause +
                    "\"secured_resource_acl\".\"allow_flags\" & 2 > 0 " +
                    "AND NOT \"secured_resource_acl\".\"deny_flags\" & 2 > 0 " +
                    "AND \"matter\".\"utc_disabled\" is null  " +
                    "AND \"secured_resource_acl\".\"utc_disabled\" is null",
                    new { Title = title });

                list.ForEach(dbo =>
                {
                    modelList.Add(Mapper.Map<ViewModels.Matters.MatterViewModel>(dbo));
                });
            }

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.List)]
        [HttpGet]
        public ActionResult ListChildrenJqGrid(Guid? id)
        {
            ViewModels.JqGridObject jqObject;
            int level = 0;

            if (id == null)
            {
                // jqGrid uses nodeid by default
                if (!string.IsNullOrEmpty(Request["nodeid"]))
                    id = Guid.Parse(Request["nodeid"]);
            }

            List<ViewModels.Matters.MatterViewModel> modelList = GetChildrenList(id);
            List<object> anonList = new List<object>();

            if (!string.IsNullOrEmpty(Request["n_level"]))
                level = int.Parse(Request["n_level"]) + 1;

            modelList.ForEach(x =>
            {
                if (x.Parent == null)
                    anonList.Add(new
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Synopsis = x.Synopsis,
                        level = level,
                        isLeaf = false,
                        expanded = false
                    });
                else
                    anonList.Add(new
                    {
                        Id = x.Id,
                        parent = x.Parent.Id,
                        Title = x.Title,
                        Synopsis = x.Synopsis,
                        level = level,
                        isLeaf = false,
                        expanded = false
                    });
            });

            jqObject = new ViewModels.JqGridObject()
            {
                TotalPages = 1,
                CurrentPage = 1,
                TotalRecords = modelList.Count,
                Rows = anonList.ToArray()
            };

            return Json(jqObject, JsonRequestBehavior.AllowGet);
        }

        private List<ViewModels.Matters.MatterViewModel> GetChildrenList(Guid? id)
        {
            List<ViewModels.Matters.MatterViewModel> modelList = new List<ViewModels.Matters.MatterViewModel>();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Matters.Matter> list = null;

                if (!id.HasValue)
                    list = db.SqlList<DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" JOIN \"secured_resource_acl\" ON " +
                        "\"matter\".\"id\"=\"secured_resource_acl\".\"secured_resource_id\" " +
                        "WHERE \"secured_resource_acl\".\"allow_flags\" & 2 > 0 " +
                        "AND NOT \"secured_resource_acl\".\"deny_flags\" & 2 > 0 " +
                        "AND \"matter\".\"utc_disabled\" is null  " +
                        "AND \"secured_resource_acl\".\"utc_disabled\" is null " +
                        "AND \"matter\".\"parent_id\" is null");
                else
                    list = db.SqlList<DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" JOIN \"secured_resource_acl\" ON " +
                        "\"matter\".\"id\"=\"secured_resource_acl\".\"secured_resource_id\" " +
                        "WHERE \"secured_resource_acl\".\"allow_flags\" & 2 > 0 " +
                        "AND NOT \"secured_resource_acl\".\"deny_flags\" & 2 > 0 " +
                        "AND \"matter\".\"utc_disabled\" is null  " +
                        "AND \"secured_resource_acl\".\"utc_disabled\" is null " +
                        "AND \"matter\".\"parent_id\"=@ParentId",
                        new { ParentId = id.Value });

                list.ForEach(dbo =>
                {
                    modelList.Add(Mapper.Map<ViewModels.Matters.MatterViewModel>(dbo));
                });
            }

            return modelList;
        }

        //
        // GET: /Matter/Details/9acb1b4f-0442-4c9b-a550-ad7478e36fb2
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Matters.MatterViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.Matter dbo = db.Single<DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id});
                
                model = Mapper.Map<ViewModels.Matters.MatterViewModel>(dbo);

                if (dbo.ParentId.HasValue)
                {
                    DBOs.Matters.Matter parentDbo = db.SingleById<DBOs.Matters.Matter>(dbo.ParentId);
                    model.Parent = Mapper.Map<ViewModels.Matters.MatterViewModel>(parentDbo);
                }

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }

        //
        // GET: /Matter/Create
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Matter/CreateE:\Projects\OpenLawOffice\OpenLawOffice.WebClient\Controllers\HomeController.cs
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Matters.MatterViewModel model)
        {
            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);
                
                DBOs.Matters.Matter dboMatter = Mapper.Map<DBOs.Matters.Matter>(model);
                dboMatter.Id = Guid.NewGuid();
                dboMatter.CreatedByUserId = dboMatter.ModifiedByUserId = user.Id.Value;
                dboMatter.UtcCreated = dboMatter.UtcModified = DateTime.UtcNow;

                DBOs.Security.SecuredResource dboSR = new DBOs.Security.SecuredResource()
                {
                    Id = dboMatter.Id, 
                    CreatedByUserId = user.Id.Value,
                    ModifiedByUserId = user.Id.Value,
                    UtcCreated = DateTime.UtcNow,
                    UtcModified = DateTime.UtcNow
                };

                DBOs.Security.SecuredResourceAcl dboSrAcl = new DBOs.Security.SecuredResourceAcl()
                {
                    Id = Guid.NewGuid(),
                    CreatedByUserId = user.Id.Value,
                    ModifiedByUserId = user.Id.Value,
                    UtcCreated = DateTime.UtcNow,
                    UtcModified = DateTime.UtcNow,
                    UserId = user.Id.Value,
                    SecuredResourceId = dboSR.Id,
                    AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllRead | Common.Models.PermissionType.AllWrite),
                    DenyFlags = (int)Common.Models.PermissionType.None
                };

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    using (IDbTransaction tran = db.BeginTransaction())
                    {
                        // Insert Matter
                        db.Insert<DBOs.Matters.Matter>(dboMatter);

                        // Insert SecuredResource
                        db.Insert<DBOs.Security.SecuredResource>(dboSR);

                        // Insert ACL
                        db.Insert<DBOs.Security.SecuredResourceAcl>(dboSrAcl);

                        tran.Commit();
                    }
                }
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
        
        //
        // GET: /Matter/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Matters.MatterViewModel model = null;
            Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.Matter dbo = db.Single<DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                model = Mapper.Map<ViewModels.Matters.MatterViewModel>(dbo);
            }

            return View(model);
        }

        //
        // POST: /Matter/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Matters.MatterViewModel model)
        {
            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);

                DBOs.Matters.Matter dbo = Mapper.Map<DBOs.Matters.Matter>(model);
                dbo.UtcModified = DateTime.UtcNow;
                dbo.ModifiedByUserId = user.Id.Value;

                DBOs.Security.SecuredResource dboSR = new DBOs.Security.SecuredResource()
                {
                    Id = id,
                    ModifiedByUserId = user.Id.Value,
                    UtcModified = DateTime.UtcNow
                };

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    using (IDbTransaction tran = db.BeginTransaction())
                    {
                        db.UpdateOnly(dbo,
                            fields => new
                            {
                                fields.Title,
                                fields.Synopsis,
                                fields.ParentId,
                                fields.ModifiedByUserId,
                                fields.UtcModified
                            },
                            where => where.Id == dbo.Id);

                        db.UpdateOnly(dboSR,
                            fields => new
                            {
                                fields.ModifiedByUserId,
                                fields.UtcModified
                            },
                            where => where.Id == dbo.Id);

                        tran.Commit();
                    }
                }
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        // A note on delete - https://github.com/NodineLegal/OpenLawOffice/wiki/Design-of-Disabling-a-Matter

        //
        // GET: /Matter/Delete/5
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
            return View();
        }

        //
        // POST: /Matter/Delete/5
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            throw new NotImplementedException();
            try
            {
                // TODO: Add delete logic here

 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Tags(Guid id)
        {
            List<ViewModels.Matters.MatterTagViewModel> modelList = new List<ViewModels.Matters.MatterTagViewModel>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Matters.MatterTag> list = db.SqlList<DBOs.Matters.MatterTag>(
                    "SELECT * FROM \"matter_tag\" WHERE \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                    new { MatterId = id });

                list.ForEach(dbo =>
                {
                    ViewModels.Matters.MatterTagViewModel tagModel = Mapper.Map<ViewModels.Matters.MatterTagViewModel>(dbo);

                    DBOs.Tagging.TagCategory tagCatDbo = db.SingleById<DBOs.Tagging.TagCategory>(tagModel.TagCategory.Id);
                    ViewModels.Tagging.TagCategoryViewModel tagCatVM = Mapper.Map<ViewModels.Tagging.TagCategoryViewModel>(tagCatDbo);
                    tagModel.TagCategory = tagCatVM;

                    modelList.Add(tagModel);
                });
            }

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult ResponsibleUsers(Guid id)
        {
            List<ViewModels.Matters.ResponsibleUserViewModel> modelList = new List<ViewModels.Matters.ResponsibleUserViewModel>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Matters.ResponsibleUser> list = db.SqlList<DBOs.Matters.ResponsibleUser>(
                    "SELECT * FROM \"responsible_user\" WHERE \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                    new { MatterId = id });

                list.ForEach(dbo =>
                {
                    ViewModels.Matters.ResponsibleUserViewModel responsibleUser = Mapper.Map<ViewModels.Matters.ResponsibleUserViewModel>(dbo);

                    //DBOs.Matters.Matter matterDbo = db.SingleById<DBOs.Matters.Matter>(dbo.MatterId);
                    DBOs.Security.User userDbo = db.SingleById<DBOs.Security.User>(dbo.UserId);

                    //responsibleUser.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matterDbo);
                    responsibleUser.User = Mapper.Map<ViewModels.Security.UserViewModel>(userDbo);

                    modelList.Add(responsibleUser);
                });
            }

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Contacts(Guid id)
        {
            List<ViewModels.Matters.MatterContactViewModel> modelList = new List<ViewModels.Matters.MatterContactViewModel>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Matters.MatterContact> list = db.SqlList<DBOs.Matters.MatterContact>(
                    "SELECT * FROM \"matter_contact\" WHERE \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                    new { MatterId = id });

                list.ForEach(dbo =>
                {
                    ViewModels.Matters.MatterContactViewModel vm = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(dbo);

                    DBOs.Contacts.Contact contactDbo = db.SingleById<DBOs.Contacts.Contact>(dbo.ContactId);

                    vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contactDbo);
                    modelList.Add(vm);
                });
            }

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.Task", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Tasks(Guid id)
        {
            List<ViewModels.Tasks.TaskViewModel> modelList = TasksController.GetListForMatter(id);

            return View(modelList);
        }
    }
}
