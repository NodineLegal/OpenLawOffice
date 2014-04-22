// -----------------------------------------------------------------------
// <copyright file="DocumentMatter.cs" company="Nodine Legal, LLC">
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
    public static class DocumentMatter
    {
        public static Common.Models.Documents.DocumentMatter Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Documents.DocumentMatter, DBOs.Documents.DocumentMatter>(
                "SELECT * FROM \"document_matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                new { Id = id });
        }

        public static Common.Models.Documents.DocumentMatter Get(Guid matterId, Guid documentId)
        {
            return DataHelper.Get<Common.Models.Documents.DocumentMatter, DBOs.Documents.DocumentMatter>(
                "SELECT * FROM \"document_matter\" WHERE \"matter_id\"=@MatterId AND \"document_id\"=@DocumentId AND \"utc_disabled\" is null",
                new { MatterId = matterId, DocumentId = documentId });
        }

        public static Common.Models.Documents.DocumentMatter GetIgnoringDisable(Guid matterId, Guid documentId)
        {
            return DataHelper.Get<Common.Models.Documents.DocumentMatter, DBOs.Documents.DocumentMatter>(
                "SELECT * FROM \"document_matter\" WHERE \"matter_id\"=@MatterId AND \"document_id\"=@DocumentId",
                new { MatterId = matterId, DocumentId = documentId });
        }

        public static Common.Models.Matters.Matter GetRelatedMatter(Guid documentId)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT \"matter\".* FROM \"document_matter\" JOIN \"matter\" ON \"document_matter\".\"matter_id\"=\"matter\".\"id\" " +
                "WHERE \"document_matter\".\"document_id\"=@DocumentId " +
                "AND \"document_matter\".\"utc_disabled\" is null " +
                "AND \"matter\".\"utc_disabled\" is null ",
                new { DocumentId = documentId });
        }

        public static Common.Models.Documents.DocumentMatter Create(Common.Models.Documents.DocumentMatter model,
            Common.Models.Security.User creator)
        {
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Documents.DocumentMatter dbo = Mapper.Map<DBOs.Documents.DocumentMatter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                Common.Models.Documents.DocumentMatter currentModel = Get(model.Matter.Id.Value, model.Document.Id.Value);

                if (currentModel != null)
                { // Update
                    conn.Execute("UPDATE \"document_matter\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                        "\"utc_disabled\"=null, \"disabled_by_user_id\"=null WHERE \"id\"=@Id", dbo);
                    model.UtcCreated = currentModel.UtcCreated;
                    model.CreatedBy = currentModel.CreatedBy;
                }
                else
                { // Create
                    conn.Execute("INSERT INTO \"document_matter\" (\"id\", \"document_id\", \"matter_id\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                        "VALUES (@Id, @DocumentId, @MatterId, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                        dbo);
                }
            }

            return model;
        }
    }
}