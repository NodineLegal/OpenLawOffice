using System;
using System.Collections.Generic;
using OpenLawOffice.Common.Rest.Responses;
using RestSharp;

namespace OpenLawOffice.WinClient.Consumers
{
    public class ListConsumerResult<TRequest, TResponse>
        where TRequest : OpenLawOffice.Common.Rest.Requests.RequestBase
        where TResponse : ResponseBase
    {
        public TRequest Request { get; set; }
        public List<TResponse> Response { get; set; }
        public ListResponseContainer<TResponse> ListResponseContainer { get; set; }
        public IRestResponse RestSharpResponse { get; set; }
        public RestRequestAsyncHandle Handle { get; set; }
    }
}
