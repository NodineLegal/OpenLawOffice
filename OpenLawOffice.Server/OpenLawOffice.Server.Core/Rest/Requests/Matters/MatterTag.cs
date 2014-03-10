using System;
using ServiceStack;

namespace OpenLawOffice.Server.Core.Rest.Requests.Matters
{
    [Route("/Matters/Tags")]
    [Route("/Matters/Tags/{Id}")]
    public class MatterTag : Common.Rest.Requests.Matters.MatterTag, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
