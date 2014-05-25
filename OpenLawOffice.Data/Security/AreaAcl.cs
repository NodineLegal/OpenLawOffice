// -----------------------------------------------------------------------
// <copyright file="AreaAcl.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Security
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
    public static class AreaAcl
    {
        public static Common.Models.Security.AreaAcl Get(int id)
        {
            return DataHelper.Get<Common.Models.Security.AreaAcl, DBOs.Security.AreaAcl>(
                "SELECT * FROM \"area_acl\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Security.AreaAcl Get(int userId, int areaId)
        {
            return DataHelper.Get<Common.Models.Security.AreaAcl, DBOs.Security.AreaAcl>(
                "SELECT * FROM \"area_acl\" WHERE \"user_id\"=@UserId AND \"security_area_id\"=@AreaId AND \"utc_disabled\" is null",
                new { UserId = userId, AreaId = areaId });
        }

        public static List<Common.Models.Security.AreaAcl> List()
        {
            return DataHelper.List<Common.Models.Security.AreaAcl, DBOs.Security.AreaAcl>(
                "SELECT * FROM \"area_acl\" WHERE \"utc_disabled\" is null");
        }

        public static List<Common.Models.Security.AreaAcl> ListForArea(int areaId)
        {
            return DataHelper.List<Common.Models.Security.AreaAcl, DBOs.Security.AreaAcl>(
                "SELECT * FROM \"area_acl\" WHERE \"security_area_id\"=@AreaId AND \"utc_disabled\" is null",
                new { AreaId = areaId });
        }

        public static Common.Models.Security.AreaAcl Create(Common.Models.Security.AreaAcl model,
            Common.Models.Security.User creator)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Security.AreaAcl dbo = Mapper.Map<DBOs.Security.AreaAcl>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"area_acl\" (\"security_area_id\", \"user_id\", \"allow_flags\", \"deny_flags\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@SecurityAreaId, @UserId, @AllowFlags, @DenyFlags, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Security.AreaAcl>("SELECT currval(pg_get_serial_sequence('area_acl', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Security.AreaAcl Edit(Common.Models.Security.AreaAcl model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Security.AreaAcl dbo = Mapper.Map<DBOs.Security.AreaAcl>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"area_acl\" SET " +
                    "\"security_area_id\"=@SecurityAreaId, \"user_id\"=@UserId, \"allow_flags\"=@AllowFlags, \"deny_flags\"=@DenyFlags, \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Security.AreaAcl Disable(Common.Models.Security.AreaAcl model,
            Common.Models.Security.User disabler)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Security.AreaAcl,
                DBOs.Security.AreaAcl>("area_acl", disabler.Id.Value, model.Id);

            return model;
        }

        public static Common.Models.Security.AreaAcl Enable(Common.Models.Security.AreaAcl model,
            Common.Models.Security.User enabler)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Security.AreaAcl,
                DBOs.Security.AreaAcl>("area_acl", enabler.Id.Value, model.Id);

            return model;
        }
    }
}