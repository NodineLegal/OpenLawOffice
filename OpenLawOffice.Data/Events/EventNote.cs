// -----------------------------------------------------------------------
// <copyright file="EventNote.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Events
{
    using System;
    using System.Data;
    using AutoMapper;
    using Dapper;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class EventNote
    {
        public static Common.Models.Events.EventNote Get(Guid eventId, Guid noteId)
        {
            return DataHelper.Get<Common.Models.Events.EventNote, DBOs.Events.EventNote>(
                "SELECT * FROM \"event_note\" WHERE \"event_id\"=@EventId AND \"note_id\"=@NoteId AND \"utc_disabled\" is null",
                new { EventId = eventId, NoteId = noteId });
        }

        public static Common.Models.Events.EventNote GetIgnoringDisable(Guid eventId, Guid noteId)
        {
            return DataHelper.Get<Common.Models.Events.EventNote, DBOs.Events.EventNote>(
                "SELECT * FROM \"event_note\" WHERE \"event_id\"=@EventId AND \"note_id\"=@NoteId",
                new { EventId = eventId, NoteId = noteId });
        }

        public static Common.Models.Events.EventNote GetRelatedTask(Guid noteId)
        {
            return DataHelper.Get<Common.Models.Events.EventNote, DBOs.Events.EventNote>(
                "SELECT \"event\".* FROM \"event_note\" JOIN \"event\" ON \"event_note\".\"event_id\"=\"event\".\"id\" " +
                "WHERE \"event_note\".\"note_id\"=@NoteId " +
                "AND \"event_note\".\"utc_disabled\" is null " +
                "AND \"event\".\"utc_disabled\" is null ",
                new { NoteId = noteId });
        }

        public static List<Common.Models.Notes.Note> ListForEvent(Guid eventId)
        {
            List<Common.Models.Notes.Note> list =
                DataHelper.List<Common.Models.Notes.Note, DBOs.Notes.Note>(
                "SELECT * FROM \"note\" WHERE \"id\" IN (SELECT \"note_id\" FROM \"event_note\" WHERE \"event_id\"=@EventId AND \"utc_disabled\" is null)",
                new { EventId = eventId });

            return list;
        }

        public static Common.Models.Events.EventNote Create(Common.Models.Events.EventNote model,
            Common.Models.Account.Users creator)
        {
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Events.EventNote dbo = Mapper.Map<DBOs.Events.EventNote>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                Common.Models.Events.EventNote currentModel = Get(model.Event.Id.Value, model.Note.Id.Value);

                if (currentModel != null)
                { // Update
                    conn.Execute("UPDATE \"event_note\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserId " +
                        "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null WHERE \"id\"=@Id", dbo);
                    model.Created = currentModel.Created;
                    model.CreatedBy = currentModel.CreatedBy;
                }
                else
                { // Create
                    conn.Execute("INSERT INTO \"event_note\" (\"id\", \"note_id\", \"event_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                        "VALUES (@Id, @NoteId, @EventId, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                        dbo);
                }
            }

            return model;
        }
    }
}
