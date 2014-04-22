// -----------------------------------------------------------------------
// <copyright file="NoteTask.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Notes
{
    using System;
    using System.Data;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NoteTask
    {
        public static Common.Models.Notes.NoteTask Get(long taskId, Guid noteId)
        {
            return DataHelper.Get<Common.Models.Notes.NoteTask, DBOs.Notes.NoteTask>(
                "SELECT * FROM \"note_task\" WHERE \"task_id\"=@TaskId AND \"note_id\"=@NoteId AND \"utc_disabled\" is null",
                new { TaskId = taskId, NoteId = noteId });
        }

        public static Common.Models.Notes.NoteTask GetIgnoringDisable(long taskId, Guid noteId)
        {
            return DataHelper.Get<Common.Models.Notes.NoteTask, DBOs.Notes.NoteTask>(
                "SELECT * FROM \"note_task\" WHERE \"task_id\"=@TaskId AND \"note_id\"=@NoteId",
                new { TaskId = taskId, NoteId = noteId });
        }

        public static Common.Models.Tasks.Task GetRelatedTask(Guid noteId)
        {
            return DataHelper.Get<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT \"task\".* FROM \"note_task\" JOIN \"task\" ON \"note_task\".\"task_id\"=\"task\".\"id\" " +
                "WHERE \"note_task\".\"note_id\"=@NoteId " +
                "AND \"note_task\".\"utc_disabled\" is null " +
                "AND \"task\".\"utc_disabled\" is null ",
                new { NoteId = noteId });
        }

        public static Common.Models.Notes.NoteTask Create(Common.Models.Notes.NoteTask model,
            Common.Models.Security.User creator)
        {
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Notes.NoteTask dbo = Mapper.Map<DBOs.Notes.NoteTask>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                Common.Models.Notes.NoteTask currentModel = Get(model.Task.Id.Value, model.Note.Id.Value);

                if (currentModel != null)
                { // Update
                    conn.Execute("UPDATE \"note_task\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                        "\"utc_disabled\"=null, \"disabled_by_user_id\"=null WHERE \"id\"=@Id", dbo);
                    model.UtcCreated = currentModel.UtcCreated;
                    model.CreatedBy = currentModel.CreatedBy;
                }
                else
                { // Create
                    conn.Execute("INSERT INTO \"note_task\" (\"id\", \"note_id\", \"task_id\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                        "VALUES (@Id, @NoteId, @TaskId, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                        dbo);
                }
            }

            return model;
        }
    }
}