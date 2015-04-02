// -----------------------------------------------------------------------
// <copyright file="FormField.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    public class FormField
    {
        public static Common.Models.Forms.FormField Get(int id)
        {
            return DataHelper.Get<Common.Models.Forms.FormField, DBOs.Forms.FormField>(
                "SELECT * FROM \"form_field\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Forms.FormField> List()
        {
            return DataHelper.List<Common.Models.Forms.FormField, DBOs.Forms.FormField>(
                "SELECT * FROM \"form_field\" WHERE \"utc_disabled\" is null");
        }

        public static Common.Models.Forms.FormField Create(Common.Models.Forms.FormField model, 
            Common.Models.Account.Users creator)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Forms.FormField dbo = Mapper.Map<DBOs.Forms.FormField>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"form_field\" (\"title\", \"description\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Title, @Description, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
                model.Id = conn.Query<DBOs.Forms.FormField>("SELECT currval(pg_get_serial_sequence('form_field', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Forms.FormField Edit(Common.Models.Forms.FormField model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Forms.FormField dbo = Mapper.Map<DBOs.Forms.FormField>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"form_field\" SET " +
                    "\"title\"=@Title, \"description\"=@Description, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Forms.FormField Disable(Common.Models.Forms.FormField model,
            Common.Models.Account.Users disabler)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Forms.FormField,
                DBOs.Forms.FormField>("form_field", disabler.PId.Value, model.Id);

            return model;
        }
    }
}
