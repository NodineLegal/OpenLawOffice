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

    public class MatterContactController : BaseController
    {
        // Selects link based on Guid of Matter
        [SecurityFilter(SecurityAreaName = "Matters.MatterContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult SelectContactToAssign(Guid id)
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

        [SecurityFilter(SecurityAreaName = "Matters.MatterContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult AssignContact(int id)
        {
            Guid matterId = Guid.Empty;

            if (Request["MatterId"] == null)
                return View("InvalidRequest");

            if (!Guid.TryParse(Request["MatterId"], out matterId))
                return View("InvalidRequest");

            ViewModels.Matters.MatterContactViewModel vm = new ViewModels.Matters.MatterContactViewModel();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                DBOs.Matters.Matter matterDbo = db.Single<DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = matterId });
                DBOs.Contacts.Contact contactDbo = db.Single<DBOs.Contacts.Contact>(
                    "SELECT * FROM \"contact\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                vm.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matterDbo);
                vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contactDbo);
            }

            return View(vm);
        }

        [SecurityFilter(SecurityAreaName = "Matters.MatterContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult AssignContact(ViewModels.Matters.MatterContactViewModel model)
        {
            // We need to reset the Id of the model as it is picking up the id from the route, 
            // which is incorrect
            model.Id = null;
            DBOs.Matters.MatterContact dbo = null;

            Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
            using (IDbConnection db = Database.Instance.OpenConnection())
            {

                dbo = db.Single<DBOs.Matters.MatterContact>(
                    "SELECT * FROM \"matter_contact\" WHERE \"matter_id\"=@MatterId AND \"contact_id\"=@ContactId",
                    new { MatterId = model.Matter.Id, ContactId = model.Contact.Id });

                if (dbo == null)
                {
                    dbo = Mapper.Map<DBOs.Matters.MatterContact>(model);
                    dbo.CreatedByUserId = dbo.ModifiedByUserId = currentUser.Id.Value;
                    dbo.UtcCreated = dbo.UtcModified = DateTime.UtcNow;
                    db.Insert<DBOs.Matters.MatterContact>(dbo);
                    dbo.Id = (int)db.LastInsertId();
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

                dbo = db.SingleById<DBOs.Matters.MatterContact>(dbo.Id);
            }

            return RedirectToAction("Contacts", "Matters", new { id = dbo.MatterId.ToString() });
        }

        //
        // GET: /MatterContact/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.MatterContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Matters.MatterContactViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.MatterContact dbo = db.Single<DBOs.Matters.MatterContact>(
                    "SELECT * FROM \"matter_contact\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Matters.Matter matterDbo = db.SingleById<DBOs.Matters.Matter>(dbo.MatterId);
                DBOs.Contacts.Contact contactDbo = db.SingleById<DBOs.Contacts.Contact>(dbo.ContactId);

                model = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(dbo);
                model.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contactDbo);
                model.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matterDbo);
            }

            return View(model);
        }

        //
        // POST: /MatterContact/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.MatterContact", IsSecuredResource = false,
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

                    dbo = db.SingleById<DBOs.Matters.MatterContact>(dbo.Id);
                }

                return RedirectToAction("Contacts", "Matters", new { id = dbo.MatterId.ToString() });
            }
            catch
            {
                return View(model);
            }
        }

        //
        // GET: /MatterContact/Details/5
        [SecurityFilter(SecurityAreaName = "Matters.MatterContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Matters.MatterContactViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Matters.MatterContact dbo = db.Single<DBOs.Matters.MatterContact>(
                    "SELECT * FROM \"matter_contact\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Matters.Matter matterDbo = db.SingleById<DBOs.Matters.Matter>(dbo.MatterId);
                DBOs.Contacts.Contact contactDbo = db.SingleById<DBOs.Contacts.Contact>(dbo.ContactId);

                model = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(dbo);
                model.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contactDbo);
                model.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matterDbo);

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }

        //
        // GET: /MatterContact/Delete/5
        [SecurityFilter(SecurityAreaName = "Matters.MatterContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(int id)
        {
            return Details(id);
        }

        //
        // POST: /MatterContact/Delete/5
        [SecurityFilter(SecurityAreaName = "Matters.MatterContact", IsSecuredResource = false,
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

                    dbo = db.SingleById<DBOs.Matters.MatterContact>(dbo.Id);
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
