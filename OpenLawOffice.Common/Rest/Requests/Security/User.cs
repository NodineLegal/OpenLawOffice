using System;

namespace OpenLawOffice.Common.Rest.Requests.Security
{
    public class User : RequestBase, IHasIntId
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid? UserAuthToken { get; set; }
        public DateTime? UserAuthTokenExpiry { get; set; }
    }
}
