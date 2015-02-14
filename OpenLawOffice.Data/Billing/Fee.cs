// -----------------------------------------------------------------------
// <copyright file="Fee.cs" company="Nodine Legal, LLC">
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
    
    public class Fee
    {
        public static Common.Models.Billing.Fee Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT * FROM \"fee\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Matters.Matter GetMatter(Guid feeId)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"id\" IN (SELECT \"matter_id\" FROM " +
                "\"fee_matter\" WHERE \"fee_id\"=@FeeId AND \"utc_disabled\" is null) " +
                "AND \"matter\".\"utc_disabled\" is null",
                new { FeeId = feeId });
        }

        public static List<Common.Models.Billing.Fee> ListForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT * FROM \"fee\" WHERE \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "\"utc_disabled\" is null ORDER BY \"incurred\" ASC",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Billing.Fee> ListBilledFeesForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT * FROM \"fee\" WHERE " +
                "   \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "   \"id\" IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"fee_id\"=\"fee\".\"id\") AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Billing.Fee> ListUnbilledFeesForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT * FROM \"fee\" WHERE " +
                "   \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "   \"id\" NOT IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"fee_id\"=\"fee\".\"id\") AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId });
        }

        public static decimal SumUnbilledFeesForMatter(Guid matterId)
        {
            return DataHelper.Get<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT SUM(\"amount\") AS \"amount\" FROM \"fee\" WHERE \"id\" IN " +
                "   (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId AND \"fee_id\" NOT IN " +
                "       (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"utc_disabled\" is NULL) " +
                "   )",
                new { MatterId = matterId }).Amount;
        }

        public static Common.Models.Billing.Fee Create(Common.Models.Billing.Fee model, Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Billing.Fee dbo = Mapper.Map<DBOs.Billing.Fee>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"fee\" (\"id\", \"incurred\", \"amount\", \"details\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @Incurred, @Amount, @Details, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Billing.Fee Edit(Common.Models.Billing.Fee model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Billing.Fee dbo = Mapper.Map<DBOs.Billing.Fee>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"fee\" SET " +
                    "\"incurred\"=@Incurred, \"amount\"=@Amount, \"details\"=@Details, " +
                    "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Billing.FeeMatter RelateMatter(Common.Models.Billing.Fee model,
            Guid matterId, Common.Models.Account.Users creator)
        {
            return FeeMatter.Create(new Common.Models.Billing.FeeMatter()
            {
                Id = Guid.NewGuid(),
                Fee = model,
                Matter = new Common.Models.Matters.Matter() { Id = matterId },
                CreatedBy = creator,
                ModifiedBy = creator,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            }, creator);
        }
    }
}
