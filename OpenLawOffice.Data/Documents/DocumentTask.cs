// -----------------------------------------------------------------------
// <copyright file="DocumentTask.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Documents
{
    using System;
    using System.Data;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DocumentTask
    {
        public static Common.Models.Documents.DocumentTask Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Documents.DocumentTask, DBOs.Documents.DocumentTask>(
                "SELECT * FROM \"document_task\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                new { Id = id });
        }

        public static Common.Models.Documents.DocumentTask Get(long taskId, Guid documentId)
        {
            return DataHelper.Get<Common.Models.Documents.DocumentTask, DBOs.Documents.DocumentTask>(
                "SELECT * FROM \"document_task\" WHERE \"task_id\"=@TaskId AND \"document_id\"=@DocumentId AND \"utc_disabled\" is null",
                new { TaskId = taskId, DocumentId = documentId });
        }

        public static Common.Models.Documents.DocumentTask GetFor(Guid documentId)
        {
            return DataHelper.Get<Common.Models.Documents.DocumentTask, DBOs.Documents.DocumentTask>(
                "SELECT * FROM \"document_task\" WHERE \"document_id\"=@DocumentId AND \"utc_disabled\" is null",
                new { DocumentId = documentId });
        }

        public static Common.Models.Documents.DocumentTask GetIgnoringDisable(Guid taskId, Guid documentId)
        {
            return DataHelper.Get<Common.Models.Documents.DocumentTask, DBOs.Documents.DocumentTask>(
                "SELECT * FROM \"document_task\" WHERE \"task_id\"=@MatterId AND \"document_id\"=@DocumentId",
                new { TaskId = taskId, DocumentId = documentId });
        }

        public static Common.Models.Tasks.Task GetRelatedMatter(Guid documentId)
        {
            return DataHelper.Get<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT \"task\".* FROM \"document_task\" JOIN \"task\" ON \"document_task\".\"task_id\"=\"task\".\"id\" " +
                "WHERE \"document_task\".\"document_id\"=@DocumentId " +
                "AND \"document_task\".\"utc_disabled\" is null " +
                "AND \"task\".\"utc_disabled\" is null ",
                new { DocumentId = documentId });
        }

        public static Common.Models.Documents.DocumentTask Create(Common.Models.Documents.DocumentTask model,
            Common.Models.Account.Users creator)
        {
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Documents.DocumentTask dbo = Mapper.Map<DBOs.Documents.DocumentTask>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                Common.Models.Documents.DocumentTask currentModel = Get(model.Task.Id.Value, model.Document.Id.Value);

                if (currentModel != null)
                { // Update
                    conn.Execute("UPDATE \"document_task\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserId " +
                        "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null WHERE \"id\"=@Id", dbo);
                    model.Created = currentModel.Created;
                    model.CreatedBy = currentModel.CreatedBy;
                }
                else
                { // Create
                    conn.Execute("INSERT INTO \"document_task\" (\"id\", \"document_id\", \"task_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                        "VALUES (@Id, @DocumentId, @TaskId, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                        dbo);
                }
            }

            return model;
        }
    }
}