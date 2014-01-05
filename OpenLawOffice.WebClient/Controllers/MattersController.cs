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
            List<Common.Models.Matters.Matter> modelList = new List<Common.Models.Matters.Matter>();

            //if (!string.IsNullOrEmpty(request.Title))
            //    filterClause += " LOWER(\"title\") like '%' || LOWER(@Title) || '%' AND";
            
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Matters.Matter> list = db.Query<DBOs.Matters.Matter>(
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
                    modelList.Add(Mapper.Map<Common.Models.Matters.Matter>(dbo));
                });
            }

            return View(modelList);
        }

        //
        // GET: /Matter/Details/9acb1b4f-0442-4c9b-a550-ad7478e36fb2
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            Common.Models.Matters.Matter model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.Matter dbo = db.QuerySingle<DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id});

                model = Mapper.Map<Common.Models.Matters.Matter>(dbo);

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
        // POST: /Matter/Create
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(Common.Models.Matters.Matter model)
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
            Common.Models.Matters.Matter model = null;
            Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.Matter dbo = db.QuerySingle<DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                model = Mapper.Map<Common.Models.Matters.Matter>(dbo);
            }

            return View(model);
        }

        //
        // POST: /Matter/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, Common.Models.Matters.Matter model)
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
    }
}
