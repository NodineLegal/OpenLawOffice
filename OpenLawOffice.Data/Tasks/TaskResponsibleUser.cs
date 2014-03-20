// <copyright file="TaskResponsibleUser.cs" company="Nodine Legal, LLC">
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
    public static class TaskResponsibleUser
    {
        public static Common.Models.Tasks.TaskResponsibleUser Get(Guid id)
        {
            return null;
            //DbModels.TaskResponsibleUser dbo = DbModels.TaskResponsibleUser.FirstOrDefault(
            //    "SELECT * FROM \"task_responsible_user\" WHERE \"id\"=@0 AND \"utc_disabled\" is null",
            //    id);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Tasks.TaskResponsibleUser>(dbo);
        }

        public static Common.Models.Tasks.TaskResponsibleUser Get(long taskId, int userId)
        {
            return null;
            //DbModels.TaskResponsibleUser dbo = DbModels.TaskResponsibleUser.FirstOrDefault(
            //    "SELECT * FROM \"task_responsible_user\" WHERE \"task_id\"=@0 AND \"user_id\"=@1 AND \"utc_disabled\" is null",
            //    taskId, userId);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Tasks.TaskResponsibleUser>(dbo);
        }

        public static Common.Models.Tasks.TaskResponsibleUser Create(Common.Models.Tasks.TaskResponsibleUser model,
            Common.Models.Security.User creator)
        {
            return null;
            //if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            //model.CreatedBy = model.ModifiedBy = creator;
            //model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            //DbModels.TaskResponsibleUser dbo = Mapper.Map<DbModels.TaskResponsibleUser>(model);
            //dbo.Insert();
            //return model;
        }
    }
}
