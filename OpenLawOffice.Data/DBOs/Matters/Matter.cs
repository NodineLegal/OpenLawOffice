// -----------------------------------------------------------------------
// <copyright file="Matter.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Matters
{
    using System;
    using AutoMapper;

    [Common.Models.MapMe]
    public class Matter : Core
    {
        [ColumnMapping(Name = "id")]
        public Guid Id { get; set; }

        [ColumnMapping(Name = "title")]
        public string Title { get; set; }

        [ColumnMapping(Name = "parent_id")]
        public Guid? ParentId { get; set; }

        [ColumnMapping(Name = "synopsis")]
        public string Synopsis { get; set; }

        [ColumnMapping(Name = "active")]
        public bool Active { get; set; }

        [ColumnMapping(Name = "jurisdiction")]
        public string Jurisdiction { get; set; }

        [ColumnMapping(Name = "case_number")]
        public string CaseNumber { get; set; }

        [ColumnMapping(Name = "lead_attorney_contact_id")]
        public int? LeadAttorneyContactId { get; set; }

        [ColumnMapping(Name = "bill_to_contact_id")]
        public int? BillToContactId { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Matter), new ColumnAttributeTypeMapper<Matter>());
            Mapper.CreateMap<DBOs.Matters.Matter, Common.Models.Matters.Matter>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Created, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcCreated.ToSystemTime();
                }))
                .ForMember(dst => dst.Modified, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcModified.ToSystemTime();
                }))
                .ForMember(dst => dst.Disabled, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcDisabled.ToSystemTime();
                }))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Account.Users()
                    {
                        PId = db.CreatedByUserPId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Account.Users()
                    {
                        PId = db.ModifiedByUserPId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (!db.DisabledByUserPId.HasValue) return null;
                    return new Common.Models.Account.Users()
                    {
                        PId = db.DisabledByUserPId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dst => dst.Jurisdiction, opt => opt.MapFrom(src => src.Jurisdiction))
                .ForMember(dst => dst.CaseNumber, opt => opt.MapFrom(src => src.CaseNumber))
                .ForMember(dst => dst.LeadAttorney, opt => opt.ResolveUsing(db =>
                {
                    if (!db.LeadAttorneyContactId.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.LeadAttorneyContactId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.BillTo, opt => opt.ResolveUsing(db =>
                {
                    if (!db.BillToContactId.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.BillToContactId.Value,
                        IsStub = true
                    };
                }));

            Mapper.CreateMap<Common.Models.Matters.Matter, DBOs.Matters.Matter>()
                .ForMember(dst => dst.UtcCreated, opt => opt.ResolveUsing(db =>
                {
                    return db.Created.ToDbTime();
                }))
                .ForMember(dst => dst.UtcModified, opt => opt.ResolveUsing(db =>
                {
                    return db.Modified.ToDbTime();
                }))
                .ForMember(dst => dst.UtcDisabled, opt => opt.ResolveUsing(db =>
                {
                    return db.Disabled.ToDbTime();
                }))
                .ForMember(dst => dst.CreatedByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.CreatedBy == null || !model.CreatedBy.PId.HasValue)
                        return Guid.Empty;
                    return model.CreatedBy.PId.Value;
                }))
                .ForMember(dst => dst.ModifiedByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.ModifiedBy == null || !model.ModifiedBy.PId.HasValue)
                        return Guid.Empty;
                    return model.ModifiedBy.PId.Value;
                }))
                .ForMember(dst => dst.DisabledByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.DisabledBy == null) return null;
                    return model.DisabledBy.PId;
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dst => dst.Jurisdiction, opt => opt.MapFrom(src => src.Jurisdiction))
                .ForMember(dst => dst.CaseNumber, opt => opt.MapFrom(src => src.CaseNumber))
                .ForMember(dst => dst.LeadAttorneyContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.LeadAttorney == null) return null;
                    return model.LeadAttorney.Id;
                }))
                .ForMember(dst => dst.BillToContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.BillTo == null) return null;
                    return model.BillTo.Id;
                }));
        }
    }
}