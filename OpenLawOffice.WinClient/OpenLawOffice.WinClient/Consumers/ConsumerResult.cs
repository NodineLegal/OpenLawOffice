using System;
using OpenLawOffice.Common.Rest.Responses;
using RestSharp;

namespace OpenLawOffice.WinClient.Consumers
{
    public class ConsumerResult<TRequest, TResponse>
        where TRequest : OpenLawOffice.Common.Rest.Requests.RequestBase
    {
        //public ConsumerBase<TRequest, TResponse> Sender { get; set; }
        public TRequest Request { get; set; }
        public TResponse Response { get; set; }
        public IRestResponse RestSharpResponse { get; set; }
        public RestRequestAsyncHandle Handle { get; set; }
    }
}
