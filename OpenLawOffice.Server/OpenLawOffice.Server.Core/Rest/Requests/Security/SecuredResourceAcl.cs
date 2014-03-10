using System;
using ServiceStack;

namespace OpenLawOffice.Server.Core.Rest.Requests.Security
{
    [Route("/Security/SecuredResourceAcls")]
    [Route("/Security/SecuredResourceAcls/{Id}")]
    public class SecuredResourceAcl : Common.Rest.Requests.Security.SecuredResourceAcl, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
