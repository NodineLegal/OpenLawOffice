// -----------------------------------------------------------------------
// <copyright file="Time.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Timing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AutoMapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Time
    {
        public static Common.Models.Timing.Time Get(Guid id)
        {
            return null;
            //DbModels.Time dbo = DbModels.Time.FirstOrDefault(
            //    "SELECT * FROM \"time\" WHERE \"id\"=@0 AND \"utc_disabled\" is null",
            //    id);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Timing.Time>(dbo);
        }

        public static List<Common.Models.Timing.Time> ListForTask(long taskId)
        {
            return null;
            //List<Common.Models.Timing.Time> list = new List<Common.Models.Timing.Time>();
            //IEnumerable<DbModels.Time> ie = DbModels.Time.Query(
            //    "SELECT * FROM \"time\" JOIN \"task_time\" ON \"time\".\"id\"=\"task_time\".\"time_id\" " +
            //    "WHERE \"task_id\"=@0 AND \"utc_disabled\" is null", taskId);

            //foreach (DbModels.Time dbo in ie)
            //    list.Add(Mapper.Map<Common.Models.Timing.Time>(dbo));

            //return list;
        }

        public static Common.Models.Tasks.Task GetRelatedTask(Guid timeId)
        {
            return null;
            //DbModels.Task dbo = DbModels.Task.FirstOrDefault(
            //    "SELECT* FROM \"task\" JOIN \"task_time\" ON \"task\".\"id\"=\"task_time\".\"task_id\" "+
            //    "WHERE \"time_id\"=@0 AND \"utc_disabled\" is null",
            //    timeId);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Tasks.Task>(dbo);
        }

        public static Common.Models.Timing.Time Create(Common.Models.Timing.Time model,
            Common.Models.Security.User creator)
        {
            return null;
            //if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            //model.CreatedBy = model.ModifiedBy = creator;
            //model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            //DbModels.Time dbo = Mapper.Map<DbModels.Time>(model);
            //dbo.Insert();
            //return model;
        }

        public static Common.Models.Timing.Time Edit(Common.Models.Timing.Time model,
            Common.Models.Security.User modifier)
        {
            return null;
            //model.ModifiedBy = modifier;
            //model.UtcModified = DateTime.UtcNow;
            //DbModels.Time dbo = Mapper.Map<DbModels.Time>(model);
            //dbo.Update(new string[] {
            //    "utc_modified",
            //    "modified_by_user_id",
            //    "start",
            //    "stop",
            //    "worker_contact_id"
            //});

            //return model;
        }
    }
}
