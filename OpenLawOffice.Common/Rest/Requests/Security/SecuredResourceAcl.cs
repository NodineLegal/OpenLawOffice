using System;

namespace OpenLawOffice.Common.Rest.Requests.Security
{
    public class SecuredResourceAcl : RequestBase, IHasGuidId
    {
        public Guid? Id { get; set; }
        public Guid? SecuredResourceId { get; set; }
        public int? UserId { get; set; }
        public Models.PermissionType? AllowFlags { get; set; }
        public Models.PermissionType? DenyFlags { get; set; }
    }
}
