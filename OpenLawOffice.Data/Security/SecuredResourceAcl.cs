// -----------------------------------------------------------------------
// <copyright file="SecuredResourceAcl.cs" company="">
// TODO: Update copyright text.
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
    public static class SecuredResourceAcl
    {
        public static Common.Models.Security.SecuredResourceAcl Create(Common.Models.Security.SecuredResourceAcl model,
            Common.Models.Security.User creator)
        {
            return null;
            //if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            //model.CreatedBy = model.ModifiedBy = creator;
            //model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            //DbModels.SecuredResourceAcl dbo = Mapper.Map<DbModels.SecuredResourceAcl>(model);
            //dbo.Insert();
            //return model;
        }

        public static Common.Models.Security.SecuredResourceAcl Get(int userId, Guid securedResourceId)
        {
            return null;
            //DbModels.SecuredResourceAcl dbo = DbModels.SecuredResourceAcl.FirstOrDefault(
            //    "SELECT * FROM \"secured_resource_acl\" WHERE \"user_id\"=@0 AND \"secured_resource_id\"=@1 AND \"utc_disabled\" is null",
            //    userId, securedResourceId);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Security.SecuredResourceAcl>(dbo);
        }
    }
}
