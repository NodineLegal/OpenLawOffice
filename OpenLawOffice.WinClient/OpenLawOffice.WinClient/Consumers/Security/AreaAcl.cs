using RestSharp;

namespace OpenLawOffice.WinClient.Consumers.Security
{
    public class AreaAcl
        : ConsumerBase<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
    {
        public override string Resource { get { return "Security/AreaAcls/"; } }

        // RestSharp bug?  The only reason this override exists is because RestSharp
        // will not parse from json int to enum
        protected override RestSharp.RestRequestAsyncHandle ExecuteAsync<TRequest, TResponse>(RestSharp.RestRequest restSharpRequest, TRequest request, System.Action<ConsumerResult<TRequest, TResponse>> callback)
        {
            RestClient client = new RestClient(Globals.Instance.Settings.HostBaseUrl);

            return client.ExecuteAsync<Common.Rest.Responses.ResponseContainer<TResponse>>
                (restSharpRequest, (response, handle) =>
                {
                    ConsumerResult<TRequest, TResponse> result = new ConsumerResult<TRequest, TResponse>();

                    // Ask ServiceStack to fix
                    Common.Rest.Responses.ResponseContainer<TResponse> deserializationOverrideResult =
                        ServiceStack.Text.JsonSerializer.DeserializeFromString<Common.Rest.Responses.ResponseContainer<TResponse>>(response.Content);

                    // override JsonSharp's deserialized object with 
                    // ServiceStack's object
                    result.Request = request;
                    result.ResponseContainer = deserializationOverrideResult;
                    result.Response = deserializationOverrideResult.Data;
                    result.RestSharpResponse = response;
                    result.Handle = handle;

                    if (callback != null)
                        callback(result);
                });
        }

        // RestSharp bug?  The only reason this override exists is because RestSharp
        // will not parse from json int to enum
        protected override ConsumerResult<TRequest, TResponse> Execute<TRequest, TResponse>(RestRequest restSharpRequest, TRequest request)
        {
            RestClient client = new RestClient(Globals.Instance.Settings.HostBaseUrl);
            ConsumerResult<TRequest, TResponse> result = new ConsumerResult<TRequest, TResponse>();

            IRestResponse<Common.Rest.Responses.ResponseContainer<TResponse>> response =
                client.Execute<Common.Rest.Responses.ResponseContainer<TResponse>>(restSharpRequest);

            // Ask ServiceStack to fix
            Common.Rest.Responses.ResponseContainer<TResponse> deserializationOverrideResult =
                ServiceStack.Text.JsonSerializer.DeserializeFromString<Common.Rest.Responses.ResponseContainer<TResponse>>(response.Content);

            // override JsonSharp's deserialized object with 
            // ServiceStack's object
            result.Request = request;
            result.ResponseContainer = deserializationOverrideResult;
            result.Response = deserializationOverrideResult.Data;
            result.RestSharpResponse = response;
            result.Handle = null;

            return result;
        }

        // RestSharp bug?  The only reason this override exists is because RestSharp
        // will not parse from json int to enum
        public override RestRequestAsyncHandle GetList<TRequest, TResponse>(TRequest request, System.Action<ListConsumerResult<TRequest, TResponse>> callback)
        {
            RestClient client = new RestClient(Globals.Instance.Settings.HostBaseUrl);
            RestRequest restSharpRequest = RequestBuilder.Build(Resource, Method.GET, request);

            return client.ExecuteAsync<Common.Rest.Responses.ListResponseContainer<TResponse>>
                (restSharpRequest, (response, handle) =>
                {
                    ListConsumerResult<TRequest, TResponse> result = new ListConsumerResult<TRequest, TResponse>();

                    // Ask ServiceStack to fix
                    Common.Rest.Responses.ListResponseContainer<TResponse> deserializationOverrideResult =
                        ServiceStack.Text.JsonSerializer.DeserializeFromString<Common.Rest.Responses.ListResponseContainer<TResponse>>(response.Content);

                    // override JsonSharp's deserialized object with 
                    // ServiceStack's object
                    result.Request = request;
                    result.ListResponseContainer = deserializationOverrideResult;
                    result.Response = deserializationOverrideResult.Data;
                    result.RestSharpResponse = response;
                    result.Handle = handle;

                    if (callback != null)
                        callback(result);
                });
        }

        // RestSharp bug?  The only reason this override exists is because RestSharp
        // will not parse from json int to enum
        public override ListConsumerResult<TRequest, TResponse> GetList<TRequest, TResponse>(TRequest request)
        {
            ListConsumerResult<TRequest, TResponse> result = new ListConsumerResult<TRequest, TResponse>();
            IRestResponse<Common.Rest.Responses.ListResponseContainer<TResponse>> response = null;
            RestClient client = new RestClient(Globals.Instance.Settings.HostBaseUrl);
            RestRequest restRequest = RequestBuilder.Build(Resource, Method.GET, request);

            response = client.Execute<Common.Rest.Responses.ListResponseContainer<TResponse>>(restRequest);

            // Ask ServiceStack to fix
            Common.Rest.Responses.ListResponseContainer<TResponse> deserializationOverrideResult =
                ServiceStack.Text.JsonSerializer.DeserializeFromString<Common.Rest.Responses.ListResponseContainer<TResponse>>(response.Content);

            // override JsonSharp's deserialized object with 
            // ServiceStack's object
            result.Request = request;
            result.ListResponseContainer = deserializationOverrideResult;
            result.Response = deserializationOverrideResult.Data;
            result.RestSharpResponse = response;
            result.Handle = null;

            return result;
        }
    }
}
