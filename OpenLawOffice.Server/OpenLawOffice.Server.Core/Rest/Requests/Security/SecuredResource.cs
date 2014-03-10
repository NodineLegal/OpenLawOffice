using System;
using ServiceStack;

namespace OpenLawOffice.Server.Core.Rest.Requests.Security
{
    [Route("/Security/SecuredResources")]
    [Route("/Security/SecuredResources/{Id}")]
    public class SecuredResource : Common.Rest.Requests.Security.SecuredResource, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
