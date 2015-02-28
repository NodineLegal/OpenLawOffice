// -----------------------------------------------------------------------
// <copyright file="BillingGroupsController.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class BillingGroupsController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Index()
        {
            List<ViewModels.Billing.BillingGroupViewModel> groups = new List<ViewModels.Billing.BillingGroupViewModel>();

            Data.Billing.BillingGroup.List().ForEach(x =>
            {
                ViewModels.Billing.BillingGroupViewModel vm = Mapper.Map<ViewModels.Billing.BillingGroupViewModel>(x);
                vm.BillTo = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(x.BillTo.Id.Value));
                groups.Add(vm);
            });

            return View(groups);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Invoices(int id)
        {
            List<ViewModels.Billing.InvoiceViewModel> list = new List<ViewModels.Billing.InvoiceViewModel>();

            Data.Billing.BillingGroup.ListInvoicesForGroup(id).ForEach(x =>
            {
                list.Add(Mapper.Map<ViewModels.Billing.InvoiceViewModel>(x));
            });

            return View(list);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Details(int id)
        {
            ViewModels.Billing.BillingGroupViewModel vm;
            Common.Models.Billing.BillingGroup group;
            List<Common.Models.Matters.Matter> matterMembers;

            group = Data.Billing.BillingGroup.Get(id);
            group.BillTo = Data.Contacts.Contact.Get(group.BillTo.Id.Value);
            matterMembers = Data.Billing.BillingGroup.ListMattersForGroup(id);

            vm = Mapper.Map<ViewModels.Billing.BillingGroupViewModel>(group);
            vm.BillTo = Mapper.Map<ViewModels.Contacts.ContactViewModel>(group.BillTo);
            vm.MatterMembers = new List<ViewModels.Matters.MatterViewModel>();
            matterMembers.ForEach(x =>
            {
                vm.MatterMembers.Add(Mapper.Map<ViewModels.Matters.MatterViewModel>(x));
            });
            PopulateCoreDetails(vm);

            return View(vm);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Login, User")]
        [HttpPost]
        public ActionResult Create(ViewModels.Billing.BillingGroupViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Billing.BillingGroup model;

            model = Mapper.Map<Common.Models.Billing.BillingGroup>(viewModel);
            currentUser = Data.Account.Users.Get(User.Identity.Name);

            Data.Billing.BillingGroup.Create(model, currentUser);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(int id)
        {
            ViewModels.Billing.BillingGroupViewModel vm;

            vm = Mapper.Map<ViewModels.Billing.BillingGroupViewModel>(Data.Billing.BillingGroup.Get(id));
            vm.BillTo = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(vm.BillTo.Id.Value));

            return View(vm);
        }

        [Authorize(Roles = "Login, User")]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Billing.BillingGroupViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Billing.BillingGroup model;

            model = Mapper.Map<Common.Models.Billing.BillingGroup>(viewModel);
            currentUser = Data.Account.Users.Get(User.Identity.Name);

            Data.Billing.BillingGroup.Edit(model, currentUser);

            return RedirectToAction("Index");
        }
    }
}