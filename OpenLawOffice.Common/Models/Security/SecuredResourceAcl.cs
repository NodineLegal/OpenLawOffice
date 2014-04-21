using System;

namespace OpenLawOffice.Common.Models.Security
{
    public class SecuredResourceAcl : Core
    {
        public Guid? Id { get; set; }

        public SecuredResource SecuredResource { get; set; }

        public User User { get; set; }

        public Models.PermissionType? AllowFlags { get; set; }

        public Models.PermissionType? DenyFlags { get; set; }
    }
}