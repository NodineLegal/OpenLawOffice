// -----------------------------------------------------------------------
// <copyright file="InvoiceViewModel.cs" company="Nodine Legal, LLC">
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

    [MapMe]
    public class InvoiceViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }
        public Contacts.ContactViewModel BillTo { get; set; }
        public DateTime Date { get; set; }
        public DateTime Due { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public string ExternalInvoiceId { get; set; }
        public string BillTo_NameLine1 { get; set; }
        public string BillTo_NameLine2 { get; set; }
        public string BillTo_AddressLine1 { get; set; }
        public string BillTo_AddressLine2 { get; set; }
        public string BillTo_City { get; set; }
        public string BillTo_State { get; set; }
        public string BillTo_Zip { get; set; }
        public Matters.MatterViewModel Matter { get; set; }
        public BillingGroupViewModel BillingGroup { get; set; }

        public List<InvoiceTimeViewModel> Times { get; set; }
        public List<InvoiceExpenseViewModel> Expenses { get; set; }
        public List<InvoiceFeeViewModel> Fees { get; set; }

        public InvoiceViewModel()
        {
            Times = new List<InvoiceTimeViewModel>();
            Expenses = new List<InvoiceExpenseViewModel>();
            Fees = new List<InvoiceFeeViewModel>();
        }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Billing.Invoice, InvoiceViewModel>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
                .ForMember(dst => dst.Disabled, opt => opt.MapFrom(src => src.Disabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = db.CreatedBy.PId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = db.ModifiedBy.PId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (db.DisabledBy == null || !db.DisabledBy.PId.HasValue) return null;
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = db.DisabledBy.PId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.BillTo, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Contacts.ContactViewModel()
                    {
                        Id = db.BillTo.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dst => dst.Due, opt => opt.MapFrom(src => src.Due))
                .ForMember(dst => dst.Subtotal, opt => opt.MapFrom(src => src.Subtotal))
                .ForMember(dst => dst.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount))
                .ForMember(dst => dst.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dst => dst.ExternalInvoiceId, opt => opt.MapFrom(src => src.ExternalInvoiceId))
                .ForMember(dst => dst.BillTo_NameLine1, opt => opt.MapFrom(src => src.BillTo_NameLine1))
                .ForMember(dst => dst.BillTo_NameLine2, opt => opt.MapFrom(src => src.BillTo_NameLine2))
                .ForMember(dst => dst.BillTo_AddressLine1, opt => opt.MapFrom(src => src.BillTo_AddressLine1))
                .ForMember(dst => dst.BillTo_AddressLine2, opt => opt.MapFrom(src => src.BillTo_AddressLine2))
                .ForMember(dst => dst.BillTo_City, opt => opt.MapFrom(src => src.BillTo_City))
                .ForMember(dst => dst.BillTo_State, opt => opt.MapFrom(src => src.BillTo_State))
                .ForMember(dst => dst.BillTo_Zip, opt => opt.MapFrom(src => src.BillTo_Zip))
                .ForMember(dst => dst.Matter, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Matters.MatterViewModel()
                    {
                        Id = db.Matter.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.BillingGroup, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Billing.BillingGroupViewModel()
                    {
                        Id = db.BillingGroup.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Times, opt => opt.Ignore())
                .ForMember(dst => dst.Expenses, opt => opt.Ignore())
                .ForMember(dst => dst.Fees, opt => opt.Ignore());

            Mapper.CreateMap<InvoiceViewModel, Common.Models.Billing.Invoice>()
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
                .ForMember(dst => dst.Disabled, opt => opt.MapFrom(src => src.Disabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(x =>
                {
                    if (x.CreatedBy == null || !x.CreatedBy.PId.HasValue)
                        return null;
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = x.CreatedBy.PId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(x =>
                {
                    if (x.CreatedBy == null || !x.CreatedBy.PId.HasValue)
                        return null;
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = x.ModifiedBy.PId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(x =>
                {
                    if (x.DisabledBy == null || !x.DisabledBy.PId.HasValue)
                        return null;
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = x.DisabledBy.PId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.BillTo, opt => opt.ResolveUsing(x =>
                {
                    if (x.BillTo == null || !x.BillTo.Id.HasValue)
                        return null;
                    return new ViewModels.Contacts.ContactViewModel()
                    {
                        Id = x.BillTo.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dst => dst.Due, opt => opt.MapFrom(src => src.Due))
                .ForMember(dst => dst.Subtotal, opt => opt.MapFrom(src => src.Subtotal))
                .ForMember(dst => dst.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount))
                .ForMember(dst => dst.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dst => dst.ExternalInvoiceId, opt => opt.MapFrom(src => src.ExternalInvoiceId))
                .ForMember(dst => dst.BillTo_NameLine1, opt => opt.MapFrom(src => src.BillTo_NameLine1))
                .ForMember(dst => dst.BillTo_NameLine2, opt => opt.MapFrom(src => src.BillTo_NameLine2))
                .ForMember(dst => dst.BillTo_AddressLine1, opt => opt.MapFrom(src => src.BillTo_AddressLine1))
                .ForMember(dst => dst.BillTo_AddressLine2, opt => opt.MapFrom(src => src.BillTo_AddressLine2))
                .ForMember(dst => dst.BillTo_City, opt => opt.MapFrom(src => src.BillTo_City))
                .ForMember(dst => dst.BillTo_State, opt => opt.MapFrom(src => src.BillTo_State))
                .ForMember(dst => dst.BillTo_Zip, opt => opt.MapFrom(src => src.BillTo_Zip))
                .ForMember(dst => dst.Matter, opt => opt.ResolveUsing(x =>
                {
                    if (x.Matter == null || !x.Matter.Id.HasValue)
                        return null;
                    return new ViewModels.Matters.MatterViewModel()
                    {
                        Id = x.Matter.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.BillingGroup, opt => opt.ResolveUsing(x =>
                {
                    if (x.BillingGroup == null || !x.BillingGroup.Id.HasValue)
                        return null;
                    return new ViewModels.Billing.BillingGroupViewModel()
                    {
                        Id = x.BillingGroup.Id,
                        IsStub = true
                    };
                }));
        }
    }
}