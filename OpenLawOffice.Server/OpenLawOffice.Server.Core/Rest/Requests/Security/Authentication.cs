using ServiceStack.ServiceHost;

namespace OpenLawOffice.Server.Core.Rest.Requests.Security
{
    [Route("/Authenticate")]
    [Route("/Authentication")]
    public class Authentication : Common.Rest.Requests.Security.Authentication, IHasSession
    {
        public Core.Security.Session Session { get; set; }
    }
}
