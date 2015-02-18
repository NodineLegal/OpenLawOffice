// -----------------------------------------------------------------------
// <copyright file="BillingController.cs" company="Nodine Legal, LLC">
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
    using System.Web.Profile;
    using System.Web.Security;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class BillingController : Controller
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Index()
        {
            ViewModels.Billing.BillingViewModel viewModel = new ViewModels.Billing.BillingViewModel();
            ViewModels.Billing.BillingViewModel.Item item;
            ViewModels.Billing.BillingViewModel.GroupItem groupItem;

            Data.Billing.Invoice.ListBillableMatters().ForEach(matter =>
            {
                item = new ViewModels.Billing.BillingViewModel.Item();
                item.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter);
                item.BillTo = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(matter.BillTo.Id.Value));
                item.Expenses = Data.Billing.Expense.SumUnbilledExpensesForMatter(matter.Id.Value);
                item.Fees = Data.Billing.Fee.SumUnbilledFeesForMatter(matter.Id.Value);
                item.Time = Data.Timing.Time.SumUnbilledAndBillableTimeForMatter(matter.Id.Value);
                viewModel.Items.Add(item);
            });

            Data.Billing.Invoice.ListBillableBillingGroups().ForEach(group =>
            {
                groupItem = new ViewModels.Billing.BillingViewModel.GroupItem();
                groupItem.BillingGroup = Mapper.Map<ViewModels.Billing.BillingGroupViewModel>(group);
                groupItem.BillingGroup.BillTo = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(group.BillTo.Id.Value));
                groupItem.Expenses = Data.Billing.BillingGroup.SumExpensesForGroup(group.Id.Value);
                groupItem.Fees = Data.Billing.BillingGroup.SumFeesForGroup(group.Id.Value);
                groupItem.Time = Data.Timing.Time.SumUnbilledAndBillableTimeForBillingGroup(group.Id.Value);
                viewModel.GroupItems.Add(groupItem);
            });
            
            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult SingleMatterBill(Guid id)
        {
            Common.Models.Billing.BillingRate defaultBillingRate = null;
            ViewModels.Billing.InvoiceViewModel viewModel = new ViewModels.Billing.InvoiceViewModel();
            Common.Models.Matters.Matter matter;

            matter = Data.Matters.Matter.Get(id);
            if (matter.DefaultBillingRate != null && matter.DefaultBillingRate.Id.HasValue)
                defaultBillingRate = Data.Billing.BillingRate.Get(matter.DefaultBillingRate.Id.Value);

            viewModel.Id = Guid.NewGuid();
            viewModel.BillTo = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(matter.BillTo.Id.Value));
            viewModel.Date = DateTime.Now;
            viewModel.Due = DateTime.Now.AddDays(30);
            viewModel.BillTo_NameLine1 = viewModel.BillTo.DisplayName;
            if (string.IsNullOrEmpty(viewModel.BillTo.Address1AddressPostOfficeBox))
                viewModel.BillTo_AddressLine1 = viewModel.BillTo.Address1AddressStreet;
            else
                viewModel.BillTo_AddressLine1 = "P.O. Box " + viewModel.BillTo.Address1AddressPostOfficeBox;
            viewModel.BillTo_City = viewModel.BillTo.Address1AddressCity;
            viewModel.BillTo_State = viewModel.BillTo.Address1AddressStateOrProvince;
            viewModel.BillTo_Zip = viewModel.BillTo.Address1AddressPostalCode;

            Data.Timing.Time.ListUnbilledAndBillableTimeForMatter(matter.Id.Value).ForEach(x =>
            {
                ViewModels.Billing.InvoiceTimeViewModel vm = new ViewModels.Billing.InvoiceTimeViewModel()
                {
                    Invoice = viewModel,
                    Time = Mapper.Map<ViewModels.Timing.TimeViewModel>(x),
                    Details = x.Details
                };
                if (defaultBillingRate != null)
                    vm.PricePerUnit = defaultBillingRate.PricePerUnit;
                viewModel.Times.Add(vm);
            });

            Data.Billing.Expense.ListUnbilledExpensesForMatter(matter.Id.Value).ForEach(x =>
            {
                viewModel.Expenses.Add(new ViewModels.Billing.InvoiceExpenseViewModel()
                {
                    Invoice = viewModel,
                    Expense = Mapper.Map<ViewModels.Billing.ExpenseViewModel>(x),
                    Details = x.Details,
                    Amount = x.Amount
                });
            });

            Data.Billing.Fee.ListUnbilledFeesForMatter(matter.Id.Value).ForEach(x =>
            {
                viewModel.Fees.Add(new ViewModels.Billing.InvoiceFeeViewModel()
                {
                    Invoice = viewModel,
                    Fee = Mapper.Map<ViewModels.Billing.FeeViewModel>(x),
                    Details = x.Details,
                    Amount = x.Amount
                });
            });

            ViewData["MatterTitle"] = matter.Title;
            ViewData["CaseNumber"] = matter.CaseNumber;
            ViewData["FirmName"] = Common.Settings.Manager.Instance.System.BillingFirmName;
            ViewData["FirmAddress"] = Common.Settings.Manager.Instance.System.BillingFirmAddress;
            ViewData["FirmCity"] = Common.Settings.Manager.Instance.System.BillingFirmCity;
            ViewData["FirmState"] = Common.Settings.Manager.Instance.System.BillingFirmState;
            ViewData["FirmZip"] = Common.Settings.Manager.Instance.System.BillingFirmZip;
            ViewData["FirmPhone"] = Common.Settings.Manager.Instance.System.BillingFirmPhone;
            ViewData["FirmWeb"] = Common.Settings.Manager.Instance.System.BillingFirmWeb;

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult SingleMatterBill(Guid id, ViewModels.Billing.InvoiceViewModel viewModel)
        {
            return View();
        }
    }
}
