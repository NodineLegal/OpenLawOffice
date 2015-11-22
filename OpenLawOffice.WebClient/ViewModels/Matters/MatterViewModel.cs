// -----------------------------------------------------------------------
// <copyright file="MatterViewModel.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.WebClient.ViewModels.Matters
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;
    using System.Collections.Generic;

    [MapMe]
    public class MatterViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }

        public MatterViewModel Parent { get; set; }

        public MatterTypeViewModel MatterType { get; set; }

        public string Title { get; set; }

        public string Synopsis { get; set; }

        public bool Active { get; set; }

        public string Jurisdiction { get; set; }

        public string CaseNumber { get; set; }

        public Contacts.ContactViewModel LeadAttorney { get; set; }

        public List<Contacts.ContactViewModel> Clients { get; set; }

        public Contacts.ContactViewModel BillTo { get; set; }

        public Billing.BillingRateViewModel DefaultBillingRate { get; set; }

        public Billing.BillingGroupViewModel BillingGroup { get; set; }

        public bool OverrideMatterRateWithEmployeeRate { get; set; }

        // -- Financial Information
            
            // -- DB Values
        public decimal? MinimumCharge { get; set; }
        public decimal? EstimatedCharge { get; set; }
        public decimal? MaximumCharge { get; set; }
            
            // -- Calculated
        public TimeSpan NonbillableTime { get; set; }
        public decimal Fees { get; set; }
        public decimal Expenses { get; set; }
        public TimeSpan TimeBilled { get; set; }
        public decimal MoneyBilled { get; set; }
        public TimeSpan TimeBillable { get; set; }
        public decimal MoneyBillable { get; set; }
        public decimal TotalValue { get; set; }
        public decimal EffectiveHourlyRate { get; set; }

        public List<ViewModels.Tasks.TaskViewModel> Tasks { get; set; }
        public List<ViewModels.Notes.NoteViewModel> Notes { get; set; }
        public Dictionary<ViewModels.Tasks.TaskViewModel, List<ViewModels.Notes.NoteViewModel>> TaskNotes { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Matters.Matter, MatterViewModel>()
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
                .ForMember(dst => dst.Parent, opt => opt.ResolveUsing(db =>
                {
                    if (!db.ParentId.HasValue) return null;
                    return new ViewModels.Matters.MatterViewModel()
                    {
                        Id = db.ParentId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.MatterType, opt => opt.ResolveUsing(db =>
                {
                    if (db.MatterType == null || !db.MatterType.Id.HasValue) return null;
                    return new ViewModels.Matters.MatterTypeViewModel()
                    {
                        Id = db.MatterType.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dst => dst.Jurisdiction, opt => opt.MapFrom(src => src.Jurisdiction))
                .ForMember(dst => dst.CaseNumber, opt => opt.MapFrom(src => src.CaseNumber))
                .ForMember(dst => dst.LeadAttorney, opt => opt.ResolveUsing(db =>
                {
                    if (db.LeadAttorney == null || !db.LeadAttorney.Id.HasValue) return null;
                    return new ViewModels.Contacts.ContactViewModel()
                    {
                        Id = db.LeadAttorney.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.BillTo, opt => opt.ResolveUsing(db =>
                {
                    if (db.BillTo == null || !db.BillTo.Id.HasValue) return null;
                    return new ViewModels.Contacts.ContactViewModel()
                    {
                        Id = db.BillTo.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DefaultBillingRate, opt => opt.ResolveUsing(db =>
                {
                    if (db.DefaultBillingRate == null || !db.DefaultBillingRate.Id.HasValue) return null;
                    return new ViewModels.Billing.BillingRateViewModel()
                    {
                        Id = db.DefaultBillingRate.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.BillingGroup, opt => opt.ResolveUsing(db =>
                {
                    if (db.BillingGroup == null || !db.BillingGroup.Id.HasValue) return null;
                    return new ViewModels.Billing.BillingGroupViewModel()
                    {
                        Id = db.BillingGroup.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.OverrideMatterRateWithEmployeeRate, opt => opt.MapFrom(src => src.OverrideMatterRateWithEmployeeRate))
                .ForMember(dst => dst.Clients, opt => opt.Ignore())
                .ForMember(dst => dst.Tasks, opt => opt.Ignore())
                .ForMember(dst => dst.Notes, opt => opt.Ignore())
                .ForMember(dst => dst.TaskNotes, opt => opt.Ignore())
                .ForMember(dst => dst.MinimumCharge, opt => opt.MapFrom(src => src.MinimumCharge))
                .ForMember(dst => dst.EstimatedCharge, opt => opt.MapFrom(src => src.EstimatedCharge))
                .ForMember(dst => dst.MaximumCharge, opt => opt.MapFrom(src => src.MaximumCharge))
                .ForMember(dst => dst.NonbillableTime, opt => opt.Ignore())
                .ForMember(dst => dst.Fees, opt => opt.Ignore())
                .ForMember(dst => dst.Expenses, opt => opt.Ignore())
                .ForMember(dst => dst.TimeBilled, opt => opt.Ignore())
                .ForMember(dst => dst.MoneyBilled, opt => opt.Ignore())
                .ForMember(dst => dst.TimeBillable, opt => opt.Ignore())
                .ForMember(dst => dst.MoneyBillable, opt => opt.Ignore())
                .ForMember(dst => dst.TotalValue, opt => opt.Ignore())
                .ForMember(dst => dst.EffectiveHourlyRate, opt => opt.Ignore());

            Mapper.CreateMap<MatterViewModel, Common.Models.Matters.Matter>()
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
                .ForMember(dst => dst.ParentId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Parent == null || !model.Parent.Id.HasValue)
                        return null;
                    return model.Parent.Id.Value;
                }))
                .ForMember(dst => dst.MatterType, opt => opt.ResolveUsing(x =>
                {
                    if (x.MatterType == null || !x.MatterType.Id.HasValue)
                        return null;
                    return new ViewModels.Matters.MatterTypeViewModel()
                    {
                        Id = x.MatterType.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dst => dst.Jurisdiction, opt => opt.MapFrom(src => src.Jurisdiction))
                .ForMember(dst => dst.CaseNumber, opt => opt.MapFrom(src => src.CaseNumber))
                .ForMember(dst => dst.LeadAttorney, opt => opt.ResolveUsing(x =>
                {
                    if (x.LeadAttorney == null || !x.LeadAttorney.Id.HasValue)
                        return null;
                    return new ViewModels.Contacts.ContactViewModel()
                    {
                        Id = x.LeadAttorney.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.BillTo, opt => opt.ResolveUsing(x =>
                {
                    if (x.BillTo == null || !x.BillTo.Id.HasValue)
                        return null;
                    return new ViewModels.Contacts.ContactViewModel()
                    {
                        Id = x.BillTo.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DefaultBillingRate, opt => opt.ResolveUsing(x =>
                {
                    if (x.DefaultBillingRate == null || !x.DefaultBillingRate.Id.HasValue)
                        return null;
                    return new ViewModels.Billing.BillingRateViewModel()
                    {
                        Id = x.DefaultBillingRate.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.BillingGroup, opt => opt.ResolveUsing(x =>
                {
                    if (x.BillingGroup == null || !x.BillingGroup.Id.HasValue)
                        return null;
                    return new ViewModels.Billing.BillingGroupViewModel()
                    {
                        Id = x.BillingGroup.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.OverrideMatterRateWithEmployeeRate, opt => opt.MapFrom(src => src.OverrideMatterRateWithEmployeeRate))
                .ForMember(dst => dst.MinimumCharge, opt => opt.MapFrom(src => src.MinimumCharge))
                .ForMember(dst => dst.EstimatedCharge, opt => opt.MapFrom(src => src.EstimatedCharge))
                .ForMember(dst => dst.MaximumCharge, opt => opt.MapFrom(src => src.MaximumCharge));
        }
    }
}