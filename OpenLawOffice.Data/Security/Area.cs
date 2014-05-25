// -----------------------------------------------------------------------
// <copyright file="Area.cs" company="Nodine Legal, LLC">
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
    public static class Area
    {
        public static Common.Models.Security.Area Get(int id)
        {
            return DataHelper.Get<Common.Models.Security.Area, DBOs.Security.Area>(
                "SELECT * FROM \"area\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Security.Area Get(string name)
        {
            return DataHelper.Get<Common.Models.Security.Area, DBOs.Security.Area>(
                "SELECT * FROM \"area\" WHERE \"name\"=@Name AND \"utc_disabled\" is null",
                new { Name = name });
        }

        public static List<Common.Models.Security.Area> List()
        {
            return DataHelper.List<Common.Models.Security.Area, DBOs.Security.Area>(
                "SELECT * FROM \"area\" WHERE \"utc_disabled\" is null");
        }

        public static List<Common.Models.Security.Area> ListChildren(int? parentId)
        {
            List<Common.Models.Security.Area> list = new List<Common.Models.Security.Area>();
            IEnumerable<DBOs.Security.Area> ie = null;
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                if (parentId.HasValue)
                    ie = conn.Query<DBOs.Security.Area>(
                        "SELECT * FROM \"area\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null",
                        new { ParentId = parentId.Value });
                else
                    ie = conn.Query<DBOs.Security.Area>(
                        "SELECT * FROM \"area\" WHERE \"parent_id\" is null AND \"utc_disabled\" is null");
            }

            foreach (DBOs.Security.Area dbo in ie)
                list.Add(Mapper.Map<Common.Models.Security.Area>(dbo));

            return list;
        }

        public static Common.Models.Security.Area Create(Common.Models.Security.Area model,
            Common.Models.Security.User creator)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Security.Area dbo = Mapper.Map<DBOs.Security.Area>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"area\" (\"parent_id\", \"name\", \"description\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@ParentId, @Name, @Description, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Security.Area>("SELECT currval(pg_get_serial_sequence('area', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Security.Area Edit(Common.Models.Security.Area model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Matters.Matter dbo = Mapper.Map<DBOs.Matters.Matter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"area\" SET " +
                    "\"parent_id\"=@ParentId, \"name\"=@Name, \"description\"=@Description, \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }
    }
}