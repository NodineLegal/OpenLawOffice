// <copyright file="TaskResponsibleUser.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Tasks
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
    public static class TaskResponsibleUser
    {
        public static Common.Models.Tasks.TaskResponsibleUser Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskResponsibleUser, DBOs.Tasks.TaskResponsibleUser>(
                "SELECT * FROM \"task_responsible_user\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Tasks.TaskResponsibleUser Get(long taskId, int userId)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskResponsibleUser, DBOs.Tasks.TaskResponsibleUser>(
                "SELECT * FROM \"task_responsible_user\" WHERE \"task_id\"=@TaskId AND \"user_id\"=@UserId AND \"utc_disabled\" is null",
                new { TaskId = taskId, UserId = userId });
        }

        public static Common.Models.Tasks.TaskResponsibleUser Create(Common.Models.Tasks.TaskResponsibleUser model,
            Common.Models.Security.User creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Tasks.TaskResponsibleUser dbo = Mapper.Map<DBOs.Tasks.TaskResponsibleUser>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"task_responsible_user\" (\"id\", \"task_id\", \"user_id\", \"responsibility\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @TaskId, @UserId, @Responsibility, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Tasks.TaskAssignedContact>("SELECT currval(pg_get_serial_sequence('task_responsible_user', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }
    }
}
