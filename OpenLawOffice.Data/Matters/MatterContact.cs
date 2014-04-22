// -----------------------------------------------------------------------
// <copyright file="MatterContact.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Matters
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
    public static class MatterContact
    {
        public static Common.Models.Matters.MatterContact Get(int id)
        {
            return DataHelper.Get<Common.Models.Matters.MatterContact, DBOs.Matters.MatterContact>(
                "SELECT * FROM \"matter_contact\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Matters.MatterContact Get(Guid matterId, int contactId)
        {
            return DataHelper.Get<Common.Models.Matters.MatterContact, DBOs.Matters.MatterContact>(
                "SELECT * FROM \"matter_contact\" WHERE \"matter_id\"=@MatterId AND \"contact_id\"=@ContactId AND \"utc_disabled\" is null",
                new { MatterId = matterId, ContactId = contactId });
        }

        public static List<Common.Models.Matters.MatterContact> ListForMatter(Guid matterId)
        {
            List<Common.Models.Matters.MatterContact> list =
                DataHelper.List<Common.Models.Matters.MatterContact, DBOs.Matters.MatterContact>(
                "SELECT * FROM \"matter_contact\" WHERE \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                new { MatterId = matterId });

            list.ForEach(x =>
            {
                x.Contact = Contacts.Contact.Get(x.Contact.Id.Value);
            });

            return list;
        }

        public static Common.Models.Matters.MatterContact Create(Common.Models.Matters.MatterContact model,
            Common.Models.Security.User creator)
        {
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Matters.MatterContact dbo = Mapper.Map<DBOs.Matters.MatterContact>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"matter_contact\" (\"matter_id\", \"contact_id\", \"role\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@MatterId, @ContactId, @Role, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Matters.MatterContact>("SELECT currval(pg_get_serial_sequence('matter_contact', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Matters.MatterContact Edit(Common.Models.Matters.MatterContact model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.UtcModified = DateTime.UtcNow;
            DBOs.Matters.MatterContact dbo = Mapper.Map<DBOs.Matters.MatterContact>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"matter_contact\" SET " +
                    "\"matter_id\"=@MatterId, \"contact_id\"=@ContactId, \"role\"=@Role, \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Matters.MatterContact Disable(Common.Models.Matters.MatterContact model,
            Common.Models.Security.User disabler)
        {
            model.DisabledBy = disabler;
            model.UtcDisabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("matter_contact", disabler.Id.Value, model.Id);

            return model;
        }

        public static Common.Models.Matters.MatterContact Enable(Common.Models.Matters.MatterContact model,
            Common.Models.Security.User enabler)
        {
            model.ModifiedBy = enabler;
            model.UtcModified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.UtcDisabled = null;

            DataHelper.Enable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("matter_contact", enabler.Id.Value, model.Id);

            return model;
        }
    }
}