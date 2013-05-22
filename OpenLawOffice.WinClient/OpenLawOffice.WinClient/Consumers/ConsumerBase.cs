using System;
using System.Collections.Generic;
using OpenLawOffice.Common.Rest.Responses;
using System.Linq;
using RestSharp;

namespace OpenLawOffice.WinClient.Consumers
{
    public abstract class ConsumerBase<TRequest, TResponse>
        : ConsumerBase
        where TRequest : OpenLawOffice.Common.Rest.Requests.RequestBase
        where TResponse : ResponseBase
    {
        protected RestRequestAsyncHandle ExecuteAsync(RestRequest restSharpRequest, TRequest request, Action<ConsumerResult<TRequest, TResponse>> callback)
        {
            return ExecuteAsync<TRequest, TResponse>(restSharpRequest, request, callback);
        }

        protected ConsumerResult<TRequest, TResponse> Execute(RestRequest restSharpRequest, TRequest request)
        {
            return Execute<TRequest, TResponse>(restSharpRequest, request);
        }

        public virtual RestRequestAsyncHandle GetSingle(TRequest request, Action<ConsumerResult<TRequest, TResponse>> callback)
        {
            return GetSingle<TRequest, TResponse>(request, callback);
        }

        public virtual ConsumerResult<TRequest, TResponse> GetSingle(TRequest request)
        {
            return GetSingle<TRequest, TResponse>(request);
        }

        public virtual RestRequestAsyncHandle GetList(TRequest request, Action<ListConsumerResult<TRequest, TResponse>> callback)
        {
            return GetList<TRequest, TResponse>(request, callback);
        }

        public virtual RestRequestAsyncHandle Create(TRequest request, Action<ConsumerResult<TRequest, TResponse>> callback)
        {
            return Create<TRequest, TResponse>(request, callback);
        }

        public virtual RestRequestAsyncHandle Update(TRequest request, Action<ConsumerResult<TRequest, TResponse>> callback)
        {
            return Update<TRequest, TResponse>(request, callback);
        }

        public virtual RestRequestAsyncHandle Disable(TRequest request, Action<ConsumerResult<TRequest, TResponse>> callback)
        {
            return Disable<TRequest, TResponse>(request, callback);
        }
    }

    public abstract class ConsumerBase
    {
        public abstract string Resource { get; }

        protected string BuildFullResource(Common.Rest.Requests.RequestBase request)
        {
            Type requestType = request.GetType();
            if (typeof(Common.Rest.Requests.IHasIntId).IsAssignableFrom(requestType))
            {
                int? id = ((Common.Rest.Requests.IHasIntId)request).Id;
                if (!id.HasValue) throw new InvalidServiceTargetException("A GetSingle request must have an Id.");
                return (Resource + id.Value.ToString());
            }
            else if (typeof(Common.Rest.Requests.IHasLongId).IsAssignableFrom(requestType))
            {
                long? id = ((Common.Rest.Requests.IHasLongId)request).Id;
                if (!id.HasValue) throw new InvalidServiceTargetException("A GetSingle request must have an Id.");
                return (Resource + id.Value.ToString());
            }
            else if (typeof(Common.Rest.Requests.IHasGuidId).IsAssignableFrom(requestType))
            {
                Guid? id = ((Common.Rest.Requests.IHasGuidId)request).Id;
                if (!id.HasValue) throw new InvalidServiceTargetException("A GetSingle request must have an Id.");
                return (Resource + id.Value.ToString());
            }
            else
            {
                // Can't determine, so just fire it at the service and let the service figure it out
                // Technically we could just do this for all of them, but that seems a bit lazy.
                return Resource;
            }
        }

        protected virtual RestRequestAsyncHandle ExecuteAsync<TRequest, TResponse>(RestRequest restSharpRequest,
            TRequest request, 
            Action<ConsumerResult<TRequest, TResponse>> callback)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            RestClient client = new RestClient(Globals.Instance.Settings.HostBaseUrl);

            return client.ExecuteAsync<Common.Rest.Responses.ResponseContainer<TResponse>>
                (restSharpRequest, (response, handle) =>
            {
                if (callback != null)
                {
                    if (response.Data != null)
                        callback(new ConsumerResult<TRequest, TResponse>()
                        {
                            Request = request,
                            ResponseContainer = response.Data,
                            Response = response.Data.Data,
                            RestSharpResponse = response,
                            Handle = handle
                        });
                    else
                        callback(new ConsumerResult<TRequest, TResponse>()
                        {
                            Request = request,
                            Response = null,
                            ResponseContainer = response.Data,
                            RestSharpResponse = response,
                            Handle = handle
                        });
                }
            });
        }

