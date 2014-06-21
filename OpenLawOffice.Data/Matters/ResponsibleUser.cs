// -----------------------------------------------------------------------
// <copyright file="ResponsibleUser.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Matters
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ResponsibleUser
    {
        public static Common.Models.Matters.ResponsibleUser Get(int id)
        {
            return DataHelper.Get<Common.Models.Matters.ResponsibleUser, DBOs.Matters.ResponsibleUser>(
                "SELECT * FROM \"responsible_user\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Matters.ResponsibleUser Get(Guid matterId, Guid userPId)
        {
            return DataHelper.Get<Common.Models.Matters.ResponsibleUser, DBOs.Matters.ResponsibleUser>(
                "SELECT * FROM \"responsible_user\" WHERE \"matter_id\"=@MatterId AND \"user_pid\"=@UserPId AND \"utc_disabled\" is null",
                new { MatterId = matterId, UserPId = userPId });
        }

        public static Common.Models.Matters.ResponsibleUser GetIgnoringDisable(Guid matterId, Guid userPId)
        {
            return DataHelper.Get<Common.Models.Matters.ResponsibleUser, DBOs.Matters.ResponsibleUser>(
                "SELECT * FROM \"responsible_user\" WHERE \"matter_id\"=@MatterId AND \"user_pid\"=@UserPId",
                new { MatterId = matterId, UserPId = userPId });
        }

        public static List<Common.Models.Matters.ResponsibleUser> ListForMatter(Guid matterId)
        {
            List<Common.Models.Matters.ResponsibleUser> list =
                DataHelper.List<Common.Models.Matters.ResponsibleUser, DBOs.Matters.ResponsibleUser>(
                "SELECT * FROM \"responsible_user\" WHERE \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                new { MatterId = matterId });

            list.ForEach(x =>
            {
                x.User = Account.Users.Get(x.User.PId.Value);
            });

            return list;
        }

        public static Common.Models.Matters.ResponsibleUser Create(Common.Models.Matters.ResponsibleUser model,
            Common.Models.Account.Users creator)
        {
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Matters.ResponsibleUser dbo = Mapper.Map<DBOs.Matters.ResponsibleUser>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"responsible_user\" (\"matter_id\", \"user_pid\", \"responsibility\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@MatterId, @UserPId, @Responsibility, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
                model.Id = conn.Query<DBOs.Matters.ResponsibleUser>("SELECT currval(pg_get_serial_sequence('responsible_user', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Matters.ResponsibleUser Edit(Common.Models.Matters.ResponsibleUser model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Matters.ResponsibleUser dbo = Mapper.Map<DBOs.Matters.ResponsibleUser>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"responsible_user\" SET " +
                    "\"matter_id\"=@MatterId, \"user_pid\"=@UserPId, \"responsibility\"=@Responsibility, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Matters.ResponsibleUser Disable(Common.Models.Matters.ResponsibleUser model,
            Common.Models.Account.Users disabler)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("responsible_user", disabler.PId.Value, model.Id);

            return model;
        }

        public static Common.Models.Matters.ResponsibleUser Enable(Common.Models.Matters.ResponsibleUser model,
            Common.Models.Account.Users enabler)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("responsible_user", enabler.PId.Value, model.Id);

            return model;
        }
    }
}