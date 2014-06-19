// -----------------------------------------------------------------------
// <copyright file="UserTaskSettings.cs" company="Nodine Legal, LLC">
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
// ------------------------------------------
namespace OpenLawOffice.Data.Settings
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
    public class UserTaskSettings
    {
        public static Common.Models.Settings.TagFilter GetTagFilter(long id)
        {
            return DataHelper.Get<Common.Models.Settings.TagFilter, DBOs.Settings.TagFilter>(
                "SELECT * FROM \"tag_filter\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Settings.TagFilter> ListTagFiltersFor(
            Common.Models.Account.Users user)
        {
            List<Common.Models.Settings.TagFilter> list =
                DataHelper.List<Common.Models.Settings.TagFilter, DBOs.Settings.TagFilter>(
                "SELECT * FROM \"tag_filter\" WHERE \"user_pid\"=@UserId AND \"utc_disabled\" is null",
                new { UserId = user.PId.Value });

            return list;
        }

        public static Common.Models.Settings.TagFilter CreateTagFilter(
            Common.Models.Settings.TagFilter model, Common.Models.Account.Users creator)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Settings.TagFilter dbo = Mapper.Map<DBOs.Settings.TagFilter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"tag_filter\" (\"user_pid\", \"category\", \"tag\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@UserId, @Category, @Tag, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Settings.TagFilter>("SELECT currval(pg_get_serial_sequence('tag_filter', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Settings.TagFilter EditTagFilter(Common.Models.Settings.TagFilter model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Settings.TagFilter dbo = Mapper.Map<DBOs.Settings.TagFilter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"tag_filter\" SET " +
                    "\"user_pid\"=@UserId, \"category\"=@Category, \"tag\"=@Tag, " +
                    "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static void DeleteTagFilter(Common.Models.Settings.TagFilter model,
            Common.Models.Account.Users deleter)
        {
            DBOs.Settings.TagFilter dbo = Mapper.Map<DBOs.Settings.TagFilter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("DELETE FROM \"tag_filter\" WHERE \"id\"=@Id", dbo);
            }
        }
    }
}