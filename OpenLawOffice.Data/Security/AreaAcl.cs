// -----------------------------------------------------------------------
// <copyright file="AreaAcl.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AutoMapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class AreaAcl
    {
        public static Common.Models.Security.AreaAcl Get(int id)
        {
            return null;
            //DbModels.AreaAcl dbo = DbModels.AreaAcl.FirstOrDefault(
            //    "SELECT * FROM \"area_acl\" WHERE \"id\"=@0 AND \"utc_disabled\" is null",
            //    id);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Security.AreaAcl>(dbo);
        }

        public static Common.Models.Security.AreaAcl Get(int userId, int areaId)
        {
            return null;
            //DbModels.AreaAcl dbo = DbModels.AreaAcl.FirstOrDefault(
            //    "SELECT * FROM \"area_acl\" WHERE \"user_id\"=@0 AND \"area_id\"=@1 AND \"utc_disabled\" is null",
            //    userId, areaId);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Security.AreaAcl>(dbo);
        }

        public static List<Common.Models.Security.AreaAcl> List()
        {
            return null;
            //List<Common.Models.Security.AreaAcl> list = new List<Common.Models.Security.AreaAcl>();
            //IEnumerable<DbModels.AreaAcl> ie = DbModels.AreaAcl.Query(
            //    "SELECT * FROM \"area_acl\" WHERE \"utc_disabled\" is null");
            //foreach (DbModels.AreaAcl dbo in ie)
            //    list.Add(Mapper.Map<Common.Models.Security.AreaAcl>(dbo));
            //return list;
        }

        public static List<Common.Models.Security.AreaAcl> ListForArea(int areaId)
        {
            return null;
            //List<Common.Models.Security.AreaAcl> list = new List<Common.Models.Security.AreaAcl>();
            //IEnumerable<DbModels.AreaAcl> ie = DbModels.AreaAcl.Query(
            //    "SELECT * FROM \"area_acl\" WHERE \"security_area_id\"=@0 \"utc_disabled\" is null",
            //    areaId);
            //foreach (DbModels.AreaAcl dbo in ie)
            //    list.Add(Mapper.Map<Common.Models.Security.AreaAcl>(dbo));
            //return list;
        }

        public static Common.Models.Security.AreaAcl Create(Common.Models.Security.AreaAcl model,
            Common.Models.Security.User creator)
        {
            return null;
            //model.CreatedBy = model.ModifiedBy = creator;
            //model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            //DbModels.AreaAcl dbo = Mapper.Map<DbModels.AreaAcl>(model);
            //model.Id = dbo.Id = (int)dbo.Insert();
            //return model;
        }

        public static Common.Models.Security.AreaAcl Edit(Common.Models.Security.AreaAcl model,
            Common.Models.Security.User modifier)
        {
            return null;
            //model.ModifiedBy = modifier;
            //model.UtcModified = DateTime.UtcNow;
            //DbModels.AreaAcl dbo = Mapper.Map<DbModels.AreaAcl>(model);
            //dbo.Update(new string[] {
            //    "utc_modified",
            //    "modified_by_user_id",
            //    "security_area_id",
            //    "user_id",
            //    "allow_flags",
            //    "deny_flags"
            //});
            //return model;
        }

        public static Common.Models.Matters.MatterContact Disable(Common.Models.Matters.MatterContact model,
            Common.Models.Security.User disabler)
        {
            return null;
            //model.DisabledBy = disabler;
            //model.UtcDisabled = DateTime.UtcNow;
            //DbModels.MatterContact dbo = Mapper.Map<DbModels.MatterContact>(model);
            //dbo.Update(new string[] {
            //    "utc_disabled",
            //    "disabled_by_user_id"
            //});
            //return model;
        }

        public static Common.Models.Matters.MatterContact Enable(Common.Models.Matters.MatterContact model,
            Common.Models.Security.User enabler)
        {
            return null;
            //model.ModifiedBy = enabler;
            //model.UtcModified = DateTime.UtcNow;
            //model.DisabledBy = null;
            //model.UtcDisabled = null;
            //DbModels.MatterContact dbo = Mapper.Map<DbModels.MatterContact>(model);
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
