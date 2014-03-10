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

    public class TaskAssignedContactController : BaseController
    {
        // Selects link based on Guid of Matter
        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult SelectContactToAssign(long id)
        {
            List<ViewModels.Contacts.SelectableContactViewModel> modelList = new List<ViewModels.Contacts.SelectableContactViewModel>();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Contacts.Contact> contactsDbo = db.SqlList<DBOs.Contacts.Contact>(
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
                DBOs.Tasks.Task taskDbo = db.Single<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = taskId });
                DBOs.Contacts.Contact contactDbo = db.Single<DBOs.Contacts.Contact>(
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
                dbo = db.Single<DBOs.Tasks.TaskAssignedContact>(
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
                    dbo.AssignmentType = (int)model.AssignmentType;
                    dbo.UtcModified = DateTime.UtcNow;
                    dbo.ModifiedByUserId = currentUser.Id.Value;
                    db.UpdateOnly(dbo,
                        fields => new
                        {
                            fields.AssignmentType,
                            fields.UtcDisabled,
                            fields.DisabledByUserId,
                            fields.ModifiedByUserId,
                            fields.UtcModified
                        }, where => where.Id == dbo.Id);
                }

                dbo = db.SingleById<DBOs.Tasks.TaskAssignedContact>(dbo.Id);
            }

            return RedirectToAction("Contacts", "Tasks", new { id = dbo.TaskId.ToString() });
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Tasks.TaskAssignedContactViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Tasks.TaskAssignedContact dbo = db.Single<DBOs.Tasks.TaskAssignedContact>(
                    "SELECT * FROM \"task_assigned_contact\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Tasks.Task taskDbo = db.SingleById<DBOs.Tasks.Task>(dbo.TaskId);
                DBOs.Contacts.Contact contactDbo = db.SingleById<DBOs.Contacts.Contact>(dbo.ContactId);

                model = Mapper.Map<ViewModels.Tasks.TaskAssignedContactViewModel>(dbo);
                model.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contactDbo);
                model.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(taskDbo);
            }

            return View(model);
        }
        
        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Tasks.TaskAssignedContactViewModel model)
        {
            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);

                DBOs.Tasks.TaskAssignedContact dbo = Mapper.Map<DBOs.Tasks.TaskAssignedContact>(model);
                dbo.UtcModified = DateTime.UtcNow;
                dbo.ModifiedByUserId = user.Id.Value;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    db.UpdateOnly(dbo,
                        fields => new
                        {
                            fields.AssignmentType,
                            fields.ModifiedByUserId,
                            fields.UtcModified
                        },
                        where => where.Id == dbo.Id);

                    dbo = db.SingleById<DBOs.Tasks.TaskAssignedContact>(dbo.Id);
                }

                return RedirectToAction("Contacts", "Tasks", new { id = dbo.TaskId.ToString() });
            }
            catch
            {
                return View(model);
            }
        }
        
        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Tasks.TaskAssignedContactViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Tasks.TaskAssignedContact dbo = db.Single<DBOs.Tasks.TaskAssignedContact>(
                    "SELECT * FROM \"task_assigned_contact\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Tasks.Task taskDbo = db.SingleById<DBOs.Tasks.Task>(dbo.TaskId);
                DBOs.Contacts.Contact contactDbo = db.SingleById<DBOs.Contacts.Contact>(dbo.ContactId);

                model = Mapper.Map<ViewModels.Tasks.TaskAssignedContactViewModel>(dbo);
                model.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contactDbo);
                model.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(taskDbo);

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(Guid id, ViewModels.Tasks.TaskAssignedContactViewModel model)
        {
            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);

                DBOs.Tasks.TaskAssignedContact dbo = Mapper.Map<DBOs.Tasks.TaskAssignedContact>(model);
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

                    dbo = db.SingleById<DBOs.Tasks.TaskAssignedContact>(dbo.Id);
                }

                return RedirectToAction("Contacts", "Tasks", new { id = dbo.TaskId.ToString() });
            }
            catch
            {
                return View(model);
            }
        }
    }
}
