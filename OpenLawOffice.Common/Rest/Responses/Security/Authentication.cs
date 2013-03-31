using System;

namespace OpenLawOffice.Common.Rest.Responses.Security
{
    public class Authentication : ResponseBase
    {
        public Guid AuthToken { get; set; }
    }
}
