namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using AutoMapper;

    public class MattersController : BaseController
    {
        //
        // GET: /Matter/
        [SecurityFilter(SecurityAreaName="Matters.Matter", IsSecuredResource=true, 
            Permission=Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            List<ViewModels.Matters.MatterViewModel> viewModelList = new List<ViewModels.Matters.MatterViewModel>();
            List<Common.Models.Matters.Matter> modelList = OpenLawOffice.Data.Matters.Matter.List();

            modelList.ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Matters.MatterViewModel>(x));
            });

            return View(viewModelList);
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
            List<ViewModels.Matters.MatterViewModel> viewModelList = new List<ViewModels.Matters.MatterViewModel>();
            List<Common.Models.Matters.Matter> modelList = OpenLawOffice.Data.Matters.Matter.ListChildren(id);

            modelList.ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Matters.MatterViewModel>(x));
            });

            return viewModelList;
        }

        //
        // GET: /Matter/Details/9acb1b4f-0442-4c9b-a550-ad7478e36fb2
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Matters.MatterViewModel viewModel = null;
            Common.Models.Matters.Matter model = OpenLawOffice.Data.Matters.Matter.Get(id);
            viewModel = Mapper.Map<ViewModels.Matters.MatterViewModel>(model);
            PopulateCoreDetails(viewModel);
            return View(viewModel);
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
        public ActionResult Create(ViewModels.Matters.MatterViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Matters.Matter model = Mapper.Map<Common.Models.Matters.Matter>(viewModel);
                model = OpenLawOffice.Data.Matters.Matter.Create(model, currentUser);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(viewModel);
            }
        }
        
        //
        // GET: /Matter/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Matters.MatterViewModel viewModel = null;
            Common.Models.Matters.Matter model = OpenLawOffice.Data.Matters.Matter.Get(id);
            viewModel = Mapper.Map<ViewModels.Matters.MatterViewModel>(id);
            return View(viewModel);
        }

        //
        // POST: /Matter/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.Matter", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Matters.MatterViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Matters.Matter model = Mapper.Map<Common.Models.Matters.Matter>(viewModel);
                model = OpenLawOffice.Data.Matters.Matter.Edit(model, currentUser);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(viewModel);
            }
        }

        // A note on delete - https://github.com/NodineLegal/OpenLawOffice/wiki/Design-of-Disabling-a-Matter


        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Tags(Guid id)
        {
            List<ViewModels.Matters.MatterTagViewModel> viewModelList = new List<ViewModels.Matters.MatterTagViewModel>();
            List<Common.Models.Matters.MatterTag> modelList = OpenLawOffice.Data.Matters.MatterTag.ListForMatter(id);

            modelList.ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Matters.MatterTagViewModel>(x));
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult ResponsibleUsers(Guid id)
        {
            List<ViewModels.Matters.ResponsibleUserViewModel> viewModelList = new List<ViewModels.Matters.ResponsibleUserViewModel>();
            List<Common.Models.Matters.ResponsibleUser> modelList = OpenLawOffice.Data.Matters.ResponsibleUser.ListForMatter(id);

            modelList.ForEach(x =>
            {
                Common.Models.Security.User user = OpenLawOffice.Data.Security.User.Get(x.User.Id.Value);
                ViewModels.Matters.ResponsibleUserViewModel viewModel = Mapper.Map<ViewModels.Matters.ResponsibleUserViewModel>(x);
                viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(user);
                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Contacts(Guid id)
        {
            List<ViewModels.Matters.MatterContactViewModel> viewModelList = new List<ViewModels.Matters.MatterContactViewModel>();
            List<Common.Models.Matters.MatterContact> modelList = OpenLawOffice.Data.Matters.MatterContact.ListForMatter(id);

            modelList.ForEach(x =>
            {
                ViewModels.Matters.MatterContactViewModel viewModel = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(x);
                Common.Models.Contacts.Contact contact = OpenLawOffice.Data.Contacts.Contact.Get(x.Contact.Id.Value);
                viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);
                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.Task", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Tasks(Guid id)
        {
            List<ViewModels.Tasks.TaskViewModel> modelList = TasksController.GetListForMatter(id);

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Notes.Note", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Notes(Guid id)
        {
            List<ViewModels.Notes.NoteViewModel> viewModelList = new List<ViewModels.Notes.NoteViewModel>();
            List<Common.Models.Notes.Note> modelList = Data.Notes.Note.ListForMatter(id);

            modelList.ForEach(x =>
            {
                ViewModels.Notes.NoteViewModel viewModel = Mapper.Map<ViewModels.Notes.NoteViewModel>(x);
                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Documents.Document", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Documents(Guid id)
        {
            List<ViewModels.Documents.DocumentViewModel> viewModelList = new List<ViewModels.Documents.DocumentViewModel>();
            List<Common.Models.Documents.Document> modelList = Data.Documents.Document.ListForMatter(id);

            modelList.ForEach(x =>
            {
                ViewModels.Documents.DocumentViewModel viewModel = Mapper.Map<ViewModels.Documents.DocumentViewModel>(x);
                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Timing.Time", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Time(Guid id)
        {
            List<ViewModels.Timing.TimeViewModel> viewModelList = new List<ViewModels.Timing.TimeViewModel>();
            List<Common.Models.Timing.Time> modelList = OpenLawOffice.Data.Timing.Time.ListForTask(id);

            modelList.ForEach(x =>
            {
                ViewModels.Timing.TimeViewModel viewModel = Mapper.Map<ViewModels.Timing.TimeViewModel>(x);
                Common.Models.Contacts.Contact contact = OpenLawOffice.Data.Contacts.Contact.Get(viewModel.Worker.Id.Value);
                viewModel.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);
                viewModel.WorkerDisplayName = viewModel.Worker.DisplayName;
                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }
    }
}
