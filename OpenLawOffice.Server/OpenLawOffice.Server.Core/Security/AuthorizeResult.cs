using System;

namespace OpenLawOffice.Server.Core.Security
{
    public class AuthorizeResult
    {
        public bool HasError { get; set; }
        public bool IsAuthorized { get; set; }
        public string ErrorMessage { get; set; }
        public Common.Models.Security.User RequestingUser { get; set; }
    }
}
