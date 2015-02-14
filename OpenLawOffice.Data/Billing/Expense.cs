// -----------------------------------------------------------------------
// <copyright file="Expense.cs" company="Nodine Legal, LLC">
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

    public class Expense
    {
        public static Common.Models.Billing.Expense Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Billing.Expense, DBOs.Billing.Expense>(
                "SELECT * FROM \"expense\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Matters.Matter GetMatter(Guid expenseId)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"id\" IN (SELECT \"matter_id\" FROM " +
                "\"expense_matter\" WHERE \"expense_id\"=@ExpenseId AND \"utc_disabled\" is null) " +
                "AND \"matter\".\"utc_disabled\" is null",
                new { ExpenseId = expenseId });
        }

        public static List<Common.Models.Billing.Expense> ListForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.Expense, DBOs.Billing.Expense>(
                "SELECT * FROM \"expense\" WHERE \"id\" IN (SELECT \"expense_id\" FROM \"expense_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "\"utc_disabled\" is null ORDER BY \"incurred\" ASC",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Billing.Expense> ListBilledExpensesForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.Expense, DBOs.Billing.Expense>(
                "SELECT * FROM \"expense\" WHERE " + 
                "   \"id\" IN (SELECT \"expense_id\" FROM \"expense_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "   \"id\" IN (SELECT \"expense_id\" FROM \"invoice_expense\" WHERE \"expense_id\"=\"expense\".\"id\") AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Billing.Expense> ListUnbilledExpensesForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.Expense, DBOs.Billing.Expense>(
                "SELECT * FROM \"expense\" WHERE " +
                "   \"id\" IN (SELECT \"expense_id\" FROM \"expense_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "   \"id\" NOT IN (SELECT \"expense_id\" FROM \"invoice_expense\" WHERE \"expense_id\"=\"expense\".\"id\") AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId });
        }

        public static decimal SumUnbilledExpensesForMatter(Guid matterId)
        {
            return DataHelper.Get<Common.Models.Billing.Expense, DBOs.Billing.Expense>(
                "SELECT SUM(\"amount\") AS \"amount\" FROM \"expense\" WHERE \"id\" IN " +
                "   (SELECT \"expense_id\" FROM \"expense_matter\" WHERE \"matter_id\"=@MatterId AND \"expense_id\" NOT IN " +
                "       (SELECT \"expense_id\" FROM \"invoice_expense\" WHERE \"utc_disabled\" is NULL) " +
                "   )",
                new { MatterId = matterId }).Amount;
        }

        public static Common.Models.Billing.Expense Create(Common.Models.Billing.Expense model, Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Billing.Expense dbo = Mapper.Map<DBOs.Billing.Expense>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"expense\" (\"id\", \"incurred\", \"paid\", \"vendor\", \"amount\", \"details\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @Incurred, @Paid, @Vendor, @Amount, @Details, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Billing.Expense Edit(Common.Models.Billing.Expense model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Billing.Expense dbo = Mapper.Map<DBOs.Billing.Expense>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"expense\" SET " +
                    "\"incurred\"=@Incurred, \"paid\"=@Paid, \"vendor\"=@Vendor, \"amount\"=@Amount, \"details\"=@Details, " +
                    "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Billing.ExpenseMatter RelateMatter(Common.Models.Billing.Expense model,
            Guid matterId, Common.Models.Account.Users creator)
        {
            return ExpenseMatter.Create(new Common.Models.Billing.ExpenseMatter()
            {
                Id = Guid.NewGuid(),
                Expense = model,
                Matter = new Common.Models.Matters.Matter() { Id = matterId },
                CreatedBy = creator,
                ModifiedBy = creator,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            }, creator);
        }
    }
}
