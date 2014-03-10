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

    public class ResponsibleUsersController : BaseController
    {
        //
        // GET: /ResponsibleUsers/Details/5
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Matters.ResponsibleUserViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.ResponsibleUser dbo = db.Single<DBOs.Matters.ResponsibleUser>(
                    "SELECT * FROM \"responsible_user\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Matters.Matter dboMatter = db.LoadSingleById<DBOs.Matters.Matter>(dbo.MatterId);
                DBOs.Security.User dboUser = db.LoadSingleById<DBOs.Security.User>(dbo.UserId);

                model = Mapper.Map<ViewModels.Matters.ResponsibleUserViewModel>(dbo);
                model.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(dboMatter);
                model.User = Mapper.Map<ViewModels.Security.UserViewModel>(dboUser);

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }

        //
        // GET: /ResponsibleUsers/Create/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create(Guid id)
        {
            List<ViewModels.Security.UserViewModel> userList = new List<ViewModels.Security.UserViewModel>();
            ViewModels.Matters.MatterViewModel matter = null;

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                DBOs.Matters.Matter matterDbo = db.Single<DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                List<DBOs.Security.User> dboUserList = db.SqlList<DBOs.Security.User>(
                   "SELECT * FROM \"user\" WHERE \"utc_disabled\" is null");

                dboUserList.ForEach(x =>
                {
                    userList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
                });

                matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matterDbo);
            }

            ViewData["UserList"] = userList;
            return View(new ViewModels.Matters.ResponsibleUserViewModel() { Matter = matter });
        }

        //
        // POST: /ResponsibleUsers/Create/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Matters.ResponsibleUserViewModel model)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                model.Matter = new ViewModels.Matters.MatterViewModel()
                {
                    Id = Guid.Parse(RouteData.Values["Id"].ToString()),
                    IsStub = true
                };

                List<DBOs.Matters.ResponsibleUser> foundUserHits = null;
                DBOs.Matters.ResponsibleUser dboRespUser = null;
                
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    foundUserHits = db.SqlList<DBOs.Matters.ResponsibleUser>(
                        "SELECT * FROM \"responsible_user\" WHERE \"matter_id\"=@MatterId AND \"user_id\"=@UserId",
                        new { MatterId = model.Matter.Id.Value, UserId = model.User.Id.Value });

                    foreach (DBOs.Matters.ResponsibleUser x in foundUserHits)
                    {
                        if (!x.UtcDisabled.HasValue)
                        {
                            // Error - user/matter combination already exists
                            ViewModels.Matters.MatterViewModel matter;
                            List<ViewModels.Security.UserViewModel> userList = new List<ViewModels.Security.UserViewModel>();
                            
                            ModelState.AddModelError("User", "This user already has a responsibility.");

                            DBOs.Matters.Matter matterDbo = db.Single<DBOs.Matters.Matter>(
                                "SELECT * FROM \"matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                                new { Id = x.MatterId });

                            List<DBOs.Security.User> dboUserList = db.SqlList<DBOs.Security.User>(
                               "SELECT * FROM \"user\" WHERE \"utc_disabled\" is null");

                            matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matterDbo);

                            dboUserList.ForEach(y =>
                            {
                                userList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(y));
                            });

                            ViewData["UserList"] = userList;

                            return View(new ViewModels.Matters.ResponsibleUserViewModel() { Matter = matter });
                        }
                    };

                    // If foundUserHits > 0 -> Disabled, so update with enable
                    if (foundUserHits != null && foundUserHits.Count > 0)
                    {
                        foundUserHits[0].Responsibility = model.Responsibility;
                        foundUserHits[0].UtcDisabled = null;
                        foundUserHits[0].DisabledByUserId = null;
                        db.UpdateOnly(foundUserHits[0],
                            fields => new
                            {
                                fields.Responsibility,
                                fields.UtcDisabled,
                                fields.DisabledByUserId
                            },
                            where => where.Id == foundUserHits[0].Id);

                        for (int i = 1; i < foundUserHits.Count; i++)
                        {
                            // Drop rest
                            db.Delete(foundUserHits[i]);
                        }
                    }
                    else
                    {
                        dboRespUser = Mapper.Map<DBOs.Matters.ResponsibleUser>(model);
                        dboRespUser.CreatedByUserId = dboRespUser.ModifiedByUserId = currentUser.Id.Value;
                        dboRespUser.UtcCreated = dboRespUser.UtcModified = DateTime.UtcNow;
                        db.Insert<DBOs.Matters.ResponsibleUser>(dboRespUser);
                    }
                }

                return RedirectToAction("ResponsibleUsers", "Matters", new { Id = model.Matter.Id });
            }
            catch (Exception)
            {
                return Create(model.Matter.Id.Value);
            }
        }
        

        //
        // GET: /ResponsibleUsers/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            List<ViewModels.Security.UserViewModel> userList = new List<ViewModels.Security.UserViewModel>();
            ViewModels.Matters.ResponsibleUserViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.ResponsibleUser dbo = db.Single<DBOs.Matters.ResponsibleUser>(
                    "SELECT * FROM \"responsible_user\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                List<DBOs.Security.User> dboUserList = db.SqlList<DBOs.Security.User>(
                    "SELECT * FROM \"user\" WHERE \"utc_disabled\" is null");

                DBOs.Matters.Matter dboMatter = db.SingleById<DBOs.Matters.Matter>(dbo.MatterId);
                DBOs.Security.User dboUser = db.SingleById<DBOs.Security.User>(dbo.UserId);

                model = Mapper.Map<ViewModels.Matters.ResponsibleUserViewModel>(dbo);
                model.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(dboMatter);
                model.User = Mapper.Map<ViewModels.Security.UserViewModel>(dboUser);
                
                // Core Details
                PopulateCoreDetails(model);

                dboUserList.ForEach(x =>
                {
                    userList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
                });
            }

            ViewData["UserList"] = userList;
            return View(model);
        }
        

        //
        // POST: /ResponsibleUsers/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Matters.ResponsibleUserViewModel model)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);

                DBOs.Matters.ResponsibleUser dbo = Mapper.Map<DBOs.Matters.ResponsibleUser>(model);
                dbo.ModifiedByUserId = currentUser.Id.Value;
                dbo.UtcModified = DateTime.UtcNow;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    DBOs.Matters.ResponsibleUser currentDbo = db.SingleById<DBOs.Matters.ResponsibleUser>(id);
                    
                    model.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(
                        db.SingleById<DBOs.Matters.Matter>(currentDbo.MatterId));

                    db.UpdateOnly(dbo,
                        fields => new
                        {
                            fields.UserId,
                            fields.Responsibility,
                            fields.ModifiedByUserId,
                            fields.UtcModified
                        },
                        where => where.Id == dbo.Id);
                }

                return RedirectToAction("ResponsibleUsers", "Matters", new { Id = model.Matter.Id });
            }
            catch
            {
                return Edit(id);
            }
        }

        //
        // GET: /ResponsibleUsers/Delete/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(int id)
        {
            return Details(id);
        }

        //
        // POST: /ResponsibleUsers/Delete/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(int id, ViewModels.Matters.ResponsibleUserViewModel model)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    DBOs.Matters.ResponsibleUser dbo = db.Single<DBOs.Matters.ResponsibleUser>(
                        "SELECT * FROM \"responsible_user\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
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

                    return RedirectToAction("ResponsibleUsers", "Matters", new { Id = dbo.MatterId });
                }
            }
            catch
            {
                return Details(id);
            }
        }
    }
}
