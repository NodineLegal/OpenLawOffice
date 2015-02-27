// -----------------------------------------------------------------------
// <copyright file="GroupInvoiceViewModel.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.WebClient.ViewModels.Billing
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;
    using System.Collections.Generic;

    public class GroupInvoiceItemViewModel
    {
        public Matters.MatterViewModel Matter { get; set; }
        public decimal ExpensesSum { get; set; }
        public decimal FeesSum { get; set; }
        public TimeSpan TimeSum { get; set; }
        public decimal TimeSumMoney { get; set; }

        public List<InvoiceTimeViewModel> Times { get; set; }
        public List<InvoiceExpenseViewModel> Expenses { get; set; }
        public List<InvoiceFeeViewModel> Fees { get; set; }

        public GroupInvoiceItemViewModel()
        {
            Times = new List<InvoiceTimeViewModel>();
            Expenses = new List<InvoiceExpenseViewModel>();
            Fees = new List<InvoiceFeeViewModel>();
        }
    }
}