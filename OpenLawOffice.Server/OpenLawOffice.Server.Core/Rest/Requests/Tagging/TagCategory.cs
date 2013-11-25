using System;
using ServiceStack.ServiceHost;

namespace OpenLawOffice.Server.Core.Rest.Requests.Tagging
{
    [Route("/Tagging/Categories")]
    [Route("/Tagging/Categories/{Id}")]
    public class TagCategory : Common.Rest.Requests.Tagging.TagCategory, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
