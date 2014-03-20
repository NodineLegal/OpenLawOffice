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
    using System.Linq;
    using System.Text;
    using AutoMapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TagCategory
    {
        public static Common.Models.Tagging.TagCategory Get(int id)
        {
        }

        public static Common.Models.Tagging.TagCategory Get(string name)
        {
            return null;
            //DbModels.TagCategory dbo = DbModels.TagCategory.FirstOrDefault(
            //    "SELECT * FROM \"tag_category\" WHERE \"name\"=@0 AND \"utc_disabled\" is null",
            //    name);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Tagging.TagCategory>(dbo);
        }

        public static Common.Models.Tagging.TagCategory Create(Common.Models.Tagging.TagCategory model,
            Common.Models.Security.User creator)
        {
            return null;
            //model.CreatedBy = model.ModifiedBy = creator;
            //model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            //DbModels.TagCategory dbo = Mapper.Map<DbModels.TagCategory>(model);
            //model.Id = dbo.Id = (int)dbo.Insert();
            //return model;
        }

        public static Common.Models.Tagging.TagCategory Enable(Common.Models.Tagging.TagCategory model,
            Common.Models.Security.User enabler)
        {
            return null;
            //model.ModifiedBy = enabler;
            //model.UtcModified = DateTime.UtcNow;
            //model.DisabledBy = null;
            //model.UtcDisabled = null;
            //DbModels.TagCategory dbo = Mapper.Map<DbModels.TagCategory>(model);
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
