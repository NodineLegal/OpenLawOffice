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
    public class BillingController : BaseController
    {
        public class RateListItem
        {
            public string Title { get; set; }
        }

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

                if ((item.Expenses + item.Fees + (decimal)item.Time.TotalHours) > 0)
                    viewModel.Items.Add(item);
            });

            Data.Billing.Invoice.ListBillableBillingGroups().ForEach(group =>
            {
                groupItem = new ViewModels.Billing.BillingViewModel.GroupItem();
                groupItem.BillingGroup = Mapper.Map<ViewModels.Billing.BillingGroupViewModel>(group);
                groupItem.BillingGroup.BillTo = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(group.BillTo.Id.Value));
                groupItem.Expenses = Data.Billing.BillingGroup.SumBillableExpensesForGroup(group.Id.Value);
                groupItem.Fees = Data.Billing.BillingGroup.SumBillableFeesForGroup(group.Id.Value);
                groupItem.Time = Data.Timing.Time.SumUnbilledAndBillableTimeForBillingGroup(group.Id.Value);
                viewModel.GroupItems.Add(groupItem);
            });

            Data.Billing.Invoice.GetMostRecentInvoices(10).ForEach(invoice =>
            {
                ViewModels.Billing.InvoiceViewModel vm = Mapper.Map<ViewModels.Billing.InvoiceViewModel>(invoice);

                if (invoice.Matter != null && invoice.Matter.Id.HasValue)
                    vm.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(Data.Matters.Matter.Get(invoice.Matter.Id.Value));
                else
                    vm.Matter = null;

                if (invoice.BillingGroup != null && invoice.BillingGroup.Id.HasValue)
                    vm.BillingGroup = Mapper.Map<ViewModels.Billing.BillingGroupViewModel>(Data.Billing.BillingGroup.Get(invoice.BillingGroup.Id.Value));
                else
                    vm.BillingGroup = null;

                vm.BillTo = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(invoice.BillTo.Id.Value));

                viewModel.RecentInvoices.Add(vm);
            });
            
            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult SingleMatterBill(Guid id)
        {
            DateTime? start = null, stop = null;
            Common.Models.Billing.BillingRate billingRate = null;
            ViewModels.Billing.InvoiceViewModel viewModel = new ViewModels.Billing.InvoiceViewModel();
            Common.Models.Billing.Invoice previousInvoice = null;
            Common.Models.Matters.Matter matter;

            matter = Data.Matters.Matter.Get(id);

            previousInvoice = Data.Billing.Invoice.GetMostRecentInvoiceForContact(matter.BillTo.Id.Value);

            // Set default billing rate
            if (matter.DefaultBillingRate != null && matter.DefaultBillingRate.Id.HasValue)
                billingRate = Data.Billing.BillingRate.Get(matter.DefaultBillingRate.Id.Value);

            viewModel.Id = Guid.NewGuid();
            viewModel.BillTo = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(matter.BillTo.Id.Value));
            viewModel.Date = DateTime.Now;
            viewModel.Due = DateTime.Now.AddDays(30);
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter);

            if (previousInvoice == null)
            {
                viewModel.BillTo_NameLine1 = viewModel.BillTo.DisplayName;
                if (string.IsNullOrEmpty(viewModel.BillTo.Address1AddressPostOfficeBox))
                    viewModel.BillTo_AddressLine1 = viewModel.BillTo.Address1AddressStreet;
                else
                    viewModel.BillTo_AddressLine1 = "P.O. Box " + viewModel.BillTo.Address1AddressPostOfficeBox;
                viewModel.BillTo_City = viewModel.BillTo.Address1AddressCity;
                viewModel.BillTo_State = viewModel.BillTo.Address1AddressStateOrProvince;
                viewModel.BillTo_Zip = viewModel.BillTo.Address1AddressPostalCode;
            }
            else
            {
                viewModel.BillTo_NameLine1 = previousInvoice.BillTo_NameLine1;
                viewModel.BillTo_NameLine2 = previousInvoice.BillTo_NameLine2;
                viewModel.BillTo_AddressLine1 = previousInvoice.BillTo_AddressLine1;
                viewModel.BillTo_AddressLine2 = previousInvoice.BillTo_AddressLine2;
                viewModel.BillTo_City = previousInvoice.BillTo_City;
                viewModel.BillTo_State = previousInvoice.BillTo_State;
                viewModel.BillTo_Zip = previousInvoice.BillTo_Zip;
            }

            if (!string.IsNullOrEmpty(Request["StartDate"]))
                start = DateTime.Parse(Request["StartDate"]);
            if (!string.IsNullOrEmpty(Request["StopDate"]))
                stop = DateTime.Parse(Request["StopDate"]);

            Data.Timing.Time.ListUnbilledAndBillableTimeForMatter(matter.Id.Value, start, stop).ForEach(x =>
            {
                ViewModels.Billing.InvoiceTimeViewModel vm = new ViewModels.Billing.InvoiceTimeViewModel()
                {
                    Invoice = viewModel,
                    Time = Mapper.Map<ViewModels.Timing.TimeViewModel>(x),
                    Details = x.Details
                };
                if (x.Stop.HasValue)
                    vm.Duration = x.Stop.Value - x.Start;
                else
                    vm.Duration = new TimeSpan(0);

                if (string.IsNullOrEmpty(Request["rateFrom"]))
                { // Not specified in URL
                    if (matter.OverrideMatterRateWithEmployeeRate)
                    {
                        Common.Models.Contacts.Contact contact = Data.Contacts.Contact.Get(x.Worker.Id.Value);
                        if (contact.BillingRate != null && contact.BillingRate.Id.HasValue)
                            billingRate = Data.Billing.BillingRate.Get(contact.BillingRate.Id.Value);
                    }
                }
                else
                { // Overridden by current user in URL
                    if (Request["rateFrom"] == "employee")
                    {
                        Common.Models.Contacts.Contact contact = Data.Contacts.Contact.Get(x.Worker.Id.Value);
                        if (contact.BillingRate != null && contact.BillingRate.Id.HasValue)
                            billingRate = Data.Billing.BillingRate.Get(contact.BillingRate.Id.Value);
                    }
                }

                if (billingRate != null)
                    vm.PricePerHour = billingRate.PricePerUnit;
                viewModel.Times.Add(vm);
            });

            Data.Billing.Expense.ListUnbilledExpensesForMatter(matter.Id.Value, start, stop).ForEach(x =>
            {
                viewModel.Expenses.Add(new ViewModels.Billing.InvoiceExpenseViewModel()
                {
                    Invoice = viewModel,
                    Expense = Mapper.Map<ViewModels.Billing.ExpenseViewModel>(x),
                    Details = x.Details,
                    Amount = x.Amount
                });
            });

            Data.Billing.Fee.ListUnbilledFeesForMatter(matter.Id.Value, start, stop).ForEach(x =>
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
        [HttpPost]
        public ActionResult SingleMatterBill(Guid id, ViewModels.Billing.InvoiceViewModel viewModel)
        {
            // Create Invoice
            // Loop Expenses
            // Loop Fees
            // Loop Times
            // Redirect to invoice viewing
            decimal subtotal = 0;
            Common.Models.Account.Users currentUser;
            
            currentUser = Data.Account.Users.Get(User.Identity.Name);
            Common.Models.Billing.Invoice invoice = Mapper.Map<Common.Models.Billing.Invoice>(viewModel);

            // Any validation would need done here

            invoice = Data.Billing.Invoice.Create(invoice, currentUser);

            viewModel.Expenses.ForEach(vm =>
            {
                Common.Models.Billing.InvoiceExpense invexp = new Common.Models.Billing.InvoiceExpense()
                {
                    Invoice = invoice,
                    Expense = new Common.Models.Billing.Expense()
                    {
                        Id = vm.Expense.Id
                    },
                    Amount = vm.Amount,
                    Details = vm.Details
                };

                subtotal += vm.Amount;
                Data.Billing.InvoiceExpense.Create(invexp, currentUser);
            });

            viewModel.Fees.ForEach(vm =>
            {
                Common.Models.Billing.InvoiceFee invfee = new Common.Models.Billing.InvoiceFee()
                {
                    Invoice = invoice,
                    Fee = new Common.Models.Billing.Fee()
                    {
                        Id = vm.Fee.Id
                    },
                    Amount = vm.Amount,
                    Details = vm.Details
                };

                subtotal += vm.Amount;
                Data.Billing.InvoiceFee.Create(invfee, currentUser);
            });

            viewModel.Times.ForEach(vm =>
            {
                Common.Models.Billing.InvoiceTime invtime = new Common.Models.Billing.InvoiceTime()
                {
                    Invoice = invoice,
                    Time = new Common.Models.Timing.Time()
                    {
                        Id = vm.Time.Id
                    },
                    Duration = vm.Duration,
                    PricePerHour = vm.PricePerHour,
                    Details = vm.Details
                };

                subtotal += ((decimal)invtime.Duration.TotalHours * invtime.PricePerHour);
                Data.Billing.InvoiceTime.Create(invtime, currentUser);
            });

            invoice.Subtotal = subtotal;
            invoice.Total = invoice.Subtotal + invoice.TaxAmount;

            Data.Billing.Invoice.Edit(invoice, currentUser);

            return RedirectToAction("Details", "Invoices", new { id = invoice.Id });
        }
        
        [Authorize(Roles = "Login, User")]
        public ActionResult SingleGroupBill(int id)
        {
            DateTime? start = null, stop = null;
            Common.Models.Billing.BillingRate billingRate = null;
            Common.Models.Billing.BillingGroup billingGroup;
            Common.Models.Billing.Invoice previousInvoice = null;
            ViewModels.Billing.GroupInvoiceViewModel viewModel = new ViewModels.Billing.GroupInvoiceViewModel();
            List<Common.Models.Matters.Matter> mattersList;

            billingGroup = Data.Billing.BillingGroup.Get(id);
            previousInvoice = Data.Billing.Invoice.GetMostRecentInvoiceForContact(billingGroup.BillTo.Id.Value);

            billingGroup.NextRun = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);

            viewModel.Id = Guid.NewGuid();
            viewModel.BillTo = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(billingGroup.BillTo.Id.Value));
            viewModel.Date = DateTime.Now;
            viewModel.Due = DateTime.Now.AddDays(30);
            viewModel.BillingGroup = Mapper.Map<ViewModels.Billing.BillingGroupViewModel>(billingGroup);
            
            if (previousInvoice == null)
            {
                viewModel.BillTo_NameLine1 = viewModel.BillTo.DisplayName;
                if (string.IsNullOrEmpty(viewModel.BillTo.Address1AddressPostOfficeBox))
                    viewModel.BillTo_AddressLine1 = viewModel.BillTo.Address1AddressStreet;
                else
                    viewModel.BillTo_AddressLine1 = "P.O. Box " + viewModel.BillTo.Address1AddressPostOfficeBox;
                viewModel.BillTo_City = viewModel.BillTo.Address1AddressCity;
                viewModel.BillTo_State = viewModel.BillTo.Address1AddressStateOrProvince;
                viewModel.BillTo_Zip = viewModel.BillTo.Address1AddressPostalCode;
            }
            else
            {
                viewModel.BillTo_NameLine1 = previousInvoice.BillTo_NameLine1;
                viewModel.BillTo_NameLine2 = previousInvoice.BillTo_NameLine2;
                viewModel.BillTo_AddressLine1 = previousInvoice.BillTo_AddressLine1;
                viewModel.BillTo_AddressLine2 = previousInvoice.BillTo_AddressLine2;
                viewModel.BillTo_City = previousInvoice.BillTo_City;
                viewModel.BillTo_State = previousInvoice.BillTo_State;
                viewModel.BillTo_Zip = previousInvoice.BillTo_Zip;
            }

            mattersList = Data.Billing.BillingGroup.ListMattersForGroup(billingGroup.Id.Value);

            if (!string.IsNullOrEmpty(Request["StartDate"]))
                start = DateTime.Parse(Request["StartDate"]);
            if (!string.IsNullOrEmpty(Request["StopDate"]))
                stop = DateTime.Parse(Request["StopDate"]);
            
            for (int i=0; i<mattersList.Count; i++) 
            {
                ViewModels.Billing.GroupInvoiceItemViewModel giivm = new ViewModels.Billing.GroupInvoiceItemViewModel();
                Common.Models.Matters.Matter matter = mattersList[i];

                giivm.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter);

                Data.Timing.Time.ListUnbilledAndBillableTimeForMatter(matter.Id.Value, start, stop).ForEach(x =>
                {
                    ViewModels.Billing.InvoiceTimeViewModel vm = new ViewModels.Billing.InvoiceTimeViewModel()
                    {
                        //Invoice = viewModel,
                        Time = Mapper.Map<ViewModels.Timing.TimeViewModel>(x),
                        Details = x.Details
                    };
                    if (x.Stop.HasValue)
                        vm.Duration = x.Stop.Value - x.Start;
                    else
                        vm.Duration = new TimeSpan(0);

                    if (string.IsNullOrEmpty(Request["RateFrom"]))
                    { // Not specified in URL
                        if (matter.OverrideMatterRateWithEmployeeRate)
                        {
                            Common.Models.Contacts.Contact contact = Data.Contacts.Contact.Get(x.Worker.Id.Value);
                            if (contact.BillingRate != null && contact.BillingRate.Id.HasValue)
                                billingRate = Data.Billing.BillingRate.Get(contact.BillingRate.Id.Value);
                        }
                    }
                    else
                    { // Overridden by current user in URL
                        if (Request["RateFrom"] == "Employee")
                        {
                            Common.Models.Contacts.Contact contact = Data.Contacts.Contact.Get(x.Worker.Id.Value);
                            if (contact.BillingRate != null && contact.BillingRate.Id.HasValue)
                                billingRate = Data.Billing.BillingRate.Get(contact.BillingRate.Id.Value);
                        }
                    }

                    if (billingRate != null)
                        vm.PricePerHour = billingRate.PricePerUnit;

                    giivm.Times.Add(vm);
                });

                Data.Billing.Expense.ListUnbilledExpensesForMatter(matter.Id.Value, start, stop).ForEach(x =>
                {
                    giivm.Expenses.Add(new ViewModels.Billing.InvoiceExpenseViewModel()
                    {
                        //Invoice = viewModel,
                        Expense = Mapper.Map<ViewModels.Billing.ExpenseViewModel>(x),
                        Details = x.Details,
                        Amount = x.Amount
                    });
                });

                Data.Billing.Fee.ListUnbilledFeesForMatter(matter.Id.Value, start, stop).ForEach(x =>
                {
                    giivm.Fees.Add(new ViewModels.Billing.InvoiceFeeViewModel()
                    {
                        //Invoice = viewModel,
                        Fee = Mapper.Map<ViewModels.Billing.FeeViewModel>(x),
                        Details = x.Details,
                        Amount = x.Amount
                    });
                });

                if ((giivm.Times.Count > 0) ||
                    (giivm.Expenses.Count > 0) ||
                    (giivm.Fees.Count > 0))
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
        [HttpPost]
        public ActionResult SingleGroupBill(int id, ViewModels.Billing.GroupInvoiceViewModel viewModel)
        {
            // Create Invoice
            // Loop Matters
                // Loop Expenses
                // Loop Fees
                // Loop Times
            // Redirect to invoice viewing
            decimal subtotal = 0;
            Common.Models.Account.Users currentUser;
            Common.Models.Billing.BillingGroup billingGroup;

            currentUser = Data.Account.Users.Get(User.Identity.Name);
            billingGroup = Data.Billing.BillingGroup.Get(id);

            // Any validation would need done here

            Common.Models.Billing.Invoice invoice = new Common.Models.Billing.Invoice()
            {
                Id = viewModel.Id,
                BillTo = Mapper.Map<Common.Models.Contacts.Contact>(viewModel.BillTo),
                Date = viewModel.Date,
                Due = viewModel.Due,
                Subtotal = viewModel.Subtotal,
                TaxAmount = viewModel.TaxAmount,
                Total = viewModel.Total,
                ExternalInvoiceId = viewModel.ExternalInvoiceId,
                BillTo_NameLine1 = viewModel.BillTo_NameLine1,
                BillTo_NameLine2 = viewModel.BillTo_NameLine2,
                BillTo_AddressLine1 = viewModel.BillTo_AddressLine1,
                BillTo_AddressLine2 = viewModel.BillTo_AddressLine2,
                BillTo_City = viewModel.BillTo_City,
                BillTo_State = viewModel.BillTo_State,
                BillTo_Zip = viewModel.BillTo_Zip,
                BillingGroup = new Common.Models.Billing.BillingGroup() { Id = id }
            };

            invoice = Data.Billing.Invoice.Create(invoice, currentUser);

            viewModel.Matters.ForEach(matterVM =>
            {
                matterVM.Expenses.ForEach(vm =>
                {
                    Common.Models.Billing.InvoiceExpense invexp = new Common.Models.Billing.InvoiceExpense()
                    {
                        Invoice = invoice,
                        Expense = new Common.Models.Billing.Expense()
                        {
                            Id = vm.Expense.Id
                        },
                        Amount = vm.Amount,
                        Details = vm.Details
                    };

                    subtotal += vm.Amount;
                    Data.Billing.InvoiceExpense.Create(invexp, currentUser);
                });

                matterVM.Fees.ForEach(vm =>
                {
                    Common.Models.Billing.InvoiceFee invfee = new Common.Models.Billing.InvoiceFee()
                    {
                        Invoice = invoice,
                        Fee = new Common.Models.Billing.Fee()
                        {
                            Id = vm.Fee.Id
                        },
                        Amount = vm.Amount,
                        Details = vm.Details
                    };

                    subtotal += vm.Amount;
                    Data.Billing.InvoiceFee.Create(invfee, currentUser);
                });

                matterVM.Times.ForEach(vm =>
                {
                    Common.Models.Billing.InvoiceTime invtime = new Common.Models.Billing.InvoiceTime()
                    {
                        Invoice = invoice,
                        Time = new Common.Models.Timing.Time()
                        {
                            Id = vm.Time.Id
                        },
                        Duration = vm.Duration,
                        PricePerHour = vm.PricePerHour,
                        Details = vm.Details
                    };

                    subtotal += ((decimal)invtime.Duration.TotalHours * invtime.PricePerHour);
                    Data.Billing.InvoiceTime.Create(invtime, currentUser);
                });
            });

            invoice.Subtotal = subtotal;
            invoice.Total = viewModel.BillingGroup.Amount + invoice.Subtotal + invoice.TaxAmount;

            Data.Billing.Invoice.Edit(invoice, currentUser);

            billingGroup.LastRun = DateTime.Now;
            billingGroup.NextRun = viewModel.BillingGroup.NextRun;
            Data.Billing.BillingGroup.Edit(billingGroup, currentUser);

            return RedirectToAction("GroupDetails", "Invoices", new { id = invoice.Id });
        }
    }
}
