// -----------------------------------------------------------------------
// <copyright file="Matter.cs" company="Nodine Legal, LLC">
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
    using Dapper;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Matter
    {
        public static Common.Models.Matters.Matter Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Matters.Matter> List()
        {
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null");
        }

        public static List<Common.Models.Matters.Matter> ListChildren(Guid? parentId)
        {
            List<Common.Models.Matters.Matter> list = new List<Common.Models.Matters.Matter>();
            IEnumerable<DBOs.Matters.Matter> ie = null;
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                if (parentId.HasValue)
                    ie = conn.Query<DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" JOIN \"secured_resource_acl\" ON " +
                        "\"matter\".\"id\"=\"secured_resource_acl\".\"secured_resource_id\" " +
                        "WHERE \"secured_resource_acl\".\"allow_flags\" & 2 > 0 " +
                        "AND NOT \"secured_resource_acl\".\"deny_flags\" & 2 > 0 " +
                        "AND \"matter\".\"utc_disabled\" is null  " +
                        "AND \"secured_resource_acl\".\"utc_disabled\" is null " +
                        "AND \"matter\".\"parent_id\"=@ParentId", new { ParentId = parentId.Value });
                else
                    ie = conn.Query<DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" JOIN \"secured_resource_acl\" ON " +
                        "\"matter\".\"id\"=\"secured_resource_acl\".\"secured_resource_id\" " +
                        "WHERE \"secured_resource_acl\".\"allow_flags\" & 2 > 0 " +
                        "AND NOT \"secured_resource_acl\".\"deny_flags\" & 2 > 0 " +
                        "AND \"matter\".\"utc_disabled\" is null  " +
                        "AND \"secured_resource_acl\".\"utc_disabled\" is null " +
                        "AND \"matter\".\"parent_id\" is null");
            }

            foreach (DBOs.Matters.Matter dbo in ie)
                list.Add(Mapper.Map<Common.Models.Matters.Matter>(dbo));

            return list;
        }

        public static Common.Models.Matters.Matter Create(Common.Models.Matters.Matter model,
            Common.Models.Security.User creator)
        {
            // Matter
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            DBOs.Matters.Matter dbo = Mapper.Map<DBOs.Matters.Matter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"matter\" (\"id\", \"title\", \"parent_id\", \"synopsis\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @Title, @ParentId, @Synopsis, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
            }

            // Secured Resource
            Security.SecuredResource.Create(new Common.Models.Security.SecuredResource()
            {
                Id = model.Id
            }, creator);

            return model;
        }

        public static Common.Models.Matters.Matter Edit(Common.Models.Matters.Matter model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.UtcModified = DateTime.UtcNow;
            DBOs.Matters.Matter dbo = Mapper.Map<DBOs.Matters.Matter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"matter\" SET " +
                    "\"title\"=@Title, \"parent_id\"=@ParentId, \"synopsis\"=@Synopsis, \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            // Secured Resource
            Security.SecuredResource.Edit(new Common.Models.Security.SecuredResource()
            {
                Id = model.Id,
                ModifiedBy = modifier,
                UtcModified = model.UtcModified
            }, modifier);

            return model;
        }
    }
}
