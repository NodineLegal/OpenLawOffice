// -----------------------------------------------------------------------
// <copyright file="EventsController.cs" company="Nodine Legal, LLC">
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
    using System.Web.Mvc;
    using AutoMapper;
    using System.Collections.Generic;

    public class EventsController : BaseController
    {

        public ActionResult SelectUser()
        {
            List<ViewModels.Security.UserViewModel> usersList;

            usersList = new List<ViewModels.Security.UserViewModel>();

            Data.Security.User.List().ForEach(x =>
            {
                usersList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
            });

            return View(usersList);
        }

        public ActionResult SelectContact()
        {
            List<ViewModels.Contacts.ContactViewModel> contactList;

            contactList = new List<ViewModels.Contacts.ContactViewModel>();

            Data.Contacts.Contact.List().ForEach(x =>
            {
                contactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            return View(contactList);
        }

        public ActionResult ContactAgenda(int id)
        {
            List<ViewModels.Events.EventViewModel> list;
            Common.Models.Contacts.Contact contact;

            contact = Data.Contacts.Contact.Get(id);

            list = new List<ViewModels.Events.EventViewModel>();

            Data.Events.Event.ListForContact(id, DateTime.Now, null).ForEach(x =>
            {
                list.Add(Mapper.Map<ViewModels.Events.EventViewModel>(x));
            });

            ViewData["ContactDisplayName"] = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact).DisplayName;

            return View(list);
        }

        public ActionResult UserAgenda(int id)
        {
            List<ViewModels.Events.EventViewModel> list;
            Common.Models.Security.User user;

            user = Data.Security.User.Get(id);

            list = new List<ViewModels.Events.EventViewModel>();

            Data.Events.Event.ListForUser(id, DateTime.Now, null).ForEach(x =>
            {
                list.Add(Mapper.Map<ViewModels.Events.EventViewModel>(x));
            });

            ViewData["Username"] = Mapper.Map<ViewModels.Security.UserViewModel>(user).Username;

            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModels.Events.EventViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Events.Event model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Events.Event>(viewModel);

            model = Data.Events.Event.Create(model, currentUser);

            return RedirectToAction("Details", new { Id = model.Id });
        }

        public ActionResult Edit(Guid id)
        {
            ViewModels.Events.EventViewModel viewModel;

            viewModel = Mapper.Map<ViewModels.Events.EventViewModel>(Data.Events.Event.Get(id));

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Events.EventViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Events.Event model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Events.Event>(viewModel);

            model = Data.Events.Event.Edit(model, currentUser);

            return RedirectToAction("Details", new { Id = id });
        }

        public ActionResult Details(Guid id)
        {
            ViewModels.Events.EventViewModel viewModel;

            viewModel = Mapper.Map<ViewModels.Events.EventViewModel>(Data.Events.Event.Get(id));
            
            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        public ActionResult Contacts(Guid id)
        {
            List<ViewModels.Events.EventAssignedContactViewModel> list;

            list = new List<ViewModels.Events.EventAssignedContactViewModel>();

            Data.Events.EventAssignedContact.ListForEvent(id).ForEach(x =>
            {
                ViewModels.Events.EventAssignedContactViewModel vm = Mapper.Map<ViewModels.Events.EventAssignedContactViewModel>(x);
                vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(x.Contact.Id.Value));
                list.Add(vm);
            });

            return View(list);
        }

        public ActionResult ResponsibleUsers(Guid id)
        {
            List<ViewModels.Events.EventResponsibleUserViewModel> list;

            list = new List<ViewModels.Events.EventResponsibleUserViewModel>();

            Data.Events.EventResponsibleUser.ListForEvent(id).ForEach(x =>
            {
                ViewModels.Events.EventResponsibleUserViewModel vm = Mapper.Map<ViewModels.Events.EventResponsibleUserViewModel>(x);
                vm.User = Mapper.Map<ViewModels.Security.UserViewModel>(Data.Security.User.Get(x.User.Id.Value));
                list.Add(vm);
            });

            return View(list);
        }

        public ActionResult Matters(Guid id)
        {
            List<ViewModels.Matters.MatterViewModel> list;

            list = new List<ViewModels.Matters.MatterViewModel>();

            Data.Events.EventMatter.ListForEvent(id).ForEach(x =>
            {
                ViewModels.Matters.MatterViewModel vm = Mapper.Map<ViewModels.Matters.MatterViewModel>(x);
                list.Add(vm);
            });

            return View(list);
        }

        public ActionResult Notes(Guid id)
        {
            List<ViewModels.Notes.NoteViewModel> list;

            list = new List<ViewModels.Notes.NoteViewModel>();

            Data.Events.EventNote.ListForEvent(id).ForEach(x =>
            {
                ViewModels.Notes.NoteViewModel vm = Mapper.Map<ViewModels.Notes.NoteViewModel>(x);
                list.Add(vm);
            });

            return View(list);
        }

        public ActionResult Tags(Guid id)
        {
            List<ViewModels.Events.EventTagViewModel> list;

            list = new List<ViewModels.Events.EventTagViewModel>();

            Data.Events.EventTag.ListForEvent(id).ForEach(x =>
            {
                ViewModels.Events.EventTagViewModel vm = Mapper.Map<ViewModels.Events.EventTagViewModel>(x);
                list.Add(vm);
            });

            return View(list);
        }
    }
}