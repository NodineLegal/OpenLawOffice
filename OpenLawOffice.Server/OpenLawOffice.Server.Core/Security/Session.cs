using System;

namespace OpenLawOffice.Server.Core.Security
{
    public class Session
    {
        public Guid? AuthToken { get; set; }
        public Common.Models.Security.User RequestingUser { get; set; }
    }
}
