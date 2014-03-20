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
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using System.Data;
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

        public static Common.Models.Matters.ResponsibleUser Get(Guid matterId, int userId)
        {
            return DataHelper.Get<Common.Models.Matters.ResponsibleUser, DBOs.Matters.ResponsibleUser>(
                "SELECT * FROM \"responsible_user\" WHERE \"matter_id\"=@MatterId AND \"user_id\"=@UserId AND \"utc_disabled\" is null",
                new { MatterId = matterId, UserId = userId });
        }

        public static Common.Models.Matters.ResponsibleUser GetIgnoringDisable(Guid matterId, int userId)
        {
            return DataHelper.Get<Common.Models.Matters.ResponsibleUser, DBOs.Matters.ResponsibleUser>(
                "SELECT * FROM \"responsible_user\" WHERE \"matter_id\"=@MatterId AND \"user_id\"=@UserId",
                new { MatterId = matterId, UserId = userId });
        }

        public static List<Common.Models.Matters.ResponsibleUser> ListForMatter(Guid matterId)
        {
            List<Common.Models.Matters.ResponsibleUser> list =
                DataHelper.List<Common.Models.Matters.ResponsibleUser, DBOs.Matters.ResponsibleUser>(
                "SELECT * FROM \"responsible_user\" WHERE \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                new { MatterId = matterId });

            list.ForEach(x =>
            {
                x.User = Security.User.Get(x.User.Id.Value);
            });

            return list;
        }

        public static Common.Models.Matters.ResponsibleUser Create(Common.Models.Matters.ResponsibleUser model,
            Common.Models.Security.User creator)
        {
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Matters.ResponsibleUser dbo = Mapper.Map<DBOs.Matters.ResponsibleUser>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"responsible_user\" (\"matter_id\", \"user_id\", \"responsibility\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@MatterId, @UserId, @Responsibility, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Matters.ResponsibleUser>("SELECT currval(pg_get_serial_sequence('responsible_user', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Matters.ResponsibleUser Edit(Common.Models.Matters.ResponsibleUser model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.UtcModified = DateTime.UtcNow;
            DBOs.Matters.ResponsibleUser dbo = Mapper.Map<DBOs.Matters.ResponsibleUser>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"responsible_user\" SET " +
                    "\"matter_id\"=@MatterId, \"user_id\"=@UserId, \"responsibility\"=@Responsibility, \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Matters.ResponsibleUser Disable(Common.Models.Matters.ResponsibleUser model,
            Common.Models.Security.User disabler)
        {
            model.DisabledBy = disabler;
            model.UtcDisabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("responsible_user", disabler.Id.Value);

            return model;
        }

        public static Common.Models.Matters.ResponsibleUser Enable(Common.Models.Matters.ResponsibleUser model,
            Common.Models.Security.User enabler)
        {
            model.ModifiedBy = enabler;
            model.UtcModified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.UtcDisabled = null;

            DataHelper.Enable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("responsible_user", enabler.Id.Value);

            return model;
        }
    }
}
