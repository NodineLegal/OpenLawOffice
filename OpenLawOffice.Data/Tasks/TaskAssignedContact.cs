// -----------------------------------------------------------------------
// <copyright file="TaskAssignedContact.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Tasks
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
    public static class TaskAssignedContact
    {
        public static Common.Models.Tasks.TaskAssignedContact Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskAssignedContact, DBOs.Tasks.TaskAssignedContact>(
                "SELECT * FROM \"task_assigned_contact\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Tasks.TaskAssignedContact Get(long taskId, int contactId)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskAssignedContact, DBOs.Tasks.TaskAssignedContact>(
                "SELECT * FROM \"task_assigned_contact\" WHERE \"task_id\"=@TaskId AND \"contact_id\"=@ContactId AND \"utc_disabled\" is null",
                new { TaskId = taskId, ContactId = contactId });
        }

        public static List<Common.Models.Tasks.TaskAssignedContact> ListForTask(long taskId)
        {
            return DataHelper.List<Common.Models.Tasks.TaskAssignedContact, DBOs.Tasks.TaskAssignedContact>(
                "SELECT * FROM \"task_assigned_contact\" WHERE \"task_id\"=@TaskId AND \"utc_disabled\" is null",
                new { TaskId = taskId });
        }

        public static Common.Models.Tasks.TaskAssignedContact Create(Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Security.User creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Tasks.TaskAssignedContact dbo = Mapper.Map<DBOs.Tasks.TaskAssignedContact>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"task_assigned_contact\" (\"id\", \"task_id\", \"contact_id\", \"assignment_type\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @TaskId, @ContactId, @AssignmentType, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Tasks.TaskAssignedContact>("SELECT currval(pg_get_serial_sequence('task_assigned_contact', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Tasks.TaskAssignedContact Edit(Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.UtcModified = DateTime.UtcNow;
            DBOs.Tasks.TaskAssignedContact dbo = Mapper.Map<DBOs.Tasks.TaskAssignedContact>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"task_assigned_contact\" SET " +
                    "\"task_id\"=@TaskId, \"contact_id\"=@ContactId, \"assignment_type\"=@AssignmentType, \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Tasks.TaskAssignedContact Disable(Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Security.User disabler)
        {
            model.DisabledBy = disabler;
            model.UtcDisabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Tasks.TaskAssignedContact,
                DBOs.Tasks.TaskAssignedContact>("task_assigned_contact", disabler.Id.Value, model.Id);

            return model;
        }

        public static Common.Models.Tasks.TaskAssignedContact Enable(Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Security.User enabler)
        {
            model.ModifiedBy = enabler;
            model.UtcModified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.UtcDisabled = null;

            DataHelper.Enable<Common.Models.Tasks.TaskAssignedContact,
                DBOs.Tasks.TaskAssignedContact>("task_assigned_contact", enabler.Id.Value, model.Id);

            return model;
        }
    }
}
