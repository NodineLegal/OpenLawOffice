// -----------------------------------------------------------------------
// <copyright file="TagCategory.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Tagging
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
    public static class TagCategory
    {
        public static Common.Models.Tagging.TagCategory Get(int id)
        {
            return DataHelper.Get<Common.Models.Tagging.TagCategory, DBOs.Tagging.TagCategory>(
                "SELECT * FROM \"tag_category\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Tagging.TagCategory Get(string name)
        {
            return DataHelper.Get<Common.Models.Tagging.TagCategory, DBOs.Tagging.TagCategory>(
                "SELECT * FROM \"tag_category\" WHERE \"name\"=@name AND \"utc_disabled\" is null",
                new { name = name });
        }

        public static List<Common.Models.Tagging.TagCategory> List(string name)
        {
            return DataHelper.List<Common.Models.Tagging.TagCategory, DBOs.Tagging.TagCategory>(
                "SELECT * FROM \"tag_category\" WHERE LOWER(\"name\") LIKE '%' || @Query || '%' AND \"utc_disabled\" is null",
                new { Query = name.ToLower() });
        }

        public static Common.Models.Tagging.TagCategory Create(Common.Models.Tagging.TagCategory model,
            Common.Models.Security.User creator)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Tagging.TagCategory dbo = Mapper.Map<DBOs.Tagging.TagCategory>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"tag_category\" (\"name\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Name, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Tagging.TagCategory>("SELECT currval(pg_get_serial_sequence('tag_category', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Tagging.TagCategory Disable(Common.Models.Tagging.TagCategory model,
            Common.Models.Security.User disabler)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Tagging.TagCategory,
                DBOs.Tagging.TagCategory>("tag_category", disabler.Id.Value, model.Id);

            return model;
        }

        public static Common.Models.Tagging.TagCategory Enable(Common.Models.Tagging.TagCategory model,
            Common.Models.Security.User enabler)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Tagging.TagCategory,
                DBOs.Tagging.TagCategory>("tag_category", enabler.Id.Value, model.Id);

            return model;
        }
    }
}