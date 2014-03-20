// -----------------------------------------------------------------------
// <copyright file="TaskMatter.cs" company="Nodine Legal, LLC">
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
    public static class TaskMatter
    {
        public static Common.Models.Tasks.TaskMatter Get(Guid id)
        {
            return null;
            //DbModels.TaskMatter dbo = DbModels.TaskMatter.FirstOrDefault(
            //    "SELECT * FROM \"task_matter\" WHERE \"id\"=@0 AND \"utc_disabled\" is null",
            //    id);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Tasks.TaskMatter>(dbo);
        }

        public static Common.Models.Tasks.TaskMatter Get(long taskId, Guid matterId)
        {
            return null;
            //DbModels.MatterContact dbo = DbModels.MatterContact.FirstOrDefault(
            //    "SELECT * FROM \"task_matter\" WHERE \"task_id\"=@0 AND \"matter_id\"=@1 AND \"utc_disabled\" is null",
            //    taskId, matterId);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Tasks.TaskMatter>(dbo);
        }

        public static Common.Models.Tasks.TaskMatter Create(Common.Models.Tasks.TaskMatter model,
            Common.Models.Security.User creator)
        {
            return null;
            //if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            //model.CreatedBy = model.ModifiedBy = creator;
            //model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            //DbModels.TaskMatter dbo = Mapper.Map<DbModels.TaskMatter>(model);
            //dbo.Insert();
            //return model;
        }

    }
}
