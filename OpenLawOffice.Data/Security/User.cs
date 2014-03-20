// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Nodine Legal, LLC">
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
    using AutoMapper;
    using System.Data;
    using Dapper;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class User
    {
        public static Common.Models.Security.User Get(int id)
        {
            return DataHelper.Get<Common.Models.Security.User, DBOs.Security.User>(
                "SELECT * FROM \"user\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Security.User Get(Guid authToken)
        {
            return DataHelper.Get<Common.Models.Security.User, DBOs.Security.User>(
                "SELECT * FROM \"user\" WHERE \"user_auth_token\"=@authToken AND \"utc_disabled\" is null",
                new { authToken = authToken });
        }

        public static Common.Models.Security.User Get(string username)
        {
            return DataHelper.Get<Common.Models.Security.User, DBOs.Security.User>(
                "SELECT * FROM \"user\" WHERE \"username\"=@username AND \"utc_disabled\" is null",
                new { username = username });
        }

        public static List<Common.Models.Security.User> List()
        {
            return DataHelper.List<Common.Models.Security.User, DBOs.Security.User>(
                "SELECT * FROM \"user\" WHERE \"utc_disabled\" is null");
        }

        public static Common.Models.Security.User Create(Common.Models.Security.User model)
        {
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;

            DBOs.Security.User dbo = Mapper.Map<DBOs.Security.User>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"user\" (\"username\", \"password\", \"password_salt\", \"utc_created\", \"utc_modified\") " +
                    "VALUES (@Username, @Password, @PasswordSalt, @UtcCreated, @UtcModified)",
                    dbo);
                model.Id = conn.Query<DBOs.Security.User>("SELECT currval(pg_get_serial_sequence('user', 'id')) AS \"id\"").Single().Id;
            }
            
            return model;
        }

        public static Common.Models.Security.User Edit(Common.Models.Security.User model)
        {
            model.UtcModified = DateTime.UtcNow;
            DBOs.Security.User dbo = Mapper.Map<DBOs.Security.User>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"user\" SET " +
                    "\"username\"=@Username, \"utc_modified\"=@UtcModified " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Security.User SetPassword(Common.Models.Security.User model)
        {
            model.UtcModified = DateTime.UtcNow;
            DBOs.Security.User dbo = Mapper.Map<DBOs.Security.User>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"user\" SET " +
                    "\"password\"=@Password, \"utc_modified\"=@UtcModified " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }
    }
}
