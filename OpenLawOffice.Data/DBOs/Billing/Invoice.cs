// -----------------------------------------------------------------------
// <copyright file="Invoice.cs" company="Nodine Legal, LLC">
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
    public class Invoice : Core
    {
        [ColumnMapping(Name = "id")]
        public Guid Id { get; set; }

        [ColumnMapping(Name = "bill_to_contact_id")]
        public int BillToContactId { get; set; }

        [ColumnMapping(Name = "bill_to_name_line_1")]
        public string BillTo_NameLine1 { get; set; }

        [ColumnMapping(Name = "bill_to_name_line_2")]
        public string BillTo_NameLine2 { get; set; }

        [ColumnMapping(Name = "bill_to_address_line_1")]
        public string BillTo_AddressLine1 { get; set; }

        [ColumnMapping(Name = "bill_to_address_line_2")]
        public string BillTo_AddressLine2 { get; set; }

        [ColumnMapping(Name = "bill_to_city")]
        public string BillTo_City { get; set; }

        [ColumnMapping(Name = "bill_to_state")]
        public string BillTo_State { get; set; }

        [ColumnMapping(Name = "bill_to_zip")]
        public string BillTo_Zip { get; set; }

        [ColumnMapping(Name = "date")]
        public DateTime Date { get; set; }

        [ColumnMapping(Name = "due")]
        public DateTime Due { get; set; }

        [ColumnMapping(Name = "subtotal")]
        public decimal Subtotal { get; set; }

        [ColumnMapping(Name = "tax_amount")]
        public decimal TaxAmount { get; set; }

        [ColumnMapping(Name = "total")]
        public decimal Total { get; set; }

        [ColumnMapping(Name = "external_invoice_id")]
        public string ExternalInvoiceId { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Invoice), new ColumnAttributeTypeMapper<Invoice>());
            Mapper.CreateMap<DBOs.Billing.Invoice, Common.Models.Billing.Invoice>()
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
                .ForMember(dst => dst.BillTo, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.BillToContactId,
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
                .ForMember(dst => dst.BillTo_Zip, opt => opt.MapFrom(src => src.BillTo_Zip));

            Mapper.CreateMap<Common.Models.Billing.Invoice, DBOs.Billing.Invoice>()
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
                .ForMember(dst => dst.BillToContactId, opt => opt.ResolveUsing(model =>
                {
                    return model.BillTo.Id.Value;
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
                .ForMember(dst => dst.BillTo_Zip, opt => opt.MapFrom(src => src.BillTo_Zip));
        }
    }
}
