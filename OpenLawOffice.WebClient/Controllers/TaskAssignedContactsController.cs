// -----------------------------------------------------------------------
// <copyright file="TaskAssignedContactsController.cs" company="Nodine Legal, LLC">
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
    public class TaskAssignedContactsController : BaseController
    {
        // Selects link based on Guid of Matter
        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult SelectContactToAssign(long id)
        {
            List<ViewModels.Contacts.SelectableContactViewModel> modelList;

            modelList = new List<ViewModels.Contacts.SelectableContactViewModel>();

            Data.Contacts.Contact.List().ForEach(x =>
            {
                modelList.Add(Mapper.Map<ViewModels.Contacts.SelectableContactViewModel>(x));
            });

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult AssignContact(int id)
        {
            long taskId = 0;
            ViewModels.Tasks.TaskAssignedContactViewModel vm;

            if (Request["TaskId"] == null)
                return View("InvalidRequest");

            if (!long.TryParse(Request["TaskId"], out taskId))
                return View("InvalidRequest");

            vm = new ViewModels.Tasks.TaskAssignedContactViewModel();
            vm.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(Data.Tasks.Task.Get(taskId));
            vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(id));

            return View(vm);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult AssignContact(ViewModels.Tasks.TaskAssignedContactViewModel model)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Tasks.TaskAssignedContact taskContact;

            // We need to reset the Id of the model as it is picking up the id from the route,
            // which is incorrect
            model.Id = null;

            currentUser = UserCache.Instance.Lookup(Request);

            taskContact = Data.Tasks.TaskAssignedContact.Get(model.Task.Id.Value, model.Contact.Id.Value);

            if (taskContact == null)
            { // Create
                taskContact = Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(model);
                taskContact = Data.Tasks.TaskAssignedContact.Create(taskContact, currentUser);
            }
            else
            { // Enable
                taskContact = Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(model);
                taskContact = Data.Tasks.TaskAssignedContact.Enable(taskContact, currentUser);
            }

            return RedirectToAction("Contacts", "Tasks",
                new { id = taskContact.Task.Id.Value.ToString() });
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            Common.Models.Tasks.TaskAssignedContact model;
            ViewModels.Tasks.TaskAssignedContactViewModel viewModel;

            model = Data.Tasks.TaskAssignedContact.Get(id);
            model.Task = Data.Tasks.Task.Get(model.Task.Id.Value);
            model.Contact = Data.Contacts.Contact.Get(model.Contact.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskAssignedContactViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);
            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Contact);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Tasks.TaskAssignedContactViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Tasks.TaskAssignedContact currentModel;
            Common.Models.Tasks.TaskAssignedContact model;

            currentUser = UserCache.Instance.Lookup(Request);

            currentModel = Data.Tasks.TaskAssignedContact.Get(id);

            model = Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(viewModel);
            model.Contact = currentModel.Contact;
            model.Task = currentModel.Task;

            model = Data.Tasks.TaskAssignedContact.Edit(model, currentUser);

            return RedirectToAction("Contacts", "Tasks",
                new { id = model.Task.Id.Value.ToString() });
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Tasks.TaskAssignedContactViewModel viewModel;
            Common.Models.Tasks.TaskAssignedContact model;

            model = Data.Tasks.TaskAssignedContact.Get(id);
            model.Task = Data.Tasks.Task.Get(model.Task.Id.Value);
            model.Contact = Data.Contacts.Contact.Get(model.Contact.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskAssignedContactViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);
            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Contact);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(Guid id, ViewModels.Tasks.TaskAssignedContactViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Tasks.TaskAssignedContact currentModel;
            Common.Models.Tasks.TaskAssignedContact model;

            currentUser = UserCache.Instance.Lookup(Request);

            currentModel = Data.Tasks.TaskAssignedContact.Get(id);

            model = Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(viewModel);
            model.Contact = currentModel.Contact;
            model.Task = currentModel.Task;

            model = Data.Tasks.TaskAssignedContact.Disable(model, currentUser);

            return RedirectToAction("Contacts", "Tasks",
                new { id = model.Task.Id.Value.ToString() });
        }
    }
}