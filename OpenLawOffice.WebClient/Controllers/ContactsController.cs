namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Mvc;
    using AutoMapper;

    public class ContactsController : BaseController
    {
        //
        // GET: /Contacts/
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            return View(GetList());
        }

        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult ListJqGrid()
        {
            ViewModels.JqGridObject jqObject;
            List<ViewModels.Contacts.ContactViewModel> modelList = GetList();
            List<object> anonList = new List<object>();

            modelList.ForEach(x =>
            {
                anonList.Add(new
                {
                    Id = x.Id,
                    DisplayName = x.DisplayName,
                    City = x.Address1AddressCity,
                    State = x.Address1AddressStateOrProvince
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

        private List<ViewModels.Contacts.ContactViewModel> GetList()
        {
            List<ViewModels.Contacts.ContactViewModel> modelList = new List<ViewModels.Contacts.ContactViewModel>();

            OpenLawOffice.Data.Contacts.Contact.List().ForEach(x =>
            {
                modelList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            return modelList;
        }

        //
        // GET: /Contacts/Details/5
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            Common.Models.Contacts.Contact contact = OpenLawOffice.Data.Contacts.Contact.Get(id);
            if (contact == null) return null;
            ViewModels.Contacts.ContactViewModel model = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);
            PopulateCoreDetails(model);
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
        public ActionResult Create(ViewModels.Contacts.ContactViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Contacts.Contact model = Mapper.Map<Common.Models.Contacts.Contact>(viewModel);
                model = OpenLawOffice.Data.Contacts.Contact.Create(model, currentUser);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(viewModel);
            }
        }

        //
        // GET: /Contacts/Edit/5
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            Common.Models.Contacts.Contact model = OpenLawOffice.Data.Contacts.Contact.Get(id);
            ViewModels.Contacts.ContactViewModel viewModel = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model);
            return View(model);
        }

        //
        // POST: /Contacts/Edit/5
        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Contacts.ContactViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Contacts.Contact model = Mapper.Map<Common.Models.Contacts.Contact>(viewModel);
                model = OpenLawOffice.Data.Contacts.Contact.Edit(model, currentUser);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(viewModel);
            }
        }
    }
}