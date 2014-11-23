// -----------------------------------------------------------------------
// <copyright file="ContactsController.cs" company="Nodine Legal, LLC">
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
    public class ContactsController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Index()
        {
            List<ViewModels.Contacts.ContactViewModel> viewModelList = null;
            string contactFilter;


            viewModelList = new List<ViewModels.Contacts.ContactViewModel>();

            if (!string.IsNullOrEmpty(contactFilter = Request["contactFilter"]))
            {
                Data.Contacts.Contact.List(contactFilter).ForEach(x =>
                {
                    viewModelList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
                });
            }
            else
            {
                Data.Contacts.Contact.List().ForEach(x =>
                {
                    viewModelList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
                });
            }

            return View(viewModelList);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult ListDisplayNameOnly()
        {
            string term;
            List<Common.Models.Contacts.Contact> list;

            term = Request["term"];

            list = Data.Contacts.Contact.List(term.Trim());

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private List<ViewModels.Contacts.ContactViewModel> GetList()
        {
            List<ViewModels.Contacts.ContactViewModel> modelList = null;

            modelList = new List<ViewModels.Contacts.ContactViewModel>();

            OpenLawOffice.Data.Contacts.Contact.List().ForEach(x =>
            {
                modelList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            return modelList;
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Details(int id)
        {
            Common.Models.Contacts.Contact contact = null;
            ViewModels.Contacts.ContactViewModel viewModel;

            contact = OpenLawOffice.Data.Contacts.Contact.Get(id);

            if (contact == null)
                return View("InvalidRequest");

            viewModel = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);
            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Contacts.ContactViewModel viewModel)
        {
            Common.Models.Account.Users currentUser = null;
            Common.Models.Contacts.Contact model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Contacts.Contact>(viewModel);

            model = OpenLawOffice.Data.Contacts.Contact.Create(model, currentUser);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(int id)
        {
            Common.Models.Contacts.Contact model = null;
            ViewModels.Contacts.ContactViewModel viewModel;

            model = OpenLawOffice.Data.Contacts.Contact.Get(id);

            viewModel = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(int id, ViewModels.Contacts.ContactViewModel viewModel)
        {
            Common.Models.Account.Users currentUser = null;
            Common.Models.Contacts.Contact model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Contacts.Contact>(viewModel);

            model = OpenLawOffice.Data.Contacts.Contact.Edit(model, currentUser);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Conflicts(int id)
        {
            List<Common.Models.Matters.Matter> matterList = null;
            Common.Models.Contacts.Contact contact;
            List<Tuple<Common.Models.Matters.Matter, Common.Models.Matters.MatterContact, Common.Models.Contacts.Contact>> matterRelationshipList;
            ViewModels.Contacts.ConflictViewModel viewModel = new ViewModels.Contacts.ConflictViewModel();

            contact = Data.Contacts.Contact.Get(id);

            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);

            viewModel.Matters = new List<ViewModels.Contacts.ConflictViewModel.MatterRelationship>();

            matterList = Data.Contacts.Contact.ListMattersForContact(id);

            foreach (var x in matterList)
            {
                ViewModels.Contacts.ConflictViewModel.MatterRelationship mr = new ViewModels.Contacts.ConflictViewModel.MatterRelationship();

                mr.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(x);

                mr.MatterContacts = new List<ViewModels.Matters.MatterContactViewModel>();

                matterRelationshipList = Data.Contacts.Contact.ListMatterRelationshipsForContact(id, x.Id.Value);

                matterRelationshipList.ForEach(y =>
                {
                    ViewModels.Matters.MatterContactViewModel mc = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(y.Item2);
                    mc.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(y.Item3);
                    mc.Matter = mr.Matter;
                    mr.MatterContacts.Add(mc);
                });

                viewModel.Matters.Add(mr);
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Matters(int? id)
        {
            List<ViewModels.Matters.MatterViewModel> list;
            int contactId;

            if (id.HasValue)
                contactId = id.Value;
            else
                contactId = int.Parse(Request["ContactId"]);

            list = new List<ViewModels.Matters.MatterViewModel>();
            Data.Matters.Matter.ListAllMattersForContact(contactId).ForEach(x =>
            {
                list.Add(Mapper.Map<ViewModels.Matters.MatterViewModel>(x));
            });

            return View(list);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Tasks(int? id)
        {
            ViewModels.Tasks.TaskViewModel viewModel;
            List<ViewModels.Tasks.TaskViewModel> list;
            int contactId;

            if (id.HasValue)
                contactId = id.Value;
            else
                contactId = int.Parse(Request["ContactId"]);

            list = new List<ViewModels.Tasks.TaskViewModel>();
            Data.Tasks.Task.ListAllTasksForContact(contactId).ForEach(x =>
            {
                viewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(x);

                if (viewModel.IsGroupingTask)
                {
                    if (Data.Tasks.Task.GetTaskForWhichIAmTheSequentialPredecessor(x.Id.Value) != null)
                        viewModel.Type = "Sequential Group";
                    else
                        viewModel.Type = "Group";
                }
                else
                {
                    if (x.SequentialPredecessor != null)
                        viewModel.Type = "Sequential";
                    else
                        viewModel.Type = "Standard";
                }
                list.Add(viewModel);
            });

            return View(list);
        }
    }
}