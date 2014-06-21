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
    using System.Data;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Document
    {
        public static Common.Models.Documents.Document Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Documents.Document, DBOs.Documents.Document>(
                "SELECT * FROM \"document\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                new { Id = id });
        }

        public static Common.Models.Matters.Matter GetMatter(Guid documentId)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT \"matter\".* FROM \"matter\" JOIN \"document_matter\" ON " +
                "\"matter\".\"id\"=\"document_matter\".\"matter_id\" " +
                "WHERE \"document_matter\".\"document_id\"=@DocumentId " +
                "AND \"matter\".\"utc_disabled\" is null " +
                "AND \"document_matter\".\"utc_disabled\" is null",
                new { DocumentId = documentId });
        }

        public static Common.Models.Tasks.Task GetTask(Guid documentId)
        {
            return DataHelper.Get<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT \"task\".* FROM \"task\" JOIN \"document_task\" ON " +
                "\"task\".\"id\"=\"document_task\".\"task_id\" " +
                "WHERE \"document_task\".\"document_id\"=@DocumentId " +
                "AND \"task\".\"utc_disabled\" is null " +
                "AND \"document_task\".\"utc_disabled\" is null",
                new { DocumentId = documentId });
        }

        public static List<Common.Models.Documents.Document> ListForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Documents.Document, DBOs.Documents.Document>(
                "SELECT \"document\".* FROM \"document\" JOIN \"document_matter\" ON " +
                "\"document\".\"id\"=\"document_matter\".\"document_id\" " +
                "WHERE \"document_matter\".\"matter_id\"=@MatterId " +
                "AND \"document\".\"utc_disabled\" is null " +
                "AND \"document_matter\".\"utc_disabled\" is null " +
                "ORDER BY \"document\".\"date\" DESC, \"document\".\"title\" ASC",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Documents.Document> ListForTask(long taskId)
        {
            return DataHelper.List<Common.Models.Documents.Document, DBOs.Documents.Document>(
                "SELECT \"document\".* FROM \"document\" JOIN \"document_task\" ON " +
                "\"document\".\"id\"=\"document_task\".\"document_id\" " +
                "WHERE \"document_task\".\"task_id\"=@TaskId " +
                "AND \"document\".\"utc_disabled\" is null " +
                "AND \"document_task\".\"utc_disabled\" is null " +
                "ORDER BY \"document\".\"date\" DESC, \"document\".\"title\" ASC",
                new { TaskId = taskId });
        }

        public static Common.Models.Documents.Document Create(Common.Models.Documents.Document model,
            Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Documents.Document dbo = Mapper.Map<DBOs.Documents.Document>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"document\" (\"id\", \"date\", \"title\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @Date, @Title, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Documents.DocumentMatter RelateMatter(Common.Models.Documents.Document model,
            Guid matterId, Common.Models.Account.Users creator)
        {
            return DocumentMatter.Create(new Common.Models.Documents.DocumentMatter()
            {
                Id = Guid.NewGuid(),
                Document = model,
                Matter = new Common.Models.Matters.Matter() { Id = matterId },
                CreatedBy = creator,
                ModifiedBy = creator,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            }, creator);
        }

        public static Common.Models.Documents.DocumentTask RelateTask(Common.Models.Documents.Document model,
            long taskId, Common.Models.Account.Users creator)
        {
            Common.Models.Tasks.TaskMatter taskMatter = Tasks.TaskMatter.GetFor(taskId);

            RelateMatter(model, taskMatter.Matter.Id.Value, creator);

            return DocumentTask.Create(new Common.Models.Documents.DocumentTask()
            {
                Id = Guid.NewGuid(),
                Document = model,
                Task = new Common.Models.Tasks.Task { Id = taskId },
                CreatedBy = creator,
                ModifiedBy = creator,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            }, creator);
        }

        public static Common.Models.Documents.Document Edit(Common.Models.Documents.Document model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Documents.Document dbo = Mapper.Map<DBOs.Documents.Document>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"document\" SET " +
                    "\"date\"=@Date, \"title\"=@Title, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Documents.Version GetCurrentVersion(Guid documentId)
        {
            return DataHelper.Get<Common.Models.Documents.Version, DBOs.Documents.Version>(
                "SELECT * FROM \"version\" WHERE \"document_id\"=@DocumentId AND \"utc_disabled\" is null ORDER BY \"version_number\" DESC LIMIT 1",
                new { DocumentId = documentId });
        }

        public static List<Common.Models.Documents.Version> GetVersions(Guid documentId)
        {
            return DataHelper.List<Common.Models.Documents.Version, DBOs.Documents.Version>(
                "SELECT * FROM \"version\" WHERE \"document_id\"=@DocumentId AND \"utc_disabled\" is null ORDER BY \"version_number\" DESC",
                new { DocumentId = documentId });
        }

        public static Common.Models.Documents.Version CreateNewVersion(Guid documentId,
            Common.Models.Documents.Version model, Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;

            Common.Models.Documents.Version currentVersion = GetCurrentVersion(documentId);
            if (currentVersion == null)
                model.VersionNumber = 1;
            else
                model.VersionNumber = currentVersion.VersionNumber + 1;

            DBOs.Documents.Version dbo = Mapper.Map<DBOs.Documents.Version>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"version\" (\"id\", \"document_id\", \"version_number\", \"mime\", \"filename\", " +
                    "\"extension\", \"size\", \"md5\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @DocumentId, @VersionNumber, @Mime, @Filename, @Extension, @Size, @Md5, " +
                    "@UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            return model;
        }
    }
}