using System.Net;

namespace OpenLawOffice.Common.Rest.Responses
{
    public abstract class ResponseContainerBase
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public Error Error { get; set; }
    }
}
