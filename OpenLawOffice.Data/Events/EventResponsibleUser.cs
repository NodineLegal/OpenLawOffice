// -----------------------------------------------------------------------
// <copyright file="EventResponsibleUser.cs" company="Nodine Legal, LLC">
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
    public static class EventResponsibleUser
    {
        public static Common.Models.Events.EventResponsibleUser Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Events.EventResponsibleUser, DBOs.Events.EventResponsibleUser>(
                "SELECT * FROM \"event_responsible_user\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Events.EventResponsibleUser Get(Guid eventId, int userId)
        {
            return DataHelper.Get<Common.Models.Events.EventResponsibleUser, DBOs.Events.EventResponsibleUser>(
                "SELECT * FROM \"event_responsible_user\" WHERE \"event_id\"=@EventId AND \"user_id\"=@UserId AND \"utc_disabled\" is null",
                new { EventId = eventId, UserId = userId });
        }

        public static Common.Models.Events.EventResponsibleUser GetIgnoringDisable(Guid eventId, int userId)
        {
            return DataHelper.Get<Common.Models.Events.EventResponsibleUser, DBOs.Events.EventResponsibleUser>(
                "SELECT * FROM \"event_responsible_user\" WHERE \"event_id\"=@EventId AND \"user_id\"=@UserId",
                new { EventId = eventId, UserId = userId });
        }

        public static List<Common.Models.Events.EventResponsibleUser> ListForEvent(Guid eventId)
        {
            List<Common.Models.Events.EventResponsibleUser> list =
                DataHelper.List<Common.Models.Events.EventResponsibleUser, DBOs.Events.EventResponsibleUser>(
                "SELECT * FROM \"event_responsible_user\" WHERE \"event_id\"=@EventId AND \"utc_disabled\" is null",
                new { EventId = eventId });

            list.ForEach(x =>
            {
                x.User = Security.User.Get(x.User.Id.Value);
            });

            return list;
        }

        public static Common.Models.Events.EventResponsibleUser Create(Common.Models.Events.EventResponsibleUser model,
            Common.Models.Security.User creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Events.EventResponsibleUser dbo = Mapper.Map<DBOs.Events.EventResponsibleUser>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"event_responsible_user\" (\"event_id\", \"user_id\", \"responsibility\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@EventId, @UserId, @Responsibility, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Events.EventResponsibleUser Edit(Common.Models.Events.EventResponsibleUser model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            Common.Models.Events.EventResponsibleUser currentModel = Data.Events.EventResponsibleUser.Get(model.Id.Value);
            model.Event = currentModel.Event;
            DBOs.Tasks.TaskResponsibleUser dbo = Mapper.Map<DBOs.Tasks.TaskResponsibleUser>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"event_responsible_user\" SET " +
                    "\"event_id\"=@EventId, \"user_id\"=@UserId, \"responsibility\"=@Responsibility, \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Events.EventResponsibleUser Disable(Common.Models.Events.EventResponsibleUser model,
            Common.Models.Security.User disabler)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Events.EventResponsibleUser,
                DBOs.Events.EventResponsibleUser>("event_responsible_user", disabler.Id.Value, model.Id);

            return model;
        }

        public static Common.Models.Events.EventResponsibleUser Enable(Common.Models.Events.EventResponsibleUser model,
            Common.Models.Security.User enabler)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Events.EventResponsibleUser,
                DBOs.Events.EventResponsibleUser>("event_responsible_user", enabler.Id.Value, model.Id);

            return model;
        }
    }
}
