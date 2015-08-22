
namespace OpenLawOffice.Common.Net
{
    using System;

    public class AuthPackage
    {
        public string AppName { get; set; }
        public Guid MachineId { get; set; }
        public string Username { get; set; }
        public string IV { get; set; }
        public string Password { get; set; }
    }
}
