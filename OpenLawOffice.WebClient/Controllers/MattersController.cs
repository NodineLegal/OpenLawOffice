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

    [HandleError(View = "Errors/Index", Order = 10)]
    public class MattersController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            List<ViewModels.Matters.MatterViewModel> viewModelList;
            List<Common.Models.Matters.Matter> modelList;

            viewModelList = new List<ViewModels.Matters.MatterViewModel>();

            modelList = OpenLawOffice.Data.Matters.Matter.List();

            modelList.ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Matters.MatterViewModel>(x));
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.List)]
        [HttpGet]
        public ActionResult ListChildrenJqGrid(Guid? id)
        {
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.List)]
        [HttpGet]
        public ActionResult ListChildrenForContactJqGrid(Guid? id)
        {
            ViewModels.JqGridObject jqObj;
            Func<ViewModels.JqGridObject, Guid?, List<ViewModels.Matters.MatterViewModel>> func = ListChildrenJqGridBase(id, null);
            ListChildrenJqGridBase(id, ListChildrenJqGridBase(id, null));
        }

        public ViewModels.JqGridObject ListChildrenJqGridBase(Guid? id, Func<Guid?, List<ViewModels.Matters.MatterViewModel>> getChildrenMethod)
        {
            List<ViewModels.Matters.MatterViewModel> modelList;
            List<object> anonList;
            ViewModels.JqGridObject jqObject;
            int level = 0;

            if (id == null)
            {
                // jqGrid uses nodeid by default
                if (!string.IsNullOrEmpty(Request["nodeid"]))
                    id = Guid.Parse(Request["nodeid"]);
            }

            modelList = GetChildrenList(id);
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

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Matters.MatterViewModel viewModel;
            Common.Models.Matters.Matter model;

            model = Data.Matters.Matter.Get(id);

            viewModel = Mapper.Map<ViewModels.Matters.MatterViewModel>(model);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            return View();
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Matters.MatterViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Matters.Matter model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Matters.Matter>(viewModel);

            model = Data.Matters.Matter.Create(model, currentUser);

            return RedirectToAction("Index");
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Matters.MatterViewModel viewModel;
            Common.Models.Matters.Matter model;

            model = Data.Matters.Matter.Get(id);

            viewModel = Mapper.Map<ViewModels.Matters.MatterViewModel>(model);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = true,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Matters.MatterViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Matters.Matter model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Matters.Matter>(viewModel);

            model = Data.Matters.Matter.Edit(model, currentUser);

            return RedirectToAction("Index");
        }

        // A note on delete - https://github.com/NodineLegal/OpenLawOffice/wiki/Design-of-Disabling-a-Matter

        [SecurityFilter(SecurityAreaName = "Tagging", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
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

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult ResponsibleUsers(Guid id)
        {
            Common.Models.Security.User user;
            ViewModels.Matters.ResponsibleUserViewModel viewModel;
            List<ViewModels.Matters.ResponsibleUserViewModel> viewModelList;

            viewModelList = new List<ViewModels.Matters.ResponsibleUserViewModel>();

            Data.Matters.ResponsibleUser.ListForMatter(id).ForEach(x =>
            {
                user = Data.Security.User.Get(x.User.Id.Value);

                viewModel = Mapper.Map<ViewModels.Matters.ResponsibleUserViewModel>(x);
                viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(user);

                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
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

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Tasks(Guid id)
        {
            return View(TasksController.GetListForMatter(id));
        }

        [SecurityFilter(SecurityAreaName = "Notes", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
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

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Documents(Guid id)
        {
            ViewModels.Documents.DocumentViewModel viewModel;
            List<ViewModels.Documents.DocumentViewModel> viewModelList;

            viewModelList = new List<ViewModels.Documents.DocumentViewModel>();

            Data.Documents.Document.ListForMatter(id).ForEach(x =>
            {
                viewModel = Mapper.Map<ViewModels.Documents.DocumentViewModel>(x);

                viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(Data.Documents.Document.GetTask(x.Id.Value));

                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Timing", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Time(Guid id)
        {
            ViewModels.Matters.MatterTimeViewModel viewModel;
            List<Common.Models.Timing.Time> times;
            ViewModels.Matters.MatterTimeViewModel.Task task;
            ViewModels.Timing.TimeViewModel timeViewModel;
            Common.Models.Contacts.Contact contact;

            viewModel = new ViewModels.Matters.MatterTimeViewModel();
            viewModel.Tasks = new List<ViewModels.Matters.MatterTimeViewModel.Task>();

            Data.Tasks.Task.ListForMatter(id).ForEach(x =>
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
    }
}