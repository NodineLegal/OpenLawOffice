using System;

namespace OpenLawOffice.Common.Models.Security
{
    public class User : ModelWithDatesOnly
    {
        public int? Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public Guid? UserAuthToken { get; set; }

        public DateTime? UserAuthTokenExpiry { get; set; }
    }
}