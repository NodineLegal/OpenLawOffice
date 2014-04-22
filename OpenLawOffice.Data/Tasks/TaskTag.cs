// -----------------------------------------------------------------------
// <copyright file="TaskTag.cs" company="Nodine Legal, LLC">
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
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TaskTag
    {
        public static Common.Models.Tasks.TaskTag Get(Guid id)
        {
            Common.Models.Tasks.TaskTag model =
                DataHelper.Get<Common.Models.Tasks.TaskTag, DBOs.Tasks.TaskTag>(
                "SELECT * FROM \"task_tag\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });

            if (model == null) return null;

            if (model.TagCategory != null)
                model.TagCategory = Tagging.TagCategory.Get(model.TagCategory.Id);

            return model;
        }

        public static List<Common.Models.Tasks.TaskTag> List()
        {
            return DataHelper.List<Common.Models.Tasks.TaskTag, DBOs.Tasks.TaskTag>(
                "SELECT * FROM \"task_tag\" WHERE \"utc_disabled\" is null");
        }

        public static List<Common.Models.Tasks.TaskTag> ListForTask(long taskId)
        {
            List<Common.Models.Tasks.TaskTag> list =
                DataHelper.List<Common.Models.Tasks.TaskTag, DBOs.Tasks.TaskTag>(
                "SELECT * FROM \"task_tag\" WHERE \"task_id\"=@TaskId AND \"utc_disabled\" is null",
                new { TaskId = taskId });

            list.ForEach(x =>
            {
                x.TagCategory = Tagging.TagCategory.Get(x.TagCategory.Id);
            });

            return list;
        }

        public static Common.Models.Tasks.TaskTag Create(Common.Models.Tasks.TaskTag model,
            Common.Models.Security.User creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;

            Common.Models.Tagging.TagCategory existingTagCat = Tagging.TagCategory.Get(model.TagCategory.Name);

            if (existingTagCat == null)
            {
                existingTagCat = Tagging.TagCategory.Create(model.TagCategory, creator);
            }

            model.TagCategory = existingTagCat;
            DBOs.Tasks.TaskTag dbo = Mapper.Map<DBOs.Tasks.TaskTag>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"task_tag\" (\"id\", \"task_id\", \"tag_category_id\", \"tag\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @TaskId, @TagCategoryId, @Tag, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Tasks.TaskTag Edit(Common.Models.Tasks.TaskTag model,
            Common.Models.Security.User modifier)
        {
            model.ModifiedBy = modifier;
            model.UtcModified = DateTime.UtcNow;
            DBOs.Tasks.TaskTag dbo = Mapper.Map<DBOs.Tasks.TaskTag>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"task_tag\" SET " +
                    "\"task_id\"=@TaskId, \"tag\"=@Tag, \"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            model.TagCategory = UpdateTagCategory(model, modifier);

            return model;
        }

        private static Common.Models.Tagging.TagCategory UpdateTagCategory(
            Common.Models.Tasks.TaskTag model,
            Common.Models.Security.User modifier)
        {
            Common.Models.Tasks.TaskTag currentTag = Get(model.Id.Value);

            if (currentTag.TagCategory != null)
            {
                if (model.TagCategory != null && !string.IsNullOrEmpty(model.TagCategory.Name))
                { // If current has tag & new has tag
                    // Are they the same - ignore if so
                    if (currentTag.TagCategory.Name != model.TagCategory.Name)
                    {
                        // Update - change tagcat
                        model.TagCategory = AddOrChangeTagCategory(model, modifier);
                    }
                }
                else
                {
                    // If current has tag & new !has tag
                    // Update - drop tagcat
                    currentTag.TagCategory = null;

                    using (IDbConnection conn = Database.Instance.GetConnection())
                    {
                        conn.Execute("UPDATE \"task_tag\" SET \"tag_category_id\"=null WHERE \"id\"=@Id",
                            new { Id = model.Id.Value });
                    }
                }
            }
            else
            {
                if (model.TagCategory != null && !string.IsNullOrEmpty(model.TagCategory.Name))
                { // If current !has tag & new has tag
                    // Update - add tagcat
                    model.TagCategory = AddOrChangeTagCategory(model, modifier);
                }

                // If current !has tag & new !has tag - do nothing
            }

            return model.TagCategory;
        }

        private static Common.Models.Tagging.TagCategory AddOrChangeTagCategory(
            Common.Models.Tasks.TaskTag tag,
            Common.Models.Security.User modifier)
        {
            Common.Models.Tagging.TagCategory newTagCat = null;

            // Check for existing name
            if (tag.TagCategory != null && !string.IsNullOrEmpty(tag.TagCategory.Name))
            {
                newTagCat = Tagging.TagCategory.Get(tag.TagCategory.Name);
            }

            // Either need to use existing or create a new tag category
            if (newTagCat != null)
            {
                // Can use existing
                tag.TagCategory = newTagCat;

                // If new tagcat was disabled, it needs enabled
                if (newTagCat.UtcDisabled.HasValue)
                {
                    tag.TagCategory = Tagging.TagCategory.Enable(tag.TagCategory, modifier);
                }
            }
            else
            {
                // Add one
                tag.TagCategory = Tagging.TagCategory.Create(tag.TagCategory, modifier);
            }

            // Update MatterTag's TagCategoryId
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"task_tag\" SET \"tag_category_id\"=@TagCategoryId WHERE \"id\"=@Id",
                    new { Id = tag.Id.Value, TagCategoryId = tag.TagCategory.Id });
            }

            return tag.TagCategory;
        }

        public static Common.Models.Tasks.TaskTag Disable(Common.Models.Tasks.TaskTag model,
            Common.Models.Security.User disabler)
        {
            model.DisabledBy = disabler;
            model.UtcDisabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("task_tag", disabler.Id.Value, model.Id);

            return model;
        }

        public static Common.Models.Tasks.TaskTag Enable(Common.Models.Tasks.TaskTag model,
            Common.Models.Security.User enabler)
        {
            model.ModifiedBy = enabler;
            model.UtcModified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.UtcDisabled = null;

            DataHelper.Enable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("task_tag", enabler.Id.Value, model.Id);

            return model;
        }

        public static List<Common.Models.Tasks.TaskTag> ListForTask(Guid taskId)
        {
            List<Common.Models.Tasks.TaskTag> list =
                DataHelper.List<Common.Models.Tasks.TaskTag, DBOs.Tasks.TaskTag>(
                "SELECT * FROM \"matter_tag\" WHERE \"task_id\"=@TaskId AND \"utc_disabled\" is null",
                new { TaskId = taskId });

            list.ForEach(x =>
            {
                x.TagCategory = Tagging.TagCategory.Get(x.TagCategory.Id);
            });

            return list;
        }

        public static List<Common.Models.Tasks.TaskTag> Search(string text)
        {
            List<Common.Models.Tasks.TaskTag> list = new List<Common.Models.Tasks.TaskTag>();
            List<DBOs.Tasks.TaskTag> dbo = null;

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                dbo = conn.Query<DBOs.Tasks.TaskTag>(
                    "SELECT * FROM \"task_tag\" WHERE LOWER(\"tag\") LIKE '%' || @Query || '%'",
                    new { Query = text }).ToList();
            }

            dbo.ForEach(x =>
            {
                Common.Models.Tasks.TaskTag tt = Mapper.Map<Common.Models.Tasks.TaskTag>(x);
                tt.TagCategory = Tagging.TagCategory.Get(tt.TagCategory.Id);
                tt.Task = Task.Get(tt.Task.Id.Value);
                list.Add(tt);
            });

            return list;
        }
    }
}