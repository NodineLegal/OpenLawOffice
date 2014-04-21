namespace OpenLawOffice.Common.Models.Security
{
    public class AreaAcl : Core
    {
        public int? Id { get; set; }

        public Area Area { get; set; }

        public User User { get; set; }

        public Models.PermissionType? AllowFlags { get; set; }

        public Models.PermissionType? DenyFlags { get; set; }
    }
}