// -----------------------------------------------------------------------
// <copyright file="InvoiceFee.cs" company="Nodine Legal, LLC">
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

    public class InvoiceFee
    {
        public static List<Common.Models.Billing.InvoiceFee> ListForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.InvoiceFee, DBOs.Billing.InvoiceFee>(
                "SELECT * FROM \"invoice_fee\" WHERE \"fee_id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId });
        }

        public static Common.Models.Billing.InvoiceFee Get(Guid invoiceId, Guid feeId)
        {
            return DataHelper.Get<Common.Models.Billing.InvoiceFee, DBOs.Billing.InvoiceFee>(
                "SELECT * FROM \"invoice_fee\" WHERE \"invoice_id\"=@InvoiceId AND \"fee_id\"=@FeeId",
                new { InvoiceId = invoiceId, FeeId = feeId });
        }

        public static Common.Models.Billing.InvoiceFee Create(Common.Models.Billing.InvoiceFee model,
            Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Billing.InvoiceFee dbo = Mapper.Map<DBOs.Billing.InvoiceFee>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                Common.Models.Billing.InvoiceFee currentModel = Get(model.Invoice.Id.Value, model.Fee.Id.Value);

                if (currentModel != null)
                { // Update
                    conn.Execute("UPDATE \"invoice_fee\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                        "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null WHERE \"id\"=@Id", dbo);
                    model.Created = currentModel.Created;
                    model.CreatedBy = currentModel.CreatedBy;
                }
                else
                { // Create
                    conn.Execute("INSERT INTO \"invoice_fee\" (\"id\", \"fee_id\", \"invoice_id\", \"amount\", \"details\", \"tax_amount\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                        "VALUES (@Id, @FeeId, @InvoiceId, @Amount, @Details, @TaxAmount, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                        dbo);
                }
            }

            return model;
        }
    }
}
