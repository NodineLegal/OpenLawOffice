// -----------------------------------------------------------------------
// <copyright file="Time.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Timing
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Time
    {
        public static Common.Models.Timing.Time Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Timing.Time, DBOs.Timing.Time>(
                "SELECT * FROM \"time\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Timing.Time> ListForTask(long taskId)
        {
            return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                "SELECT \"time\".* FROM \"time\" JOIN \"task_time\" ON \"time\".\"id\"=\"task_time\".\"time_id\" " +
                "WHERE \"task_id\"=@TaskId AND \"time\".\"utc_disabled\" is null AND \"task_time\".\"utc_disabled\" is null",
                new { TaskId = taskId });
        }

        public static Common.Models.Tasks.Task GetRelatedTask(Guid timeId)
        {
            return DataHelper.Get<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT \"task\".* FROM \"task\" JOIN \"task_time\" ON \"task\".\"id\"=\"task_time\".\"task_id\" " +
                "WHERE \"time_id\"=@TimeId AND \"task\".\"utc_disabled\" is null AND \"task_time\".\"utc_disabled\" is null",
                new { TimeId = timeId });
        }

        public static Common.Models.Timing.Time Create(Common.Models.Timing.Time model,
            Common.Models.Security.User creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            DBOs.Timing.Time dbo = Mapper.Map<DBOs.Timing.Time>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"time\" (\"id\", \"start\", \"stop\", \"worker_contact_id\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @Start, @Stop, @WorkerContactId, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Timing.Time Edit(Common.Models.Timing.Time model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.UtcModified = DateTime.UtcNow;
            DBOs.Timing.Time dbo = Mapper.Map<DBOs.Timing.Time>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"time\" SET " +
                    "\"start\"=@Start, \"stop\"=@Stop, \"worker_contact_id\"=@WorkerContactId, \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Tasks.TaskTime RelateTask(Common.Models.Timing.Time timeModel,
            long taskId, Common.Models.Security.User creator)
        {
            Common.Models.Tasks.TaskTime taskTime = new Common.Models.Tasks.TaskTime()
            {
                Id = Guid.NewGuid(),
                Task = new Common.Models.Tasks.Task() { Id = taskId, IsStub = true },
                Time = timeModel,
                UtcCreated = DateTime.UtcNow,
                UtcModified = DateTime.UtcNow,
                CreatedBy = creator,
                ModifiedBy = creator
            };

            DBOs.Tasks.TaskTime dbo = Mapper.Map<DBOs.Tasks.TaskTime>(taskTime);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"task_time\" (\"id\", \"task_id\", \"time_id\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @TaskId, @TimeId, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
            }

            return taskTime;
        }
    }
}