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

namespace OpenLawOffice.Data.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;
    
    public class Invoice
    {
        public static Common.Models.Billing.Invoice Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Billing.Invoice, DBOs.Billing.Invoice>(
                "SELECT * FROM \"invoice\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Matters.Matter> ListBillableMatters()
        {
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT \"matter\".*, \"contact\".\"display_name\" FROM \"matter\" LEFT JOIN \"contact\" ON \"matter\".\"bill_to_contact_id\"=\"contact\".\"id\" " +
                "WHERE \"matter\".\"id\" IN ( " +
                "   SELECT \"matter_id\" FROM \"task_matter\" WHERE " +
                "       \"task_id\" IN (SELECT \"task_id\" FROM \"task_time\" WHERE " +
                "           \"time_id\" IN (SELECT \"id\" FROM \"time\" WHERE \"billable\"=true AND \"utc_disabled\" is NULL) AND \"utc_disabled\" is NULL " +
                "       ) AND \"utc_disabled\" is NULL " +
                "	UNION " +
                "   SELECT \"matter_id\" FROM \"expense_matter\" WHERE \"expense_id\" NOT IN (SELECT \"expense_id\" FROM \"invoice_expense\" WHERE \"utc_disabled\" is NULL) AND \"utc_disabled\" is NULL " +
                "   UNION " +
                "   SELECT \"matter_id\" FROM \"fee_matter\" WHERE \"fee_id\" NOT IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"utc_disabled\" is NULL) AND \"utc_disabled\" is NULL " +
                ") AND \"matter\".\"billing_group_id\" IS NULL ORDER BY \"contact\".\"display_name\" ASC");
        }

        public static List<Common.Models.Billing.Invoice> ListInvoicesForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.Invoice, DBOs.Billing.Invoice>(
                "SELECT * FROM \"invoice\" WHERE \"matter_id\"=@MatterId ORDER BY \"date\" DESC",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Billing.InvoiceExpense> ListInvoiceExpensesForInvoice(Guid invoiceId)
        {
            return DataHelper.List<Common.Models.Billing.InvoiceExpense, DBOs.Billing.InvoiceExpense>(
                "SELECT * FROM \"invoice_expense\" WHERE \"invoice_id\"=@InvoiceId AND \"utc_disabled\" is NULL",
                new { InvoiceId = invoiceId });
        }

        public static List<Common.Models.Billing.InvoiceFee> ListInvoiceFeesForInvoice(Guid invoiceId)
        {
            return DataHelper.List<Common.Models.Billing.InvoiceFee, DBOs.Billing.InvoiceFee>(
                "SELECT * FROM \"invoice_fee\" WHERE \"invoice_id\"=@InvoiceId AND \"utc_disabled\" is NULL",
                new { InvoiceId = invoiceId });
        }

        public static List<Common.Models.Billing.InvoiceTime> ListInvoiceTimesForInvoice(Guid invoiceId)
        {
            return DataHelper.List<Common.Models.Billing.InvoiceTime, DBOs.Billing.InvoiceTime>(
                "SELECT * FROM \"invoice_time\" WHERE \"invoice_id\"=@InvoiceId AND \"utc_disabled\" is NULL",
                new { InvoiceId = invoiceId });
        }

        public static List<Common.Models.Billing.BillingGroup> ListBillableBillingGroups()
        {
            return DataHelper.List<Common.Models.Billing.BillingGroup, DBOs.Billing.BillingGroup>(
                "SELECT * FROM \"billing_group\" WHERE \"next_run\" <= now() at time zone 'utc'");
        }

        public static Common.Models.Billing.Invoice GetMostRecentInvoiceForContact(int contactId)
        {
            return DataHelper.Get<Common.Models.Billing.Invoice, DBOs.Billing.Invoice>(
                "SELECT * FROM \"invoice\" WHERE \"bill_to_contact_id\"=@BillToContactId ORDER BY \"utc_created\" DESC LIMIT 1",
                new { BillToContactId = contactId });
        }

        public static Common.Models.Billing.Invoice Create(Common.Models.Billing.Invoice model, Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Billing.Invoice dbo = Mapper.Map<DBOs.Billing.Invoice>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"invoice\" (\"id\", \"bill_to_contact_id\", \"date\", \"due\", " +
                    "\"subtotal\", \"tax_amount\", \"total\", \"external_invoice_id\", \"bill_to_name_line_1\", " +
                    "\"bill_to_name_line_2\", \"bill_to_address_line_1\", \"bill_to_address_line_2\", " +
                    "\"bill_to_city\", \"bill_to_state\", \"bill_to_zip\", \"matter_id\", \"billing_group_id\", " +
                    "\"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @BillToContactId, @Date, @Due, @Subtotal, @TaxAmount, " +
                    "@Total, @ExternalInvoiceId, @BillTo_NameLine1, @BillTo_NameLine2, @BillTo_AddressLine1, " +
                    "@BillTo_AddressLine2, @BillTo_City, @BillTo_State, @BillTo_Zip, @MatterId, @BillingGroupId, " +
                    "@UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Billing.Invoice Edit(Common.Models.Billing.Invoice model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Billing.Invoice dbo = Mapper.Map<DBOs.Billing.Invoice>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"invoice\" SET " +
                    "\"bill_to_contact_id\"=@BillToContactId, \"date\"=@Date, \"due\"=@Due, \"subtotal\"=@Subtotal, \"tax_amount\"=@TaxAmount, " +
                    "\"total\"=@Total, \"external_invoice_id\"=@ExternalInvoiceId, \"bill_to_name_line_1\"=@BillTo_NameLine1, \"bill_to_name_line_2\"=@BillTo_NameLine2, " +
                    "\"bill_to_address_line_1\"=@BillTo_AddressLine1, \"bill_to_address_line_2\"=@BillTo_AddressLine2, \"bill_to_city\"=@BillTo_City, " +
                    "\"bill_to_state\"=@BillTo_State, \"bill_to_zip\"=@BillTo_Zip, \"matter_id\"=@MatterId, \"billing_group_id\"=@BillingGroupId, " +
                    "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }
    }
}
