// -----------------------------------------------------------------------
// <copyright file="SecuredResourceAcl.cs" company="Nodine Legal, LLC">
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
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class SecuredResourceAcl
    {
        public static Common.Models.Security.SecuredResourceAcl Get(int userId, Guid securedResourceId)
        {
            return DataHelper.Get<Common.Models.Security.SecuredResourceAcl, DBOs.Security.SecuredResourceAcl>(
                "SELECT * FROM \"secured_resource_acl\" WHERE \"user_id\"=@UserId AND \"secured_resource_id\"=@SecuredResourceId AND \"utc_disabled\" is null",
                new { UserId = userId, SecuredResourceId = securedResourceId });
        }

        public static Common.Models.Security.SecuredResourceAcl Create(Common.Models.Security.SecuredResourceAcl model,
            Common.Models.Security.User creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            DBOs.Security.SecuredResourceAcl dbo = Mapper.Map<DBOs.Security.SecuredResourceAcl>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"secured_resource_acl\" (\"id\", \"secured_resource_id\", \"user_id\", \"allow_flags\", \"deny_flags\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @SecuredResourceId, @UserId, @AllowFlags, @DenyFlags, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Security.SecuredResourceAcl>("SELECT currval(pg_get_serial_sequence('secured_resource_acl', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }
    }
}