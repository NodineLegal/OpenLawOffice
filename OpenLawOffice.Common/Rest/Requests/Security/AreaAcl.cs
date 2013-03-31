namespace OpenLawOffice.Common.Rest.Requests.Security
{
    public class AreaAcl : RequestBase, IHasIntId
    {
        public int? Id { get; set; }
        public int? SecurityAreaId { get; set; }
        public int? UserId { get; set; }
        public Models.PermissionType? AllowFlags { get; set; }
        public Models.PermissionType? DenyFlags { get; set; }
    }
}
