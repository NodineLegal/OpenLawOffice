// -----------------------------------------------------------------------
// <copyright file="EventAssignedContactController.cs" company="Nodine Legal, LLC">
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
    public class EventAssignedContactController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult SelectContactToAssign(Guid id)
        {
            List<ViewModels.Contacts.SelectableContactViewModel> modelList;

            modelList = new List<ViewModels.Contacts.SelectableContactViewModel>();

            Data.Contacts.Contact.List().ForEach(x =>
            {
                modelList.Add(Mapper.Map<ViewModels.Contacts.SelectableContactViewModel>(x));
            });

            return View(modelList);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult AssignContact(int id)
        {
            Guid eventId = Guid.Empty;
            ViewModels.Events.EventAssignedContactViewModel vm;

            if (Request["EventId"] == null)
                return View("InvalidRequest");

            if (!Guid.TryParse(Request["EventId"], out eventId))
                return View("InvalidRequest");

            vm = new ViewModels.Events.EventAssignedContactViewModel();
            vm.Event = Mapper.Map<ViewModels.Events.EventViewModel>(Data.Events.Event.Get(eventId));
            vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(id));

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult AssignContact(ViewModels.Events.EventAssignedContactViewModel model)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Events.EventAssignedContact eventContact;

            // We need to reset the Id of the model as it is picking up the id from the route,
            // which is incorrect
            model.Id = null;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            eventContact = Data.Events.EventAssignedContact.Get(model.Event.Id.Value, model.Contact.Id.Value);

            if (eventContact == null)
            { // Create
                eventContact = Mapper.Map<Common.Models.Events.EventAssignedContact>(model);
                eventContact = Data.Events.EventAssignedContact.Create(eventContact, currentUser);
            }
            else
            { // Enable
                eventContact = Mapper.Map<Common.Models.Events.EventAssignedContact>(model);
                eventContact = Data.Events.EventAssignedContact.Enable(eventContact, currentUser);
            }

            return RedirectToAction("Contacts", "Events",
                new { id = eventContact.Event.Id.Value.ToString() });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id)
        {
            Common.Models.Events.EventAssignedContact model;
            ViewModels.Events.EventAssignedContactViewModel viewModel;

            model = Data.Events.EventAssignedContact.Get(id);
            model.Event = Data.Events.Event.Get(model.Event.Id.Value);
            model.Contact = Data.Contacts.Contact.Get(model.Contact.Id.Value);

            viewModel = Mapper.Map<ViewModels.Events.EventAssignedContactViewModel>(model);
            viewModel.Event = Mapper.Map<ViewModels.Events.EventViewModel>(model.Event);
            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Contact);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id, ViewModels.Events.EventAssignedContactViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Events.EventAssignedContact currentModel;
            Common.Models.Events.EventAssignedContact model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            currentModel = Data.Events.EventAssignedContact.Get(id);

            model = Mapper.Map<Common.Models.Events.EventAssignedContact>(viewModel);
            model.Contact = currentModel.Contact;
            model.Event = currentModel.Event;

            model = Data.Events.EventAssignedContact.Edit(model, currentUser);

            return RedirectToAction("Contacts", "Events",
                new { id = model.Event.Id.Value.ToString() });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Details(Guid id)
        {
            ViewModels.Events.EventAssignedContactViewModel viewModel;
            Common.Models.Events.EventAssignedContact model;

            model = Data.Events.EventAssignedContact.Get(id);
            model.Event = Data.Events.Event.Get(model.Event.Id.Value);
            model.Contact = Data.Contacts.Contact.Get(model.Contact.Id.Value);

            viewModel = Mapper.Map<ViewModels.Events.EventAssignedContactViewModel>(model);
            viewModel.Event = Mapper.Map<ViewModels.Events.EventViewModel>(model.Event);
            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Contact);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(Guid id, ViewModels.Tasks.TaskAssignedContactViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Events.EventAssignedContact currentModel;
            Common.Models.Events.EventAssignedContact model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            currentModel = Data.Events.EventAssignedContact.Get(id);

            model = Mapper.Map<Common.Models.Events.EventAssignedContact>(viewModel);
            model.Contact = currentModel.Contact;
            model.Event = currentModel.Event;

            model = Data.Events.EventAssignedContact.Disable(model, currentUser);

            return RedirectToAction("Contacts", "Events",
                new { id = model.Event.Id.Value.ToString() });
        }
    }
}
