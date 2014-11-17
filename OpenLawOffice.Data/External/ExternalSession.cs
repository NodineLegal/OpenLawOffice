// -----------------------------------------------------------------------
// <copyright file="ExternalSession.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.External
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    public static class ExternalSession
    {
        public static Common.Models.External.ExternalSession Get(Guid token)
        {
            return DataHelper.Get<Common.Models.External.ExternalSession, DBOs.External.ExternalSession>(
                "SELECT * FROM \"external_session\" WHERE \"id\"=@Id",
                new { Id = token });
        }

        public static Common.Models.External.ExternalSession Get(string appName, Guid machineId, string username)
        {
            return DataHelper.Get<Common.Models.External.ExternalSession, DBOs.External.ExternalSession>(
                "SELECT * FROM \"external_session\" WHERE \"app_name\"=@AppName AND \"machine_id\"=@MachineId " +
                "AND \"user_pid\" IN (SELECT \"pId\" FROM \"Users\" WHERE \"Username\"=@Username)",
                new { AppName = appName, MachineId = machineId, Username = username });
        }

        public static Common.Models.External.ExternalSession Create(Common.Models.External.ExternalSession model)
        {
            model.Id = Guid.NewGuid();
            model.Created = DateTime.UtcNow;
            model.Timeout = 15 * 60; // 15 minutes
            model.Expires = model.Created.AddSeconds(model.Timeout);

            DBOs.External.ExternalSession dbo = Mapper.Map<DBOs.External.ExternalSession>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"external_session\" (\"id\", \"user_pid\", \"app_name\", \"utc_created\", \"utc_expires\", \"timeout\", \"machine_id\") " +
                    "VALUES (@Id, @UserPId, @AppName, @UtcCreated, @UtcExpires, @Timeout, @MachineId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.External.ExternalSession Update(Common.Models.External.ExternalSession model)
        {
            Delete(model);
            return Create(model);
        }

        public static Common.Models.External.ExternalSession Renew(Common.Models.External.ExternalSession model)
        {
            Common.Models.External.ExternalSession curSes = Get(model.Id.Value);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"external_session\" SET " +
                    "\"utc_expires\"=@UtcExpires WHERE \"id\"=@Id",
                    new { Id = model.Id.Value, UtcExpires = DateTime.UtcNow.AddSeconds(curSes.Timeout) });
            }

            return model;
        }

        public static Common.Models.External.ExternalSession Delete(Common.Models.External.ExternalSession model)
        {
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("DELETE FROM \"external_session\" WHERE \"id\"=@Id",
                    new { Id = model.Id.Value });
            }

            return model;
        }
    }
}
