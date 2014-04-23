// -----------------------------------------------------------------------
// <copyright file="MatterContactController.cs" company="Nodine Legal, LLC">
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

    [HandleError(View = "Errors/", Order = 10)]
    public class MatterContactController : BaseController
    {
        // Selects link based on Guid of Matter
        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult SelectContactToAssign(Guid id)
        {
            List<ViewModels.Contacts.SelectableContactViewModel> modelList = new List<ViewModels.Contacts.SelectableContactViewModel>();

            Data.Contacts.Contact.List().ForEach(x =>
            {
                modelList.Add(Mapper.Map<ViewModels.Contacts.SelectableContactViewModel>(x));
            });

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult AssignContact(int id)
        {
            ViewModels.Matters.MatterContactViewModel vm;
            Guid matterId = Guid.Empty;

            if (Request["MatterId"] == null)
                return View("InvalidRequest");

            if (!Guid.TryParse(Request["MatterId"], out matterId))
                return View("InvalidRequest");

            vm = new ViewModels.Matters.MatterContactViewModel();
            vm.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(Data.Matters.Matter.Get(matterId));
            vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(id));

            return View(vm);
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult AssignContact(ViewModels.Matters.MatterContactViewModel model)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Matters.MatterContact matterContact;

            // We need to reset the Id of the model as it is picking up the id from the route,
            // which is incorrect
            model.Id = null;

            currentUser = UserCache.Instance.Lookup(Request);

            matterContact = Data.Matters.MatterContact.Get(model.Matter.Id.Value, model.Contact.Id.Value);

            if (matterContact == null)
            { // Create
                matterContact = Mapper.Map<Common.Models.Matters.MatterContact>(model);
                matterContact = Data.Matters.MatterContact.Create(matterContact, currentUser);
            }
            else
            { // Enable
                matterContact = Mapper.Map<Common.Models.Matters.MatterContact>(model);
                matterContact = Data.Matters.MatterContact.Enable(matterContact, currentUser);
            }

            return RedirectToAction("Contacts", "Matters",
                new { id = matterContact.Matter.Id.Value.ToString() });
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Matters.MatterContactViewModel viewModel;
            Common.Models.Matters.MatterContact model;

            model = Data.Matters.MatterContact.Get(id);
            model.Matter = Data.Matters.Matter.Get(model.Matter.Id.Value);
            model.Contact = Data.Contacts.Contact.Get(model.Contact.Id.Value);

            viewModel = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(model);
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter);
            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Contact);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Matters.MatterContactViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Matters.MatterContact model;

            currentUser = UserCache.Instance.Lookup(Request);
            model = Mapper.Map<Common.Models.Matters.MatterContact>(viewModel);
            model = Data.Matters.MatterContact.Edit(model, currentUser);

            return RedirectToAction("Contacts", "Matters",
                new { id = model.Matter.Id.Value.ToString() });
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Matters.MatterContactViewModel viewModel;
            Common.Models.Matters.MatterContact model;

            model = Data.Matters.MatterContact.Get(id);
            model.Matter = Data.Matters.Matter.Get(model.Matter.Id.Value);
            model.Contact = Data.Contacts.Contact.Get(model.Contact.Id.Value);

            viewModel = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(model);
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter);
            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Contact);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(int id)
        {
            return Details(id);
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(int id, ViewModels.Matters.MatterContactViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Matters.MatterContact model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Matters.MatterContact>(viewModel);

            model = Data.Matters.MatterContact.Disable(model, currentUser);

            return RedirectToAction("Contacts", "Matters",
                new { id = model.Matter.Id.Value.ToString() });
        }
    }
}