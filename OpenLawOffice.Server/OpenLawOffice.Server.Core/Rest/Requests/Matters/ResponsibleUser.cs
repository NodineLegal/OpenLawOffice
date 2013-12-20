using System;
using ServiceStack.ServiceHost;

namespace OpenLawOffice.Server.Core.Rest.Requests.Matters
{
    [Route("/Matters/ResponsibleUsers")]
    [Route("/Matters/ResponsibleUsers/{Id}")]
    public class ResponsibleUser : Common.Rest.Requests.Matters.ResponsibleUser, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
