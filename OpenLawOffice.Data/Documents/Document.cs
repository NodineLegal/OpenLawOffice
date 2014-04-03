// -----------------------------------------------------------------------
// <copyright file="Document.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using Dapper;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Document
    {
        public static Common.Models.Documents.Document Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Documents.Document, DBOs.Documents.Document>(
                "SELECT * FROM \"document\" JOIN \"WHERE \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Documents.Document> ListForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Documents.Document, DBOs.Documents.Document>(
                "SELECT \"document\".* FROM \"document\" JOIN \"document_matter\" ON " +
                "\"document\".\"id\"=\"document_matter\".\"document_id\" " +
                "WHERE \"document_matter\".\"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Documents.Document> ListForTask(long taskId)
        {
            return DataHelper.List<Common.Models.Documents.Document, DBOs.Documents.Document>(
                "SELECT \"document\".* FROM \"document\" JOIN \"document_task\" ON " +
                "\"document\".\"id\"=\"document_task\".\"document_id\" " +
                "WHERE \"document_task\".\"task_id\"=@TaskId AND \"utc_disabled\" is null",
                new { TaskId = taskId });
        }

        public static Common.Models.Documents.Document Create(Common.Models.Documents.Document model, 
            Common.Models.Security.User creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            DBOs.Notes.Note dbo = Mapper.Map<DBOs.Notes.Note>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"document\" (\"id\", \"title\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @Title, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Documents.DocumentMatter RelateMatter(Common.Models.Documents.Document model,
            Guid matterId, Common.Models.Security.User creator)
        {
            return DocumentMatter.Create(new Common.Models.Documents.DocumentMatter()
            {
                Id = Guid.NewGuid(),
                Document = model,
                Matter = new Common.Models.Matters.Matter() { Id = matterId },
                CreatedBy = creator,
                ModifiedBy = creator,
                UtcCreated = DateTime.UtcNow,
                UtcModified = DateTime.UtcNow
            }, creator);
        }

        public static Common.Models.Documents.DocumentTask RelateTask(Common.Models.Documents.Document model,
            long taskId, Common.Models.Security.User creator)
        {
            return DocumentTask.Create(new Common.Models.Documents.DocumentTask()
            {
                Id = Guid.NewGuid(),
                Document = model,
                Task = new Common.Models.Tasks.Task { Id = taskId },
                CreatedBy = creator,
                ModifiedBy = creator,
                UtcCreated = DateTime.UtcNow,
                UtcModified = DateTime.UtcNow
            }, creator);
        }

        public static Common.Models.Documents.Document Edit(Common.Models.Documents.Document model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.UtcModified = DateTime.UtcNow;
            DBOs.Documents.Document dbo = Mapper.Map<DBOs.Documents.Document>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"document\" SET " +
                    "\"title\"=@Title, \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }
    }
}
