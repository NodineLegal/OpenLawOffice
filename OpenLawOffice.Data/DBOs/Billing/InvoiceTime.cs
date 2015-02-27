// -----------------------------------------------------------------------
// <copyright file="InvoiceTime.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Billing
{
    using System;
    using AutoMapper;

    [Common.Models.MapMe]
    public class InvoiceTime : Core
    {
        [ColumnMapping(Name = "id")]
        public Guid Id { get; set; }

        [ColumnMapping(Name = "invoice_id")]
        public Guid InvoiceId { get; set; }

        [ColumnMapping(Name = "time_id")]
        public Guid TimeId { get; set; }

        [ColumnMapping(Name = "duration")]
        public TimeSpan Duration { get; set; }

        [ColumnMapping(Name = "price_per_hour")]
        public decimal PricePerHour { get; set; }

        [ColumnMapping(Name = "details")]
        public string Details { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(InvoiceTime), new ColumnAttributeTypeMapper<InvoiceTime>());
            Mapper.CreateMap<DBOs.Billing.InvoiceTime, Common.Models.Billing.InvoiceTime>()
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
                .ForMember(dst => dst.Invoice, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Billing.Invoice()
                    {
                        Id = db.InvoiceId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Time, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Timing.Time()
                    {
                        Id = db.TimeId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dst => dst.PricePerHour, opt => opt.MapFrom(src => src.PricePerHour))
                .ForMember(dst => dst.Details, opt => opt.MapFrom(src => src.Details));

            Mapper.CreateMap<Common.Models.Billing.InvoiceTime, DBOs.Billing.InvoiceTime>()
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
                .ForMember(dst => dst.InvoiceId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Invoice == null || !model.Invoice.Id.HasValue)
                        return Guid.Empty;
                    return model.Invoice.Id.Value;
                }))
                .ForMember(dst => dst.TimeId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Time == null || !model.Time.Id.HasValue)
                        return Guid.Empty;
                    return model.Time.Id.Value;
                }))
                .ForMember(dst => dst.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dst => dst.PricePerHour, opt => opt.MapFrom(src => src.PricePerHour))
                .ForMember(dst => dst.Details, opt => opt.MapFrom(src => src.Details));
        }
    }
}
