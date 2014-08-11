// -----------------------------------------------------------------------
// <copyright file="MattersController.cs" company="Nodine Legal, LLC">
// Licensed to Nodine Legal, LLC under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  Nodine Legal, LLC licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;
    using Ionic.Zip;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class MattersController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Index()
        {
            List<ViewModels.Matters.MatterViewModel> viewModelList;
            bool? active;
            string activeStr;

            switch (activeStr = Request["active"])
            {
                case "inactive":
                    active = false;
                    break;
                case "both":
                    active = null;
                    break;
                default:
                    active = true;
                    break;
            }

            viewModelList = new List<ViewModels.Matters.MatterViewModel>();

            Data.Matters.Matter.List(active).ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Matters.MatterViewModel>(x));
            });

            return View(viewModelList);
        }

        [HttpGet]
        [Authorize(Roles = "Login, User")]
        public ActionResult ListChildrenJqGrid(Guid? id)
        {
            ViewModels.JqGridObject jqObject;

            jqObject = ListChildrenJqGridObject(id, x => GetChildrenList(x));

            return Json(jqObject, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Login, User")]
        public ActionResult ListChildrenForContactJqGrid(Guid? id)
        {
            ViewModels.JqGridObject jqObject;
            int contactId;

            contactId = int.Parse(Request["ContactId"]);

            jqObject = ListChildrenJqGridObject(id, x => GetChildrenListForContact(x, contactId));

            return Json(jqObject, JsonRequestBehavior.AllowGet);
        }

        private ViewModels.JqGridObject ListChildrenJqGridObject(Guid? id, Func<Guid?, List<ViewModels.Matters.MatterViewModel>> act)
        {
            List<ViewModels.Matters.MatterViewModel> modelList;
            List<object> anonList;
            int level = 0;

            if (id == null)
            {
                // jqGrid uses nodeid by default
                if (!string.IsNullOrEmpty(Request["nodeid"]))
                    id = Guid.Parse(Request["nodeid"]);
            }

            modelList = act(id);
            //modelList = GetChildrenList(id);
            anonList = new List<object>();

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

            return new ViewModels.JqGridObject()
            {
                TotalPages = 1,
                CurrentPage = 1,
                TotalRecords = modelList.Count,
                Rows = anonList.ToArray()
            };
        }

        private List<ViewModels.Matters.MatterViewModel> GetChildrenList(Guid? id)
        {
            List<ViewModels.Matters.MatterViewModel> viewModelList;

            viewModelList = new List<ViewModels.Matters.MatterViewModel>();

            Data.Matters.Matter.ListChildren(id).ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Matters.MatterViewModel>(x));
            });

            return viewModelList;
        }

        private List<ViewModels.Matters.MatterViewModel> GetChildrenListForContact(Guid? id, int contactId)
        {
            List<ViewModels.Matters.MatterViewModel> viewModelList;

            viewModelList = new List<ViewModels.Matters.MatterViewModel>();

            Data.Matters.Matter.ListChildrenForContact(id, contactId).ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Matters.MatterViewModel>(x));
            });

            return viewModelList;
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Details(Guid id)
        {
            ViewModels.Matters.MatterViewModel viewModel;
            Common.Models.Matters.Matter model;

            model = Data.Matters.Matter.Get(id);
            model.LeadAttorney = Data.Contacts.Contact.Get(model.LeadAttorney.Id.Value);

            viewModel = Mapper.Map<ViewModels.Matters.MatterViewModel>(model);
            viewModel.Tasks = TasksController.GetListForMatter(id, true);
            viewModel.LeadAttorney = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.LeadAttorney);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create()
        {
            List<ViewModels.Account.UsersViewModel> userList;
            List<ViewModels.Contacts.ContactViewModel> employeeContactList;

            userList = new List<ViewModels.Account.UsersViewModel>();
            employeeContactList = new List<ViewModels.Contacts.ContactViewModel>();

            Data.Account.Users.List().ForEach(x =>
            {
                userList.Add(Mapper.Map<ViewModels.Account.UsersViewModel>(x));
            });

            Data.Contacts.Contact.ListEmployeesOnly().ForEach(x =>
            {
                employeeContactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            ViewData["UserList"] = userList;
            ViewData["EmployeeContactList"] = employeeContactList;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Matters.CreateMatterViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Matters.Matter model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Matters.Matter>(viewModel.Matter);

            if (viewModel.LeadAttorney == null || viewModel.LeadAttorney.Contact == null ||
                !viewModel.LeadAttorney.Contact.Id.HasValue)
            {
                List<ViewModels.Account.UsersViewModel> userList;
                List<ViewModels.Contacts.ContactViewModel> employeeContactList;

                userList = new List<ViewModels.Account.UsersViewModel>();
                employeeContactList = new List<ViewModels.Contacts.ContactViewModel>();

                Data.Account.Users.List().ForEach(x =>
                {
                    userList.Add(Mapper.Map<ViewModels.Account.UsersViewModel>(x));
                });

                Data.Contacts.Contact.ListEmployeesOnly().ForEach(x =>
                {
                    employeeContactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
                });

                ModelState.AddModelError("LeadAttorney", "Lead Attorney is required");

                ViewData["UserList"] = userList;
                ViewData["EmployeeContactList"] = employeeContactList;
                return View(viewModel);
            }

            model.LeadAttorney = Mapper.Map<Common.Models.Contacts.Contact>(viewModel.LeadAttorney.Contact);

            // Lead Attorney is created within this method
            model = Data.Matters.Matter.Create(model, currentUser);

            Data.Matters.ResponsibleUser.Create(new Common.Models.Matters.ResponsibleUser()
            {
                Matter = model,
                User = new Common.Models.Account.Users() { PId = viewModel.ResponsibleUser.User.PId },
                Responsibility = viewModel.ResponsibleUser.Responsibility
            }, currentUser);

            return RedirectToAction("Details", new { Id = model.Id });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id)
        {
            List<ViewModels.Contacts.ContactViewModel> employeeContactList;
            ViewModels.Matters.EditMatterViewModel viewModel;
            Common.Models.Matters.Matter model;

            employeeContactList = new List<ViewModels.Contacts.ContactViewModel>();
            model = Data.Matters.Matter.Get(id);
            model.LeadAttorney = Data.Contacts.Contact.Get(model.LeadAttorney.Id.Value);
            
            Data.Contacts.Contact.ListEmployeesOnly().ForEach(x =>
            {
                employeeContactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            viewModel = new ViewModels.Matters.EditMatterViewModel();
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model);
            viewModel.LeadAttorney = new ViewModels.Matters.MatterContactViewModel();
            viewModel.LeadAttorney.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.LeadAttorney);

            ViewData["EmployeeContactList"] = employeeContactList;
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id, ViewModels.Matters.EditMatterViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Matters.Matter model;

            viewModel.Matter.Id = id;

            if (viewModel.LeadAttorney == null || viewModel.LeadAttorney.Contact == null ||
                !viewModel.LeadAttorney.Contact.Id.HasValue)
            {
                List<ViewModels.Contacts.ContactViewModel> employeeContactList;

                employeeContactList = new List<ViewModels.Contacts.ContactViewModel>();

                Data.Contacts.Contact.ListEmployeesOnly().ForEach(x =>
                {
                    employeeContactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
                });

                ModelState.AddModelError("LeadAttorney", "Lead Attorney is required");

                ViewData["EmployeeContactList"] = employeeContactList;
                return View(viewModel);
            }

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            viewModel.Matter.LeadAttorney = viewModel.LeadAttorney.Contact;
            model = Mapper.Map<Common.Models.Matters.Matter>(viewModel.Matter);

            model = Data.Matters.Matter.Edit(model, currentUser);

            return RedirectToAction("Index");
        }

        // A note on delete - https://github.com/NodineLegal/OpenLawOffice/wiki/Design-of-Disabling-a-Matter

        [Authorize(Roles = "Login, User")]
        public ActionResult Tags(Guid id)
        {
            List<ViewModels.Matters.MatterTagViewModel> viewModelList;

            viewModelList = new List<ViewModels.Matters.MatterTagViewModel>();

            Data.Matters.MatterTag.ListForMatter(id).ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Matters.MatterTagViewModel>(x));
            });

            return View(viewModelList);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult ResponsibleUsers(Guid id)
        {
            Common.Models.Account.Users user;
            ViewModels.Matters.ResponsibleUserViewModel viewModel;
            List<ViewModels.Matters.ResponsibleUserViewModel> viewModelList;

            viewModelList = new List<ViewModels.Matters.ResponsibleUserViewModel>();

            Data.Matters.ResponsibleUser.ListForMatter(id).ForEach(x =>
            {
                user = Data.Account.Users.Get(x.User.PId.Value);

                viewModel = Mapper.Map<ViewModels.Matters.ResponsibleUserViewModel>(x);
                viewModel.User = Mapper.Map<ViewModels.Account.UsersViewModel>(user);

                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Contacts(Guid id)
        {
            Common.Models.Contacts.Contact contact;
            ViewModels.Matters.MatterContactViewModel viewModel;
            List<ViewModels.Matters.MatterContactViewModel> viewModelList;

            viewModelList = new List<ViewModels.Matters.MatterContactViewModel>();

            Data.Matters.MatterContact.ListForMatter(id).ForEach(x =>
            {
                contact = OpenLawOffice.Data.Contacts.Contact.Get(x.Contact.Id.Value);

                viewModel = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(x);
                viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);

                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Tasks(Guid id)
        {
            bool? active;
            string activeStr = Request["active"];
            switch (activeStr)
            {
                case "inactive":
                    active = false;
                    break;
                case "both":
                    active = null;
                    break;
                default:
                    active = true;
                    break;
            }
            return View(TasksController.GetListForMatter(id, active));
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Notes(Guid id)
        {
            ViewModels.Notes.NoteViewModel viewModel;
            List<ViewModels.Notes.NoteViewModel> viewModelList;

            viewModelList = new List<ViewModels.Notes.NoteViewModel>();

            Data.Notes.Note.ListForMatter(id).ForEach(x =>
            {
                viewModel = Mapper.Map<ViewModels.Notes.NoteViewModel>(x);

                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Documents(Guid id)
        {
            Common.Models.Documents.Version version;
            ViewModels.Documents.SelectableDocumentViewModel viewModel;
            List<ViewModels.Documents.SelectableDocumentViewModel> viewModelList;

            viewModelList = new List<ViewModels.Documents.SelectableDocumentViewModel>();

            Data.Documents.Document.ListForMatter(id).ForEach(x =>
            {
                version = Data.Documents.Document.GetCurrentVersion(x.Id.Value);

                viewModel = Mapper.Map<ViewModels.Documents.SelectableDocumentViewModel>(x);

                viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(Data.Documents.Document.GetTask(x.Id.Value));
                viewModel.Extension = version.Extension;

                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Documents(Guid id, List<ViewModels.Documents.SelectableDocumentViewModel> viewModelListNull)
        {
            string zipTitle;
            int inc = 0;
            string path;
            Guid temp;
            List<Common.Models.Documents.Document> documents;
            Common.Models.Matters.Matter matter;
            Common.Models.Documents.Version version;
            ViewModels.Documents.SelectableDocumentViewModel viewModel;
            List<ViewModels.Documents.SelectableDocumentViewModel> viewModelList;

            viewModelList = new List<ViewModels.Documents.SelectableDocumentViewModel>();

            matter = Data.Matters.Matter.Get(id);

            documents = Data.Documents.Document.ListForMatter(id);

            using (ZipFile zip = new ZipFile())
            {
                //zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                documents.ForEach(x =>
                {
                    version = Data.Documents.Document.GetCurrentVersion(x.Id.Value);

                    viewModel = Mapper.Map<ViewModels.Documents.SelectableDocumentViewModel>(x);

                    viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(Data.Documents.Document.GetTask(x.Id.Value));

                    viewModelList.Add(viewModel);

                    if (Request["CB_" + x.Id.Value.ToString()] == "on")
                    {

                        path = Common.Settings.Manager.Instance.FileStorage.GetCurrentVersionFilepathFor(version.Id.Value + "." + version.Extension);

                        zipTitle = x.Title + "." + version.Extension;

                        if (zip[zipTitle] != null)
                        {
                            inc = 1;
                            zipTitle = x.Title + " (" + inc.ToString() + ")" + "." + version.Extension;
                            while (zip[zipTitle] != null)
                            {
                                inc++;
                                zipTitle = x.Title + " (" + inc.ToString() + ")" + "." + version.Extension;
                            }
                        }

                        ZipEntry ze = zip.AddFile(path, "");
                        ze.FileName = zipTitle;
                    }
                });

                if (!System.IO.Directory.Exists(Common.Settings.Manager.Instance.FileStorage.TempPath))
                    System.IO.Directory.CreateDirectory(Common.Settings.Manager.Instance.FileStorage.TempPath);

                temp = Guid.NewGuid();

                if (zip.Count > 0)
                    zip.Save(Common.Settings.Manager.Instance.FileStorage.TempPath + temp.ToString() + ".zip");
                else
                    return View(viewModelList);
            }


            return new DeletingFileResult(Common.Settings.Manager.Instance.FileStorage.TempPath + temp.ToString() + ".zip", matter.Title + ".zip");
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Time(Guid id)
        {
            ViewModels.Matters.MatterTimeViewModel viewModel;
            List<Common.Models.Timing.Time> times;
            ViewModels.Matters.MatterTimeViewModel.Task task;
            ViewModels.Timing.TimeViewModel timeViewModel;
            Common.Models.Contacts.Contact contact;

            viewModel = new ViewModels.Matters.MatterTimeViewModel();
            viewModel.Tasks = new List<ViewModels.Matters.MatterTimeViewModel.Task>();

            Data.Tasks.Task.ListForMatter(id, null).ForEach(x =>
            {
                times = Data.Timing.Time.ListForTask(x.Id.Value);

                if (times != null && times.Count > 0)
                {
                    task = new ViewModels.Matters.MatterTimeViewModel.Task()
                    {
                        Id = x.Id.Value,
                        Title = x.Title
                    };

                    task.Times = new List<ViewModels.Timing.TimeViewModel>();

                    times.ForEach(y =>
                    {
                        timeViewModel = Mapper.Map<ViewModels.Timing.TimeViewModel>(y);

                        contact = Data.Contacts.Contact.Get(timeViewModel.Worker.Id.Value);

                        timeViewModel.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);
                        timeViewModel.WorkerDisplayName = timeViewModel.Worker.DisplayName;

                        task.Times.Add(timeViewModel);
                    });

                    viewModel.Tasks.Add(task);
                }
            });

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Events(Guid id)
        {
            ViewModels.Events.EventViewModel viewModel;
            List<ViewModels.Events.EventViewModel> viewModelList;

            viewModelList = new List<ViewModels.Events.EventViewModel>();

            Data.Events.Event.ListForMatter(id).ForEach(x =>
            {
                viewModel = Mapper.Map<ViewModels.Events.EventViewModel>(x);

                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }
    }
}