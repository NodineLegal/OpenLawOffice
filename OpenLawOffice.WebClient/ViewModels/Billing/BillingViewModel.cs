// -----------------------------------------------------------------------
// <copyright file="BillingViewModel.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using OpenLawOffice.Common.Models;

    public class BillingViewModel
    {
        public class Item
        {
            public bool IsSelected { get; set; }
            public Contacts.ContactViewModel BillTo { get; set; }
            public Matters.MatterViewModel Matter { get; set; }
            public decimal Expenses { get; set; }
            public decimal Fees { get; set; }
            public TimeSpan Time { get; set; }
        }

        public class GroupItem
        {
            public bool IsSelected { get; set; }
            public Billing.BillingGroupViewModel BillingGroup { get; set; }
            public decimal Expenses { get; set; }
            public decimal Fees { get; set; }
            public TimeSpan Time { get; set; }
        }

        public List<Item> Items { get; set; }
        public List<GroupItem> GroupItems { get; set; }
        public List<InvoiceViewModel> RecentInvoices { get; set; }

        public BillingViewModel()
        {
            Items = new List<Item>();
            GroupItems = new List<GroupItem>();
            RecentInvoices = new List<InvoiceViewModel>();
        }
    }
}