using System;
using ServiceStack.ServiceHost;

namespace OpenLawOffice.Server.Core.Rest.Requests.Security
{
    [Route("/Security/Users")]
    [Route("/Security/Users/{Id}")]
    public class User : Common.Rest.Requests.Security.User, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
