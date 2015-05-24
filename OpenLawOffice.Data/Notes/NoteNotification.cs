// -----------------------------------------------------------------------
// <copyright file="NoteNotification.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NoteNotification
    {
        public static Common.Models.Notes.NoteNotification Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Notes.NoteNotification, DBOs.Notes.NoteNotification>(
                "SELECT * FROM \"note_notification\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                new { Id = id });
        }

        public static Common.Models.Notes.NoteNotification Get(Guid noteId, int contactId)
        {
            return DataHelper.Get<Common.Models.Notes.NoteNotification, DBOs.Notes.NoteNotification>(
                "SELECT * FROM \"note_notification\" WHERE \"note_id\"=@NoteId AND \"contact_id\"=@ContactId AND \"utc_disabled\" is null",
                new { NoteId = noteId, ContactId = contactId });
        }

        public static List<Common.Models.Notes.NoteNotification> ListForNote(Guid noteId)
        {
            return DataHelper.List<Common.Models.Notes.NoteNotification, DBOs.Notes.NoteNotification>(
                "SELECT * FROM \"note_notification\" WHERE \"note_id\"=@NoteId AND " +
                "\"utc_disabled\" is null ORDER BY \"cleared\" DESC",
                new { NoteId = noteId });
        }

        public static List<Common.Models.Notes.NoteNotification> ListAllNoteNotificationsForContact(int contactId)
        {
            return DataHelper.List<Common.Models.Notes.NoteNotification, DBOs.Notes.NoteNotification>(
                "SELECT * FROM \"note_notification\" WHERE \"contact_id\"=@ContactId " +
                "AND \"cleared\" is null AND \"utc_disabled\" is null",
                new { ContactId = contactId });
        }

        public static Common.Models.Notes.NoteNotification Create(Common.Models.Notes.NoteNotification model,
            Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Notes.NoteNotification dbo = Mapper.Map<DBOs.Notes.NoteNotification>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                Common.Models.Notes.NoteNotification currentModel = Get(model.Note.Id.Value, model.Contact.Id.Value);
                dbo = Mapper.Map<DBOs.Notes.NoteNotification>(currentModel);

                if (currentModel != null)
                { // Update
                    conn.Execute("UPDATE \"note_notification\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId, " +
                        "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null, \"cleared\"=null WHERE \"id\"=@Id", dbo);
                    model.Created = currentModel.Created;
                    model.CreatedBy = currentModel.CreatedBy;
                }
                else
                { // Create
                    conn.Execute("INSERT INTO \"note_notification\" (\"id\", \"note_id\", \"contact_id\", \"cleared\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                        "VALUES (@Id, @NoteId, @ContactId, @Cleared, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                        dbo);
                }
            }

            return model;
        }

        public static Common.Models.Notes.NoteNotification Clear(Common.Models.Notes.NoteNotification model,
            Common.Models.Account.Users modifier)
        {
            model.Cleared = DateTime.Now;
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Notes.NoteNotification dbo = Mapper.Map<DBOs.Notes.NoteNotification>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"note_notification\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId, " +
                    "\"cleared\"=@Cleared WHERE \"id\"=@Id", dbo);
            }

            return model;
        }
    }
}
