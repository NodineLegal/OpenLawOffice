// -----------------------------------------------------------------------
// <copyright file="Event.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Event
    {
        public static Common.Models.Events.Event Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Events.Event, DBOs.Events.Event>(
                "SELECT * FROM \"event\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Events.Event> List()
        {
            return DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                "SELECT * FROM \"event\" WHERE \"utc_disabled\" is null");
        }

        public static List<Common.Models.Events.Event> List(double start, double? stop)
        {
            if (stop.HasValue)
                return List(Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value));
            else
                return List(Common.Utilities.UnixTimeStampToDateTime(start), null);
        }

        public static List<Common.Models.Events.Event> ListForUser(Guid userId, double start, double? stop)
        {
            if (stop.HasValue)
                return ListForUser(userId, Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value));
            else
                return ListForUser(userId, Common.Utilities.UnixTimeStampToDateTime(start), null);
        }

        public static List<Common.Models.Events.Event> ListForUserAndContact(Guid userId, int contactId, double start, double? stop)
        {
            if (stop.HasValue)
                return ListForUserAndContact(userId, contactId, Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value));
            else
                return ListForUserAndContact(userId, contactId, Common.Utilities.UnixTimeStampToDateTime(start), null);
        }

        public static List<Common.Models.Events.Event> ListForContact(int contactId, double start, double? stop)
        {
            if (stop.HasValue)
                return ListForContact(contactId, Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value));
            else
                return ListForContact(contactId, Common.Utilities.UnixTimeStampToDateTime(start), null);
        }

        public static List<Common.Models.Events.Event> List(DateTime start, DateTime? stop)
        {
            List<Common.Models.Events.Event> events;

            if (stop.HasValue)
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop",
                    new { Start = start, Stop = stop });
            else
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"utc_disabled\" is null AND \"start\">=@Start",
                    new { Start = start });

            return events;
        }

        public static List<Common.Models.Events.Event> ListForUser(Guid userId, DateTime start, DateTime? stop)
        {
            List<Common.Models.Events.Event> events;

            if (stop.HasValue)
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_responsible_user\" WHERE \"user_pid\"=@UserPId) " +
                    "AND \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop ORDER BY \"start\" ASC",
                    new { UserPId = userId, Start = start, Stop = stop });
            else
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_responsible_user\" WHERE \"user_pid\"=@UserPId) " +
                    "AND \"utc_disabled\" is null AND \"start\">=@Start ORDER BY \"start\" ASC",
                    new { UserPId = userId, Start = start });

            return events;
        }

        public static List<Common.Models.Events.Event> ListForUserAndContact(Guid userId, int contactId, DateTime start, DateTime? stop)
        {
            List<Common.Models.Events.Event> events;

            if (stop.HasValue)
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT DISTINCT * FROM \"event\" WHERE (\"id\" IN (SELECT \"event_id\" FROM \"event_responsible_user\" WHERE \"user_pid\"=@UserPId) " +
                    "OR \"id\" IN (SELECT \"event_id\" FROM \"event_assigned_contact\" WHERE \"contact_id\"=@ContactId)) " +
                    "AND \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop ORDER BY \"start\" ASC",
                    new { UserPId = userId, ContactId = contactId, Start = start, Stop = stop });
            else
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT DISTINCT * FROM \"event\" WHERE (\"id\" IN (SELECT \"event_id\" FROM \"event_responsible_user\" WHERE \"user_pid\"=@UserPId) " +
                    "OR \"id\" IN (SELECT \"event_id\" FROM \"event_assigned_contact\" WHERE \"contact_id\"=@ContactId)) " +
                    "AND \"utc_disabled\" is null AND \"start\">=@Start ORDER BY \"start\" ASC",
                    new { UserPId = userId, ContactId = contactId, Start = start });

            return events;
        }

        public static List<Common.Models.Events.Event> ListForContact(int contactId, DateTime start, DateTime? stop)
        {
            List<Common.Models.Events.Event> events;

            start = start.ToDbTime();

            if (stop.HasValue)
            {
                stop = stop.Value.ToDbTime();
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_assigned_contact\" WHERE \"contact_id\"=@ContactId) " +
                    "AND \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop ORDER BY \"start\" ASC",
                    new { ContactId = contactId, Start = start, Stop = stop });
            }
            else
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_assigned_contact\" WHERE \"contact_id\"=@ContactId) " +
                    "AND \"utc_disabled\" is null AND \"start\">=@Start ORDER BY \"start\" ASC",
                    new { ContactId = contactId, Start = start });

            return events;
        }

        public static List<Common.Models.Matters.Matter> ListMattersFor(Guid eventId)
        {
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"id\" IN (SELECT \"matter_id\" FROM \"event_matter\" WHERE \"event_id\"=@EventId) " +
                "AND \"utc_disabled\" is null",
                new { EventId = eventId });
        }

        public static List<Common.Models.Events.Event> ListForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                "SELECT * FROM \"event\" WHERE \"id\" in (SELECT \"event_id\" FROM \"event_matter\" WHERE \"matter_id\"=@MatterId) " +
                "AND \"utc_disabled\" is null",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Notes.Note> ListNotesFor(Guid eventId)
        {
            return DataHelper.List<Common.Models.Notes.Note, DBOs.Notes.Note>(
                "SELECT * FROM \"notes\" WHERE \"id\" IN (SELECT \"note_id\" FROM \"event_note\" WHERE \"event_id\"=@EventId) " +
                "AND \"utc_disabled\" is null",
                new { EventId = eventId });
        }

        public static List<Common.Models.Matters.Matter> ListTasksFor(Guid eventId)
        {
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"task\" WHERE \"id\" IN (SELECT \"task_id\" FROM \"event_task\" WHERE \"event_id\"=@EventId) " +
                "AND \"utc_disabled\" is null",
                new { EventId = eventId });
        }

        public static List<Common.Models.Events.Event> ListForTask(long taskId)
        {
            return DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                "SELECT * FROM \"event\" WHERE \"id\" in (SELECT \"event_id\" FROM \"event_task\" WHERE \"task_id\"=@TaskId) " +
                "AND \"utc_disabled\" is null",
                new { TaskId = taskId });
        }

        public static Common.Models.Events.Event Create(Common.Models.Events.Event model,
            Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Events.Event dbo = null;

            dbo = Mapper.Map<DBOs.Events.Event>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"event\" (\"id\", \"title\", \"allday\", \"start\", \"end\", \"location\", \"description\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @Title, @AllDay, @Start, @End, @Location, @Description, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Events.Event Edit(Common.Models.Events.Event model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Events.Event dbo = Mapper.Map<DBOs.Events.Event>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"event\" SET " +
                    "\"title\"=@Title, \"allday\"=@AllDay, \"start\"=@Start, " +
                    "\"end\"=@End, \"location\"=@Location, \"description\"=@Description, " +
                    "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }
    }
}
