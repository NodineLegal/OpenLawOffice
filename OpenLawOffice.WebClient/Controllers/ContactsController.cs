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

    public class ContactsController : BaseController
    {
        //
        // GET: /Contacts/
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            List<ViewModels.Contacts.ContactViewModel> modelList = new List<ViewModels.Contacts.ContactViewModel>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Contacts.Contact> list = db.Query<DBOs.Contacts.Contact>(
                    "SELECT * FROM \"contact\" " +
                    "WHERE \"utc_disabled\" is null");

                list.ForEach(dbo =>
                {
                    modelList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(dbo));
                });
            }

            return View(modelList);
        }

        //
        // GET: /Contacts/Details/5
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Contacts.ContactViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Contacts.Contact dbo = db.QuerySingle<DBOs.Contacts.Contact>(
                    "SELECT * FROM \"contact\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                model = Mapper.Map<ViewModels.Contacts.ContactViewModel>(dbo);

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }

        //
        // GET: /Contacts/Create
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Contacts/Create
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Contacts.ContactViewModel model)
        {
            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);

                DBOs.Contacts.Contact dbo = Mapper.Map<DBOs.Contacts.Contact>(model);
                dbo.CreatedByUserId = dbo.ModifiedByUserId = user.Id.Value;
                dbo.UtcCreated = dbo.UtcModified = DateTime.UtcNow;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    // Insert Contact
                    db.Insert<DBOs.Contacts.Contact>(dbo);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
        
        //
        // GET: /Contacts/Edit/5
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Contacts.ContactViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Contacts.Contact dbo = db.QuerySingle<DBOs.Contacts.Contact>(
                    "SELECT * FROM \"contact\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                model = Mapper.Map<ViewModels.Contacts.ContactViewModel>(dbo);
            }

            return View(model);
        }

        //
        // POST: /Contacts/Edit/5
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Contacts.ContactViewModel model)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);

                DBOs.Contacts.Contact dbo = Mapper.Map<DBOs.Contacts.Contact>(model);
                dbo.UtcModified = DateTime.UtcNow;
                dbo.ModifiedByUserId = currentUser.Id.Value;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    DBOs.Contacts.Contact currentDbo = db.GetById<DBOs.Contacts.Contact>(id);

                    // load existing, move over constant information and update the whole thing
                    // we can change this to remove the db call to load the current object later
                    // once all the fields are for sure.  However, while they might change, this 
                    // will significantly decrease maintenance requirements as the object has
                    // many properties.
                    dbo.Id = currentDbo.Id;
                    dbo.UtcCreated = currentDbo.UtcCreated;
                    dbo.CreatedByUserId = currentDbo.CreatedByUserId;
                    dbo.UtcDisabled = currentDbo.UtcDisabled;
                    dbo.DisabledByUserId = currentDbo.DisabledByUserId;

                    db.Update<DBOs.Contacts.Contact>(dbo);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        //
        // GET: /Contacts/Delete/5
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
            return View();
        }

        //
        // POST: /Contacts/Delete/5
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
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
