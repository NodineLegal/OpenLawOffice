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
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using AutoMapper;

    public class MatterContactController : BaseController
    {
        // Selects link based on Guid of Matter
        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult SelectContactToAssign(Guid id)
        {
            List<ViewModels.Contacts.SelectableContactViewModel> modelList = new List<ViewModels.Contacts.SelectableContactViewModel>();
            List<Common.Models.Contacts.Contact> contactList = OpenLawOffice.Data.Contacts.Contact.List();

            contactList.ForEach(x =>
            {
                modelList.Add(Mapper.Map<ViewModels.Contacts.SelectableContactViewModel>(x));
            });

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult AssignContact(int id)
        {
            Guid matterId = Guid.Empty;

            if (Request["MatterId"] == null)
                return View("InvalidRequest");

            if (!Guid.TryParse(Request["MatterId"], out matterId))
                return View("InvalidRequest");

            ViewModels.Matters.MatterContactViewModel vm = new ViewModels.Matters.MatterContactViewModel();

            vm.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(OpenLawOffice.Data.Matters.Matter.Get(matterId));
            vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(OpenLawOffice.Data.Contacts.Contact.Get(id));

            return View(vm);
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult AssignContact(ViewModels.Matters.MatterContactViewModel model)
        {
            // We need to reset the Id of the model as it is picking up the id from the route, 
            // which is incorrect
            model.Id = null;

            Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);

            Common.Models.Matters.MatterContact matterContact = 
                OpenLawOffice.Data.Matters.MatterContact.Get(model.Matter.Id.Value, model.Contact.Id.Value);

            if (matterContact == null)
            { // Create
                matterContact = Mapper.Map<Common.Models.Matters.MatterContact>(model);
                matterContact = OpenLawOffice.Data.Matters.MatterContact.Create(matterContact, currentUser);
            }
            else
            { // Enable
                matterContact = Mapper.Map<Common.Models.Matters.MatterContact>(model);
                matterContact = OpenLawOffice.Data.Matters.MatterContact.Enable(matterContact, currentUser);
            }

            return RedirectToAction("Contacts", "Matters", 
                new { id = matterContact.Matter.Id.Value.ToString() });
        }

        //
        // GET: /MatterContact/Edit/5
        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Matters.MatterContactViewModel viewModel = null;
            
            Common.Models.Matters.MatterContact model = OpenLawOffice.Data.Matters.MatterContact.Get(id);
            model.Matter = OpenLawOffice.Data.Matters.Matter.Get(model.Matter.Id.Value);
            model.Contact = OpenLawOffice.Data.Contacts.Contact.Get(model.Contact.Id.Value);

            viewModel = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(model);
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter);
            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Contact);

            return View(viewModel);
        }

        //
        // POST: /MatterContact/Edit/5
        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Matters.MatterContactViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Matters.MatterContact model = Mapper.Map<Common.Models.Matters.MatterContact>(viewModel);
                model = OpenLawOffice.Data.Matters.MatterContact.Edit(model, currentUser);

                return RedirectToAction("Contacts", "Matters", 
                    new { id = model.Matter.Id.Value.ToString() });
            }
            catch
            {
                return View(viewModel);
            }
        }

        //
        // GET: /MatterContact/Details/5
        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Matters.MatterContactViewModel viewModel = null;

            Common.Models.Matters.MatterContact model = OpenLawOffice.Data.Matters.MatterContact.Get(id);
            model.Matter = OpenLawOffice.Data.Matters.Matter.Get(model.Matter.Id.Value);
            model.Contact = OpenLawOffice.Data.Contacts.Contact.Get(model.Contact.Id.Value);

            viewModel = Mapper.Map<ViewModels.Matters.MatterContactViewModel>(model);
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter);
            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Contact);
            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        //
        // GET: /MatterContact/Delete/5
        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(int id)
        {
            return Details(id);
        }

        //
        // POST: /MatterContact/Delete/5
        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(int id, ViewModels.Matters.MatterContactViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Matters.MatterContact model = Mapper.Map<Common.Models.Matters.MatterContact>(viewModel);
                model = OpenLawOffice.Data.Matters.MatterContact.Disable(model, currentUser);

                return RedirectToAction("Contacts", "Matters", 
                    new { id = model.Matter.Id.Value.ToString() });
            }
            catch
            {
                return View(viewModel);
            }
        }
    }
}
