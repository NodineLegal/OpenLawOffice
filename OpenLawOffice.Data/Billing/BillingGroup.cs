// -----------------------------------------------------------------------
// <copyright file="BillingGroup.cs" company="Nodine Legal, LLC">
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
    using System.Linq;
    using AutoMapper;
    using Dapper;

    public class BillingGroup
    {
        public static Common.Models.Billing.BillingGroup Get(int id)
        {
            return DataHelper.Get<Common.Models.Billing.BillingGroup, DBOs.Billing.BillingGroup>(
                "SELECT * FROM \"billing_group\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Billing.BillingGroup> List()
        {
            return DataHelper.List<Common.Models.Billing.BillingGroup, DBOs.Billing.BillingGroup>(
                "SELECT * FROM \"billing_group\" WHERE \"utc_disabled\" is null");
        }

        public static Common.Models.Billing.BillingGroup Create(Common.Models.Billing.BillingGroup model,
            Common.Models.Account.Users creator)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Billing.BillingGroup dbo = Mapper.Map<DBOs.Billing.BillingGroup>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"billing_group\" (\"title\", \"last_run\", \"next_run\", \"amount\", \"bill_to_contact_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Title, @LastRun, @NextRun, @Amount, @BillToContactId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);

                model.Id = conn.Query<DBOs.Billing.BillingGroup>("SELECT currval(pg_get_serial_sequence('billing_group', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Billing.BillingGroup Edit(Common.Models.Billing.BillingGroup model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Billing.BillingGroup dbo = Mapper.Map<DBOs.Billing.BillingGroup>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"billing_group\" SET " +
                    "\"title\"=@Title, \"last_run\"=@LastRun, \"next_run\"=@NextRun, \"amount\"=@Amount, \"bill_to_contact_id\"=@BillToContactId, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static decimal SumExpensesForGroup(int billingGroupId)
        {
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                IEnumerable<dynamic> result = conn.Query("SELECT SUM(\"amount\") AS \"Amount\" FROM \"expense\" WHERE \"id\" IN " +
                    "(SELECT \"expense_id\" FROM \"expense_matter\" WHERE \"matter_id\" IN " +
                        "(SELECT \"id\" FROM \"matter\" WHERE \"billing_group_id\"=@BillingGroupId) " +
                    ")", new { BillingGroupId = billingGroupId });

                IEnumerator<dynamic> enumerator = result.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    return enumerator.Current.Amount;
                }

                return 0;
            }
        }
    }
}
