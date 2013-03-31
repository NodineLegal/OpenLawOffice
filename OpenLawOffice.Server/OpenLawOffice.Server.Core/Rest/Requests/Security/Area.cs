using System;
using ServiceStack.ServiceHost;

namespace OpenLawOffice.Server.Core.Rest.Requests.Security
{
    [Route("/Security/Areas")]
    [Route("/Security/Areas/{Id}")]
    public class Area : Common.Rest.Requests.Security.Area, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
