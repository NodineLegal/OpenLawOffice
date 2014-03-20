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
    using System.Linq;
    using System.Text;
    using AutoMapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TaskTag
    {
        public static Common.Models.Tasks.TaskTag Get(Guid id)
        {
            return null;
            //DbModels.TaskTag dbo = DbModels.TaskTag.FirstOrDefault(
            //    "SELECT * FROM \"task_tag\" WHERE \"id\"=@0 AND \"utc_disabled\" is null",
            //    id);
            //if (dbo == null) return null;

            //Common.Models.Tasks.TaskTag model = Mapper.Map<Common.Models.Tasks.TaskTag>(dbo);

            //if (dbo.TagCategoryId.HasValue)
            //    model.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(
            //        DbModels.TagCategory.SingleOrDefault(model.TagCategory.Id));

            //return model;
        }

        public static List<Common.Models.Tasks.TaskTag> List()
        {
            return null;
            //List<Common.Models.Tasks.TaskTag> list = new List<Common.Models.Tasks.TaskTag>();
            //IEnumerable<DbModels.TaskTag> ie = DbModels.TaskTag.Query(
            //    "SELECT * FROM \"task_tag\" WHERE \"utc_disabled\" is null");
            //foreach (DbModels.TaskTag dbo in ie)
            //    list.Add(Mapper.Map<Common.Models.Tasks.TaskTag>(dbo));
            //return list;
        }

        public static List<Common.Models.Tasks.TaskTag> ListForMatter(Guid taskId)
        {
            return null;
            //List<Common.Models.Tasks.TaskTag> list = new List<Common.Models.Tasks.TaskTag>();
            //IEnumerable<DbModels.TaskTag> ie = DbModels.TaskTag.Query(
            //    "SELECT * FROM \"task_tag\" WHERE \"task_id\"=@0 \"utc_disabled\" is null",
            //    taskId.ToString());
            //foreach (DbModels.TaskTag dbo in ie)
            //{
            //    Common.Models.Tasks.TaskTag tagModel = Mapper.Map<Common.Models.Tasks.TaskTag>(dbo);
            //    if (dbo.TagCategoryId.HasValue)
            //    {
            //        DbModels.TagCategory tagCat = DbModels.TagCategory.SingleOrDefault(dbo.TagCategoryId.Value);
            //        tagModel.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(tagCat);
            //    }
            //    list.Add(tagModel);
            //}
            //return list;
        }

        public static Common.Models.Tasks.TaskTag Create(Common.Models.Tasks.TaskTag model,
            Common.Models.Security.User creator)
        {
            return null;
            //if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            //model.CreatedBy = model.ModifiedBy = creator;
            //model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            //DbModels.TaskTag dbo = Mapper.Map<DbModels.TaskTag>(model);
            //dbo.Insert();
            //UpdateTagCategory(model, creator);
            //return model;
        }

        public static Common.Models.Tasks.TaskTag Edit(Common.Models.Tasks.TaskTag model,
            Common.Models.Security.User modifier)
        {
            return null;
            //model.ModifiedBy = modifier;
            //model.UtcModified = DateTime.UtcNow;
            //DbModels.TaskTag dbo = Mapper.Map<DbModels.TaskTag>(model);
            //dbo.Update(new string[] {
            //    "utc_modified",
            //    "modified_by_user_id",
            //    "task_id",
            //    "tag"
            //});
            //UpdateTagCategory(model, modifier);
            //return model;
        }

        private static Common.Models.Tagging.TagCategory UpdateTagCategory(
            Common.Models.Tasks.TaskTag model,
            Common.Models.Security.User modifier)
        {
            return null;
            //DbModels.TaskTag currentTagDbo = DbModels.TaskTag.SingleOrDefault(model.Id.Value);

            //if (currentTagDbo.TagCategoryId.HasValue)
            //{
            //    if (model.TagCategory != null && !string.IsNullOrEmpty(model.TagCategory.Name))
            //    { // If current has tag & new has tag                    
            //        // Are they the same - ignore if so
            //        if (currentTagDbo.Tag != model.Tag)
            //        {
            //            // Update - change tagcat
            //            model.TagCategory = AddOrChangeTagCategory(model, modifier);
            //        }
            //    }
            //    else
            //    {
            //        // If current has tag & new !has tag
            //        // Update - drop tagcat
            //        currentTagDbo.TagCategoryId = null;
            //        currentTagDbo.Update(new string[] { "tag_category_id" });
            //    }
            //}
            //else
            //{
            //    if (model.TagCategory != null && !string.IsNullOrEmpty(model.TagCategory.Name))
            //    { // If current !has tag & new has tag
            //        // Update - add tagcat
            //        model.TagCategory = AddOrChangeTagCategory(model, modifier);
            //    }
            //    // If current !has tag & new !has tag - do nothing
            //}

            //return model.TagCategory;
        }

        private static Common.Models.Tagging.TagCategory AddOrChangeTagCategory(
            Common.Models.Tasks.TaskTag tag,
            Common.Models.Security.User modifier)
        {
            return null;
            //DbModels.TagCategory newTagCat = null;

            //// Check for existing name
            //if (tag.TagCategory != null && !string.IsNullOrEmpty(tag.TagCategory.Name))
            //{
            //    newTagCat = DbModels.TagCategory.FirstOrDefault("SELECT * FROM \"tag_category\" WHERE \"name\"=@0",
            //        tag.TagCategory.Name);
            //}

            //// Either need to use existing or create a new tag category
            //if (newTagCat != null)
            //{
            //    // Can use existing
            //    tag.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(newTagCat);

            //    // If new tagcat was disabled, it needs enabled
            //    if (newTagCat.UtcDisabled.HasValue)
            //    {
            //        tag.TagCategory = Tagging.TagCategory.Enable(tag.TagCategory, modifier);
            //    }
            //}
            //else
            //{
            //    // Add one
            //    tag.TagCategory = Tagging.TagCategory.Create(tag.TagCategory, modifier);
            //}

            //// Update TaskTag's TagCategoryId
            //DbModels.TaskTag dboTag = Mapper.Map<DbModels.TaskTag>(tag);
            //dboTag.Update(new string[] { "tag_category_id" });

            //return tag.TagCategory;
        }

        public static Common.Models.Tasks.TaskTag Disable(Common.Models.Tasks.TaskTag model,
            Common.Models.Security.User disabler)
        {
            return null;
            //model.DisabledBy = disabler;
            //model.UtcDisabled = DateTime.UtcNow;
            //DbModels.TaskTag dbo = Mapper.Map<DbModels.TaskTag>(model);
            //dbo.Update(new string[] {
            //    "utc_disabled",
            //    "disabled_by_user_id"
            //});
            //return model;
        }

        public static Common.Models.Tasks.TaskTag Enable(Common.Models.Tasks.TaskTag model,
            Common.Models.Security.User enabler)
        {
            return null;
            //model.ModifiedBy = enabler;
            //model.UtcModified = DateTime.UtcNow;
            //model.DisabledBy = null;
            //model.UtcDisabled = null;
            //DbModels.TaskTag dbo = Mapper.Map<DbModels.TaskTag>(model);
            //dbo.Update(new string[] {
            //    "utc_modified",
            //    "modified_by_user_id",
            //    "utc_disabled",
            //    "disabled_by_user_id"
            //});
            //return model;
        }
    }
}
