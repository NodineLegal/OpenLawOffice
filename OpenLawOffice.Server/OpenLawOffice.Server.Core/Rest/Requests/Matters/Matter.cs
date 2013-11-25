using System;
using ServiceStack.ServiceHost;

namespace OpenLawOffice.Server.Core.Rest.Requests.Matters
{
    [Route("/Matters")]
    [Route("/Matters/{Id}")]
    public class Matter : Common.Rest.Requests.Matters.Matter, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
