// -----------------------------------------------------------------------
// <copyright file="ExpenseMatter.cs" company="Nodine Legal, LLC">
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
    using System.Data;
    using AutoMapper;
    using Dapper;
    using System.Collections.Generic;

    public class ExpenseMatter
    {
        public static Common.Models.Billing.ExpenseMatter Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Billing.ExpenseMatter, DBOs.Billing.ExpenseMatter>(
                "SELECT * FROM \"expense_matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                new { Id = id });
        }

        public static Common.Models.Billing.ExpenseMatter Get(Guid matterId, Guid expenseId)
        {
            return DataHelper.Get<Common.Models.Billing.ExpenseMatter, DBOs.Billing.ExpenseMatter>(
                "SELECT * FROM \"expense_matter\" WHERE \"matter_id\"=@MatterId AND \"expense_id\"=@ExpenseId AND \"utc_disabled\" is null",
                new { MatterId = matterId, ExpenseId = expenseId });
        }

        public static Common.Models.Billing.ExpenseMatter Create(Common.Models.Billing.ExpenseMatter model,
            Common.Models.Account.Users creator)
        {
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Billing.ExpenseMatter dbo = Mapper.Map<DBOs.Billing.ExpenseMatter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                Common.Models.Billing.ExpenseMatter currentModel = Get(model.Matter.Id.Value, model.Expense.Id.Value);

                if (currentModel != null)
                { // Update
                    conn.Execute("UPDATE \"expense_matter\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                        "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null WHERE \"id\"=@Id", dbo);
                    model.Created = currentModel.Created;
                    model.CreatedBy = currentModel.CreatedBy;
                }
                else
                { // Create
                    conn.Execute("INSERT INTO \"expense_matter\" (\"id\", \"expense_id\", \"matter_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                        "VALUES (@Id, @ExpenseId, @MatterId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                        dbo);
                }
            }

            return model;
        }
    }
}
