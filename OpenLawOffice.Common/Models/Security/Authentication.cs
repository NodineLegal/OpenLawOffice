using System;

namespace OpenLawOffice.Common.Models.Security
{
    public class Authentication : ModelBase
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public Guid? AuthToken { get; set; }
    }
}