// -----------------------------------------------------------------------
// <copyright file="FormFieldMatter.cs" company="Nodine Legal, LLC">
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

    public static class FormFieldMatter
    {
        public static Common.Models.Forms.FormFieldMatter Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Forms.FormFieldMatter, DBOs.Forms.FormFieldMatter>(
                "SELECT * FROM \"form_field_matter\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Forms.FormFieldMatter Get(Guid matterId, int formFieldId)
        {
            return DataHelper.Get<Common.Models.Forms.FormFieldMatter, DBOs.Forms.FormFieldMatter>(
                "SELECT * FROM \"form_field_matter\" WHERE \"matter_id\"=@MatterId AND \"form_field_id\"=@FormFieldId AND \"utc_disabled\" is null",
                new { MatterId = matterId, FormFieldId = formFieldId });
        }

        public static List<Common.Models.Forms.FormFieldMatter> ListForMatter(Guid matterId)
        {
            List<Common.Models.Forms.FormFieldMatter> list;
            List<Common.Models.Forms.FormField> fieldList;

            fieldList = Data.Forms.FormField.List();
            list = new List<Common.Models.Forms.FormFieldMatter>();

            fieldList.ForEach(x =>
            {
                Common.Models.Forms.FormFieldMatter ffm;

                ffm = Data.Forms.FormFieldMatter.Get(matterId, x.Id.Value);

                if (ffm == null)
                    ffm = new Common.Models.Forms.FormFieldMatter() 
                    { 
                        FormField = x
                    };
                else
                    ffm.FormField = x;

                list.Add(ffm);
            });

            return list;
        }

        public static Common.Models.Forms.FormFieldMatter Create(Common.Models.Forms.FormFieldMatter model,
            Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Forms.FormFieldMatter dbo = Mapper.Map<DBOs.Forms.FormFieldMatter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"form_field_matter\" (\"id\", \"matter_id\", \"form_field_id\", \"value\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @MatterId, @FormFieldId, @Value, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Forms.FormFieldMatter Edit(Common.Models.Forms.FormFieldMatter model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Forms.FormFieldMatter dbo = Mapper.Map<DBOs.Forms.FormFieldMatter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"form_field_matter\" SET " +
                    "\"matter_id\"=@MatterId, \"form_field_id\"=@FormFieldId, \"value\"=@Value, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }
    }
}
