using System;
using ServiceStack.ServiceHost;

namespace OpenLawOffice.Server.Core.Rest.Requests.Security
{
    [Route("/Security/Acls")]
    [Route("/Security/Acls/{Id}")]
    public class AreaAcl : Common.Rest.Requests.Security.AreaAcl, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
