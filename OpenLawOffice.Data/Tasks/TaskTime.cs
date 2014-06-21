// -----------------------------------------------------------------------
// <copyright file="TaskTime.cs" company="Nodine Legal, LLC">
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
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TaskTime
    {
        public static Common.Models.Tasks.TaskTime Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskTime, DBOs.Tasks.TaskTime>(
                "SELECT * FROM \"task_time\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Tasks.TaskTime Get(long taskId, Guid timeId)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskTime, DBOs.Tasks.TaskTime>(
                "SELECT * FROM \"task_time\" WHERE \"task_id\"=@TaskId AND \"time_id\"=@TimeId AND \"utc_disabled\" is null",
                new { TaskId = taskId, TimeId = timeId });
        }

        public static Common.Models.Tasks.TaskTime Create(Common.Models.Tasks.TaskTime model,
            Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Tasks.TaskTime dbo = Mapper.Map<DBOs.Tasks.TaskTime>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"task_time\" (\"id\", \"task_id\", \"time_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @TaskId, @TimeId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            return model;
        }
    }
}