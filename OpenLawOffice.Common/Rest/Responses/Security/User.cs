using System;

namespace OpenLawOffice.Common.Rest.Responses.Security
{
    public class User : ResponseBaseWithDatesOnly
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Guid? UserAuthToken { get; set; }
        public DateTime? UserAuthTokenExpiry { get; set; }
    }
}
