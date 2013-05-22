using System;
using ServiceStack.ServiceHost;

namespace OpenLawOffice.Server.Core.Rest.Requests.Security
{
    [Route("/Security/AreaAcls")]
    [Route("/Security/AreaAcls/{Id}")]
    public class AreaAcl : Common.Rest.Requests.Security.AreaAcl, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
