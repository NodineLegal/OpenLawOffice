// -----------------------------------------------------------------------
// <copyright file="TaskMatter.cs" company="Nodine Legal, LLC">
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
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TaskMatter
    {
        public static Common.Models.Tasks.TaskMatter Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskMatter, DBOs.Tasks.TaskMatter>(
                "SELECT * FROM \"task_matter\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Tasks.TaskMatter Get(long taskId, Guid matterId)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskMatter, DBOs.Tasks.TaskMatter>(
                "SELECT * FROM \"task_matter\" WHERE \"task_id\"=@TaskId AND \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                new { TaskId = taskId, MatterId = matterId });
        }

        public static Common.Models.Tasks.TaskMatter GetFor(long taskId)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskMatter, DBOs.Tasks.TaskMatter>(
                "SELECT * FROM \"task_matter\" WHERE \"task_id\"=@TaskId AND \"utc_disabled\" is null",
                new { TaskId = taskId });
        }

        public static Common.Models.Tasks.TaskMatter Create(Common.Models.Tasks.TaskMatter model,
            Common.Models.Security.User creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Tasks.TaskMatter dbo = Mapper.Map<DBOs.Tasks.TaskMatter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"task_matter\" (\"id\", \"task_id\", \"matter_id\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @TaskId, @MatterId, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Tasks.TaskMatter>("SELECT currval(pg_get_serial_sequence('task_matter', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }
    }
}