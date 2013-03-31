using System.Net;

namespace OpenLawOffice.Common.Rest.Responses
{
    public class ResponseContainer<T> : ResponseContainerBase
        where T : ResponseBase
    {
        public T Data { get; set; }
    }
}
