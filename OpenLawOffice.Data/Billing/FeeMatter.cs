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

    public class FeeMatter
    {
        public static Common.Models.Billing.FeeMatter Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Billing.FeeMatter, DBOs.Billing.FeeMatter>(
                "SELECT * FROM \"fee_matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                new { Id = id });
        }

        public static Common.Models.Billing.FeeMatter Get(Guid matterId, Guid feeId)
        {
            return DataHelper.Get<Common.Models.Billing.FeeMatter, DBOs.Billing.FeeMatter>(
                "SELECT * FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId AND \"fee_id\"=@FeeId AND \"utc_disabled\" is null",
                new { MatterId = matterId, FeeId = feeId });
        }

        public static Common.Models.Billing.FeeMatter Create(Common.Models.Billing.FeeMatter model,
            Common.Models.Account.Users creator)
        {
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Billing.FeeMatter dbo = Mapper.Map<DBOs.Billing.FeeMatter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                Common.Models.Billing.FeeMatter currentModel = Get(model.Matter.Id.Value, model.Fee.Id.Value);

                if (currentModel != null)
                { // Update
                    conn.Execute("UPDATE \"fee_matter\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                        "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null WHERE \"id\"=@Id", dbo);
                    model.Created = currentModel.Created;
                    model.CreatedBy = currentModel.CreatedBy;
                }
                else
                { // Create
                    conn.Execute("INSERT INTO \"fee_matter\" (\"id\", \"fee_id\", \"matter_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                        "VALUES (@Id, @FeeId, @MatterId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                        dbo);
                }
            }

            return model;
        }
    }
}
