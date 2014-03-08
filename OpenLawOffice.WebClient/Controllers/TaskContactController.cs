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

    public class TaskContactController : BaseController
    {
        // Selects link based on Guid of Matter
        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult SelectContactToAssign(long id)
        {
            List<ViewModels.Contacts.SelectableContactViewModel> modelList = new List<ViewModels.Contacts.SelectableContactViewModel>();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Contacts.Contact> contactsDbo = db.Query<DBOs.Contacts.Contact>(
                    "SELECT * FROM \"contact\" WHERE \"utc_disabled\" is null");

                contactsDbo.ForEach(x =>
                {
                    ViewModels.Contacts.SelectableContactViewModel vm = Mapper.Map<ViewModels.Contacts.SelectableContactViewModel>(x);
                    modelList.Add(vm);
                });
            }

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult AssignContact(long id)
        {
            long taskId = 0;

            if (Request["TaskId"] == null)
                return View("InvalidRequest");

            if (!long.TryParse(Request["TaskId"], out taskId))
                return View("InvalidRequest");

            ViewModels.Tasks.TaskAssignedContactViewModel vm = new ViewModels.Tasks.TaskAssignedContactViewModel();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                DBOs.Tasks.Task taskDbo = db.QuerySingle<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = taskId });
                DBOs.Contacts.Contact contactDbo = db.QuerySingle<DBOs.Contacts.Contact>(
                    "SELECT * FROM \"contact\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                vm.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(taskDbo);
                vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contactDbo);
            }

            return View(vm);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult AssignContact(ViewModels.Tasks.TaskAssignedContactViewModel model)
        {
            // We need to reset the Id of the model as it is picking up the id from the route, 
            // which is incorrect
            model.Id = null;
            DBOs.Tasks.TaskAssignedContact dbo = null;

            Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                dbo = db.QuerySingle<DBOs.Tasks.TaskAssignedContact>(
                    "SELECT * FROM \"task_assigned_contact\" WHERE \"task_id\"=@TaskId AND \"contact_id\"=@ContactId",
                    new { TaskId = model.Task.Id, ContactId = model.Contact.Id });

                if (dbo == null)
                {
                    dbo = Mapper.Map<DBOs.Tasks.TaskAssignedContact>(model);
                    dbo.Id = Guid.NewGuid();
                    dbo.CreatedByUserId = dbo.ModifiedByUserId = currentUser.Id.Value;
                    dbo.UtcCreated = dbo.UtcModified = DateTime.UtcNow;
                    db.Insert<DBOs.Tasks.TaskAssignedContact>(dbo);
                }
                else
                {
                    dbo.DisabledByUserId = null;
                    dbo.UtcDisabled = null;
                    dbo.Role = model.Role;
                    dbo.UtcModified = DateTime.UtcNow;
                    dbo.ModifiedByUserId = currentUser.Id.Value;
                    db.UpdateOnly(dbo,
                        fields => new
                        {
                            fields.Role,
                            fields.UtcDisabled,
                            fields.DisabledByUserId,
                            fields.ModifiedByUserId,
                            fields.UtcModified
                        }, where => where.Id == dbo.Id);
                }

                dbo = db.GetById<DBOs.Matters.MatterContact>(dbo.Id);
            }

            return RedirectToAction("Contacts", "Matters", new { id = dbo.MatterId.ToString() });
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Matters.MatterContactViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.MatterContact dbo = db.QuerySingle<DBOs.Matters.MatterContact>(
                    "SELECT * FROM \"matter_contact\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Matters.Matter matterDbo = db.GetById<DBOs.Matters.Matter>(dbo.MatterId);
                DBOs.Contacts.Contact contactDbo = db.GetById<DBOs.Contacts.Contact>(dbo.ContactId);

                model = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(dbo);
                model.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contactDbo);
                model.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matterDbo);
            }

            return View(model);
        }
        
        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Matters.MatterContactViewModel model)
        {
            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);

                DBOs.Matters.MatterContact dbo = Mapper.Map<DBOs.Matters.MatterContact>(model);
                dbo.UtcModified = DateTime.UtcNow;
                dbo.ModifiedByUserId = user.Id.Value;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    db.UpdateOnly(dbo,
                        fields => new
                        {
                            fields.Role,
                            fields.ModifiedByUserId,
                            fields.UtcModified
                        },
                        where => where.Id == dbo.Id);

                    dbo = db.GetById<DBOs.Matters.MatterContact>(dbo.Id);
                }

                return RedirectToAction("Contacts", "Matters", new { id = dbo.MatterId.ToString() });
            }
            catch
            {
                return View(model);
            }
        }


        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Matters.MatterContactViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.MatterContact dbo = db.QuerySingle<DBOs.Matters.MatterContact>(
                    "SELECT * FROM \"matter_contact\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Matters.Matter matterDbo = db.GetById<DBOs.Matters.Matter>(dbo.MatterId);
                DBOs.Contacts.Contact contactDbo = db.GetById<DBOs.Contacts.Contact>(dbo.ContactId);

                model = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(dbo);
                model.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contactDbo);
                model.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matterDbo);

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(int id)
        {
            return Details(id);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(int id, ViewModels.Matters.MatterContactViewModel model)
        {
            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);

                DBOs.Matters.MatterContact dbo = Mapper.Map<DBOs.Matters.MatterContact>(model);
                dbo.UtcDisabled = DateTime.UtcNow;
                dbo.DisabledByUserId = user.Id.Value;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    db.UpdateOnly(dbo,
                        fields => new
                        {
                            fields.DisabledByUserId,
                            fields.UtcDisabled
                        },
                        where => where.Id == dbo.Id);

                    dbo = db.GetById<DBOs.Matters.MatterContact>(dbo.Id);
                }

                return RedirectToAction("Contacts", "Matters", new { id = dbo.MatterId.ToString() });
            }
            catch
            {
                return View(model);
            }
        }
    }
}
