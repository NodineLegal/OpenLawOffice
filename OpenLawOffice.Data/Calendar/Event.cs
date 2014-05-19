// -----------------------------------------------------------------------
// <copyright file="Task.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Calendar
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
        public static Common.Models.Calendar.Event Get(long id)
        {
            return DataHelper.Get<Common.Models.Calendar.Event, DBOs.Calendar.Event>(
                "SELECT * FROM \"event\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Calendar.Event> List(double start, double? stop)
        {
            if (stop.HasValue)
                return List(Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value));
            else
                return List(Common.Utilities.UnixTimeStampToDateTime(start), null);
        }

        public static List<Common.Models.Calendar.Event> ListForUser(int userId, double start, double? stop)
        {
            if (stop.HasValue)
                return ListForUser(userId, Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value));
            else
                return ListForUser(userId, Common.Utilities.UnixTimeStampToDateTime(start), null);
        }

        public static List<Common.Models.Calendar.Event> ListForContact(int contactId, double start, double? stop)
        {
            if (stop.HasValue)
                return ListForContact(contactId, Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value));
            else
                return ListForContact(contactId, Common.Utilities.UnixTimeStampToDateTime(start), null);
        }

        public static List<Common.Models.Calendar.Event> List(DateTime start, DateTime? stop)
        {
            List<Common.Models.Calendar.Event> events;

            if (stop.HasValue)
                events = DataHelper.List<Common.Models.Calendar.Event, DBOs.Calendar.Event>(
                    "SELECT * FROM \"event\" WHERE \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop",
                    new { Start = start, Stop = stop });
            else
                events = DataHelper.List<Common.Models.Calendar.Event, DBOs.Calendar.Event>(
                    "SELECT * FROM \"event\" WHERE \"utc_disabled\" is null AND \"start\">=@Start",
                    new { Start = start });

            return events;
        }

        public static List<Common.Models.Calendar.Event> ListForUser(int userId, DateTime start, DateTime? stop)
        {
            List<Common.Models.Calendar.Event> events;

            if (stop.HasValue)
                events = DataHelper.List<Common.Models.Calendar.Event, DBOs.Calendar.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_responsible_user\" WHERE \"user_id\"=@UserId) " +
                    "AND \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop",
                    new { UserId = userId, Start = start, Stop = stop });
            else
                events = DataHelper.List<Common.Models.Calendar.Event, DBOs.Calendar.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_responsible_user\" WHERE \"user_id\"=@UserId) " +
                    "AND \"utc_disabled\" is null AND \"start\">=@Start",
                    new { UserId = userId, Start = start });

            return events;
        }

        public static List<Common.Models.Calendar.Event> ListForContact(int contactId, DateTime start, DateTime? stop)
        {
            List<Common.Models.Calendar.Event> events;

            if (stop.HasValue)
                events = DataHelper.List<Common.Models.Calendar.Event, DBOs.Calendar.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_assigned_contact\" WHERE \"contact_id\"=@ContactId) " +
                    "AND \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop",
                    new { ContactId = contactId, Start = start, Stop = stop });
            else
                events = DataHelper.List<Common.Models.Calendar.Event, DBOs.Calendar.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_assigned_contact\" WHERE \"contact_id\"=@ContactId) " +
                    "AND \"utc_disabled\" is null AND \"start\">=@Start",
                    new { ContactId = contactId, Start = start });

            return events;
        }
    }
}
