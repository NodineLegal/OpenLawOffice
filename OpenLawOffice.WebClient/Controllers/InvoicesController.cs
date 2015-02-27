// -----------------------------------------------------------------------
// <copyright file="InvoicesController.cs" company="Nodine Legal, LLC">
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
    public class InvoicesController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Details(Guid id)
        {
            Common.Models.Billing.Invoice invoice = Data.Billing.Invoice.Get(id);

            if (invoice.BillingGroup != null && invoice.BillingGroup.Id.HasValue)
                return RedirectToAction("GroupDetails", new { Id = id });

            return RedirectToAction("MatterDetails", new { Id = id });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult MatterDetails(Guid id)
        {
            Common.Models.Billing.Invoice invoice = null;
            ViewModels.Billing.InvoiceViewModel viewModel = new ViewModels.Billing.InvoiceViewModel();

            invoice = Data.Billing.Invoice.Get(id);
            viewModel = Mapper.Map<ViewModels.Billing.InvoiceViewModel>(invoice);

            if (invoice.Matter != null)
                viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(Data.Matters.Matter.Get(invoice.Matter.Id.Value));

            Data.Billing.Invoice.ListInvoiceExpensesForInvoice(invoice.Id.Value).ForEach(x =>
            {
                ViewModels.Billing.InvoiceExpenseViewModel vm = Mapper.Map<ViewModels.Billing.InvoiceExpenseViewModel>(x);
                vm.Expense = Mapper.Map<ViewModels.Billing.ExpenseViewModel>(Data.Billing.Expense.Get(vm.Expense.Id.Value));
                viewModel.Expenses.Add(vm);
            });

            Data.Billing.Invoice.ListInvoiceFeesForInvoice(invoice.Id.Value).ForEach(x =>
            {
                ViewModels.Billing.InvoiceFeeViewModel vm = Mapper.Map<ViewModels.Billing.InvoiceFeeViewModel>(x);
                vm.Fee = Mapper.Map<ViewModels.Billing.FeeViewModel>(Data.Billing.Fee.Get(vm.Fee.Id.Value));
                viewModel.Fees.Add(vm);
            });

            Data.Billing.Invoice.ListInvoiceTimesForInvoice(invoice.Id.Value).ForEach(x =>
            {
                ViewModels.Billing.InvoiceTimeViewModel vm = Mapper.Map<ViewModels.Billing.InvoiceTimeViewModel>(x);
                vm.Time = Mapper.Map<ViewModels.Timing.TimeViewModel>(Data.Timing.Time.Get(vm.Time.Id.Value));
                viewModel.Times.Add(vm);
            });

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
        public ActionResult MatterPrint(Guid id)
        {
            return MatterDetails(id);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult GroupDetails(Guid id)
        {
            Common.Models.Billing.BillingGroup billingGroup;
            ViewModels.Billing.GroupInvoiceViewModel viewModel = new ViewModels.Billing.GroupInvoiceViewModel();
            List<Common.Models.Matters.Matter> mattersList;
            Common.Models.Billing.Invoice invoice = null;

            invoice = Data.Billing.Invoice.Get(id);
            billingGroup = Data.Billing.BillingGroup.Get(invoice.BillingGroup.Id.Value);

            viewModel = new ViewModels.Billing.GroupInvoiceViewModel()
            {
                Id = invoice.Id,
                BillTo = new ViewModels.Contacts.ContactViewModel() { Id = invoice.BillTo.Id },
                Date = invoice.Date,
                Due = invoice.Due,
                Subtotal = invoice.Subtotal,
                TaxAmount = invoice.TaxAmount,
                Total = invoice.Total,
                ExternalInvoiceId = invoice.ExternalInvoiceId,
                BillTo_NameLine1 = invoice.BillTo_NameLine1,
                BillTo_NameLine2 = invoice.BillTo_NameLine2,
                BillTo_AddressLine1 = invoice.BillTo_AddressLine1,
                BillTo_AddressLine2 = invoice.BillTo_AddressLine2,
                BillTo_City = invoice.BillTo_City,
                BillTo_State = invoice.BillTo_State,
                BillTo_Zip = invoice.BillTo_Zip,
                BillingGroup = Mapper.Map<ViewModels.Billing.BillingGroupViewModel>(billingGroup)
            };

            mattersList = Data.Billing.BillingGroup.ListMattersForGroup(billingGroup.Id.Value);

            for (int i = 0; i < mattersList.Count; i++)
            {
                ViewModels.Billing.GroupInvoiceItemViewModel giivm = new ViewModels.Billing.GroupInvoiceItemViewModel();
                giivm.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(mattersList[i]);

                Data.Billing.InvoiceExpense.ListForMatter(mattersList[i].Id.Value).ForEach(x =>
                {
                    ViewModels.Billing.InvoiceExpenseViewModel vm = Mapper.Map<ViewModels.Billing.InvoiceExpenseViewModel>(x);
                    vm.Expense = Mapper.Map<ViewModels.Billing.ExpenseViewModel>(Data.Billing.Expense.Get(vm.Expense.Id.Value));
                    giivm.ExpensesSum += vm.Amount;
                    giivm.Expenses.Add(vm);
                });

                Data.Billing.InvoiceFee.ListForMatter(mattersList[i].Id.Value).ForEach(x =>
                {
                    ViewModels.Billing.InvoiceFeeViewModel vm = Mapper.Map<ViewModels.Billing.InvoiceFeeViewModel>(x);
                    vm.Fee = Mapper.Map<ViewModels.Billing.FeeViewModel>(Data.Billing.Fee.Get(vm.Fee.Id.Value));
                    giivm.FeesSum += vm.Amount;
                    giivm.Fees.Add(vm);
                });

                Data.Billing.InvoiceTime.ListForMatter(mattersList[i].Id.Value).ForEach(x =>
                {
                    ViewModels.Billing.InvoiceTimeViewModel vm = Mapper.Map<ViewModels.Billing.InvoiceTimeViewModel>(x);
                    vm.Time = Mapper.Map<ViewModels.Timing.TimeViewModel>(Data.Timing.Time.Get(vm.Time.Id.Value));
                    giivm.TimeSum = giivm.TimeSum.Add(vm.Duration);
                    giivm.TimeSumMoney += vm.PricePerHour * (decimal)vm.Duration.TotalHours;
                    giivm.Times.Add(vm);
                });

                viewModel.Matters.Add(giivm);
            }

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
        public ActionResult GroupPrint_Full(Guid id)
        {
            return GroupDetails(id);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult GroupPrint_Summary(Guid id)
        {
            return GroupDetails(id);
        }
    }
}
