// -----------------------------------------------------------------------
// <copyright file="SecuredResource.cs" company="">
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
    public static class SecuredResource
    {
        public static Common.Models.Security.SecuredResource Get(Guid id)
        {
            return null;
            //DbModels.SecuredResource dbo = DbModels.SecuredResource.FirstOrDefault("SELECT * FROM \"secured_resource\" WHERE \"id\"=@0 AND \"utc_disabled\" is null",
            //    id);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Security.SecuredResource>(dbo);
        }

        public static Common.Models.Security.SecuredResource Create(Common.Models.Security.SecuredResource model,
            Common.Models.Security.User creator)
        {
            return null;
            //if (!model.Id.HasValue)
            //    throw new ArgumentException("Must set Id before attempting to create SecuredResource.");

            //model.CreatedBy = model.ModifiedBy = creator;
            //model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            //DbModels.SecuredResource dbo = Mapper.Map<DbModels.SecuredResource>(model);
            //dbo.Insert();

            //// Acl
            //SecuredResourceAcl.Create(new Common.Models.Security.SecuredResourceAcl()
            //{
            //    User = creator,
            //    SecuredResource = model,
            //    AllowFlags = Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllRead | Common.Models.PermissionType.AllWrite,
            //    DenyFlags = Common.Models.PermissionType.None
            //}, creator);

            //return model;
        }

        public static Common.Models.Security.SecuredResource Edit(Common.Models.Security.SecuredResource model,
            Common.Models.Security.User modifier)
        {
            return null;
            //model.ModifiedBy = modifier;
            //model.UtcModified = DateTime.UtcNow;
            //DbModels.SecuredResource dbo = Mapper.Map<DbModels.SecuredResource>(model);
            //dbo.Update(new string[] {
            //    "utc_modified",
            //    "modified_by_user_id"
            //});
            //return model;
        }
    }
}