        protected virtual ConsumerResult<TRequest, TResponse> Execute<TRequest, TResponse>(
            RestRequest restSharpRequest, TRequest request)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            RestClient client = new RestClient(Globals.Instance.Settings.HostBaseUrl);

            IRestResponse<Common.Rest.Responses.ResponseContainer<TResponse>> response =
                client.Execute<Common.Rest.Responses.ResponseContainer<TResponse>>(restSharpRequest);

            if (response.Data != null)
                return new ConsumerResult<TRequest, TResponse>()
                {
                    Request = request,
                    ResponseContainer = response.Data,
                    Response = response.Data.Data,
                    RestSharpResponse = response
                };
            else
                return new ConsumerResult<TRequest, TResponse>()
                {
                    Request = request,
                    Response = null,
                    ResponseContainer = response.Data,
                    RestSharpResponse = response
                };
        }

        public virtual RestRequestAsyncHandle GetSingle<TRequest, TResponse>(TRequest request,
            Action<ConsumerResult<TRequest, TResponse>> callback)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            RestRequest restRequest = RequestBuilder.Build(BuildFullResource(request), Method.GET, request);
            return ExecuteAsync<TRequest, TResponse>(restRequest, request, callback);
        }

        public virtual ConsumerResult<TRequest, TResponse> GetSingle<TRequest, TResponse>(TRequest request)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            RestRequest restRequest = RequestBuilder.Build(BuildFullResource(request), Method.GET, request);
            return Execute<TRequest, TResponse>(restRequest, request);
        }

        public virtual RestRequestAsyncHandle GetList<TRequest, TResponse>(TRequest request,
            Action<ListConsumerResult<TRequest, TResponse>> callback)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : ResponseBase
        {
            RestClient client = new RestClient(Globals.Instance.Settings.HostBaseUrl);
            RestRequest restRequest = RequestBuilder.Build(Resource, Method.GET, request);

            return client.ExecuteAsync<Common.Rest.Responses.ListResponseContainer<TResponse>>
                (restRequest, (response, handle) =>
            {
                if (callback != null)
                {
                    if (response.Data != null)
                        callback(new ListConsumerResult<TRequest, TResponse>()
                        {
                            //Sender = this,
                            Request = request,
                            ListResponseContainer = response.Data,
                            Response = response.Data.Data,
                            RestSharpResponse = response,
                            Handle = handle
                        });
                    else
                        callback(new ListConsumerResult<TRequest, TResponse>()
                        {
                            Request = request,
                            ListResponseContainer = response.Data,
                            Response = null,
                            RestSharpResponse = response,
                            Handle = handle
                        });
                }
            });
        }

        public virtual ListConsumerResult<TRequest, TResponse> GetList<TRequest, TResponse>(TRequest request)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : ResponseBase
        {
            IRestResponse<ListResponseContainer<TResponse>> response = null;
            RestClient client = new RestClient(Globals.Instance.Settings.HostBaseUrl);
            RestRequest restRequest = RequestBuilder.Build(Resource, Method.GET, request);

            response = client.Execute<Common.Rest.Responses.ListResponseContainer<TResponse>>(restRequest);
                        
            if (response.Data != null)
                return new ListConsumerResult<TRequest, TResponse>()
                {
                    //Sender = this,
                    Request = request,
                    ListResponseContainer = response.Data,
                    Response = response.Data.Data,
                    RestSharpResponse = response,
                    Handle = null
                };
            else
                return new ListConsumerResult<TRequest, TResponse>()
                {
                    Request = request,
                    ListResponseContainer = response.Data,
                    Response = null,
                    RestSharpResponse = response,
                    Handle = null
                };
        }

        public virtual RestRequestAsyncHandle Create<TRequest, TResponse>(TRequest request,
            Action<ConsumerResult<TRequest, TResponse>> callback)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            RestRequest restRequest = RequestBuilder.Build(Resource, Method.POST, request);
            return ExecuteAsync<TRequest, TResponse>(restRequest, request, callback);
        }

        public virtual RestRequestAsyncHandle Update<TRequest, TResponse>(TRequest request,
            Action<ConsumerResult<TRequest, TResponse>> callback)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            RestRequest restRequest = RequestBuilder.Build(Resource, Method.PUT, request);
            return ExecuteAsync<TRequest, TResponse>(restRequest, request, callback);
        }

        public virtual RestRequestAsyncHandle Disable<TRequest, TResponse>(TRequest request, 
            Action<ConsumerResult<TRequest, TResponse>> callback)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            RestRequest restRequest = RequestBuilder.Build(Resource, Method.DELETE, request);
            return ExecuteAsync<TRequest, TResponse>(restRequest, request, callback);
        }
    }
}
