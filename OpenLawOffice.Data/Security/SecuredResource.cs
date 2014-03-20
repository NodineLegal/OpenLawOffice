// -----------------------------------------------------------------------
// <copyright file="SecuredResource.cs" company="Nodine Legal, LLC">
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
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using Dapper;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class SecuredResource
    {
        public static Common.Models.Security.SecuredResource Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Security.SecuredResource, DBOs.Security.SecuredResource>(
                "SELECT * FROM \"secured_resource\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Security.SecuredResource Create(Common.Models.Security.SecuredResource model,
            Common.Models.Security.User creator)
        {
            if (!model.Id.HasValue) throw new ArgumentException("SecuredResource must have its ID property set before calling Create().");
            model.CreatedBy = model.ModifiedBy = creator;
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            DBOs.Security.SecuredResource dbo = Mapper.Map<DBOs.Security.SecuredResource>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"secured_resource\" (\"id\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
            }

            // Secured Resource ACL
            Security.SecuredResourceAcl.Create(new Common.Models.Security.SecuredResourceAcl()
            {
                User = creator,
                SecuredResource = model,
                AllowFlags = Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllRead | Common.Models.PermissionType.AllWrite,
                DenyFlags = Common.Models.PermissionType.None
            }, creator);

            return model;
        }

        public static Common.Models.Security.SecuredResource Edit(Common.Models.Security.SecuredResource model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.UtcModified = DateTime.UtcNow;
            DBOs.Security.SecuredResource dbo = Mapper.Map<DBOs.Security.SecuredResource>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"secured_resource\" SET " +
                    "\"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }
    }
}
