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

    public class SecurityAreaAclsController : BaseController
    {
        //
        // GET: /SecurityAreaAcls/
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            List<ViewModels.Security.AreaAclViewModel> modelList = new List<ViewModels.Security.AreaAclViewModel>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Security.AreaAcl> list = db.Query<DBOs.Security.AreaAcl>(
                    "SELECT * FROM \"area_acl\" WHERE \"utc_disabled\" is null");

                list.ForEach(dbo =>
                {
                    // Get User
                    DBOs.Security.User userDbo = db.GetById<DBOs.Security.User>(dbo.UserId);

                    // Get Area
                    DBOs.Security.Area areaDbo = db.GetById<DBOs.Security.Area>(dbo.SecurityAreaId);

                    ViewModels.Security.AreaAclViewModel model = Mapper.Map<ViewModels.Security.AreaAclViewModel>(dbo);

                    model.User = Mapper.Map<ViewModels.Security.UserViewModel>(userDbo);

                    model.Area = Mapper.Map<ViewModels.Security.AreaViewModel>(areaDbo);

                    modelList.Add(model);
                });
            }

            return View(modelList);
        }

        //
        // GET: /SecurityAreaAcls/Details/5
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            List<ViewModels.Security.UserViewModel> userList = new List<ViewModels.Security.UserViewModel>();
            ViewModels.Security.AreaAclViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Security.User> list = db.Query<DBOs.Security.User>(
                    "SELECT * FROM \"user\" " +
                    "WHERE \"utc_disabled\" is null");

                list.ForEach(dbo =>
                {
                    userList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(dbo));
                });

                // Load base DBO
                DBOs.Security.AreaAcl dboAcl = db.QuerySingle<DBOs.Security.AreaAcl>(
                    "SELECT * FROM \"area_acl\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                // Get User
                DBOs.Security.User userDbo = db.GetById<DBOs.Security.User>(dboAcl.UserId);

                // Get Area
                DBOs.Security.Area areaDbo = db.GetById<DBOs.Security.Area>(dboAcl.SecurityAreaId);

                model = Mapper.Map<ViewModels.Security.AreaAclViewModel>(dboAcl);

                model.User = Mapper.Map<ViewModels.Security.UserViewModel>(userDbo);

                model.Area = Mapper.Map<ViewModels.Security.AreaViewModel>(areaDbo);

                PopulateCoreDetails(model);
            }

            ViewData["UserList"] = userList;
            return View(model);
        }

        //
        // GET: /SecurityAreaAcls/Create
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            List<Common.Models.Security.User> userList = new List<Common.Models.Security.User>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Security.User> list = db.Query<DBOs.Security.User>(
                    "SELECT * FROM \"user\" " +
                    "WHERE \"utc_disabled\" is null");

                list.ForEach(dbo =>
                {
                    userList.Add(Mapper.Map<Common.Models.Security.User>(dbo));
                });
            }

            ViewData["Readonly"] = false;
            ViewData["UserList"] = userList;

            return View(new Common.Models.Security.AreaAcl() { AllowFlags = 0, DenyFlags = 0 });
        } 

        //
        // POST: /SecurityAreaAcls/Create
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(Common.Models.Security.AreaAcl model)
        {
            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);

                DBOs.Security.AreaAcl dbo = Mapper.Map<DBOs.Security.AreaAcl>(model);
                dbo.CreatedByUserId = dbo.ModifiedByUserId = user.Id.Value;
                dbo.UtcCreated = dbo.UtcModified = DateTime.UtcNow;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    // Insert User
                    db.Insert<DBOs.Security.AreaAcl>(dbo);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
        
        //
        // GET: /SecurityAreaAcls/Edit/5
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Security.AreaAclViewModel model = null;
            List<Common.Models.Security.User> userList = new List<Common.Models.Security.User>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Security.AreaAcl dboAcl = db.QuerySingle<DBOs.Security.AreaAcl>(
                    "SELECT * FROM \"area_acl\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                List<DBOs.Security.User> list = db.Query<DBOs.Security.User>(
                    "SELECT * FROM \"user\" " +
                    "WHERE \"utc_disabled\" is null");

                list.ForEach(dbo =>
                {
                    userList.Add(Mapper.Map<Common.Models.Security.User>(dbo));
                });

                // Get User
                DBOs.Security.User userDbo = db.GetById<DBOs.Security.User>(dboAcl.UserId);

                // Get Area
                DBOs.Security.Area areaDbo = db.GetById<DBOs.Security.Area>(dboAcl.SecurityAreaId);

                model = Mapper.Map<ViewModels.Security.AreaAclViewModel>(dboAcl);

                model.User = Mapper.Map<ViewModels.Security.UserViewModel>(userDbo);

                model.Area = Mapper.Map<ViewModels.Security.AreaViewModel>(areaDbo);
            }

            List<SelectListItem> selectList = new List<SelectListItem>();
            
            userList.ForEach(item =>
            {
                SelectListItem slItem = new SelectListItem()
                {
                    Value = item.Id.Value.ToString(),
                    Text = item.Username
                };
                if (item.Id == model.User.Id.Value)
                    slItem.Selected = true;
                selectList.Add(slItem);
            });

            ViewData["UserList"] = selectList;
            return View(model);
        }

        //
        // POST: /SecurityAreaAcls/Edit/5
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, Common.Models.Security.AreaAcl model)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);

                DBOs.Security.AreaAcl dbo = Mapper.Map<DBOs.Security.AreaAcl>(model);
                dbo.ModifiedByUserId = currentUser.Id.Value;
                dbo.UtcModified = DateTime.UtcNow;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    db.UpdateOnly(dbo,
                        fields => new
                        {
                            // Note - Only flags may be changed
                            fields.AllowFlags,
                            fields.DenyFlags,
                            fields.ModifiedByUserId,
                            fields.UtcModified
                        },
                        where => where.Id == dbo.Id);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
    }
}
