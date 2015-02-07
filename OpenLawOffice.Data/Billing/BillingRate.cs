// -----------------------------------------------------------------------
// <copyright file="BillingRate.cs" company="Nodine Legal, LLC">
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

    public class BillingRate
    {
        public static Common.Models.Billing.BillingRate Get(int id)
        {
            return DataHelper.Get<Common.Models.Billing.BillingRate, DBOs.Billing.BillingRate>(
                "SELECT * FROM \"billing_rate\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Billing.BillingRate> List()
        {
            return DataHelper.List<Common.Models.Billing.BillingRate, DBOs.Billing.BillingRate>(
                "SELECT * FROM \"billing_rate\" WHERE \"utc_disabled\" is null");
        }

        public static Common.Models.Billing.BillingRate Create(Common.Models.Billing.BillingRate model, 
            Common.Models.Account.Users creator)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Billing.BillingRate dbo = Mapper.Map<DBOs.Billing.BillingRate>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"billing_rate\" (\"title\", \"price_per_unit\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Title, @PricePerUnit, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);

                model.Id = conn.Query<DBOs.Contacts.Contact>("SELECT currval(pg_get_serial_sequence('billing_rate', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Billing.BillingRate Edit(Common.Models.Billing.BillingRate model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Billing.BillingRate dbo = Mapper.Map<DBOs.Billing.BillingRate>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"billing_rate\" SET " +
                    "\"title\"=@Title, \"price_per_unit\"=@PricePerUnit, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }
    }
}
