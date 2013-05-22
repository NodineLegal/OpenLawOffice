using System;
using System.Reflection;
using RestSharp;

namespace OpenLawOffice.WinClient.Consumers
{
    public static class RequestBuilder
    {
        /* RequestBuilder is born to simply build all parts of the RestSharp RestRequest EXCEPT
         * the host/resource (url).  There are some considerations that made this desireable.
         * 1) I do not want to manually code every parameter for every object - read extensibility
         * 2) While PUT and Post might be accomidating of a request body, Get and Delete are not
         * 3) Uniformity in procedure is desired, when feasible
         * 
         * Thus, we will require all requests objects going through request builder to be flat.  Meaning
         * all properties must be capable of being handed to RestRequest.AddParameter as the value.
         */

        public static RestRequest Build(string resource, Method method, Common.Rest.Requests.RequestBase request)
        {
            RestRequest restSharpRequest = new RestRequest(resource, method);
            PropertyInfo[] requestProperties = request.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (!request.AuthToken.HasValue)
                request.AuthToken = Globals.Instance.AuthToken;

            foreach (PropertyInfo property in requestProperties)
            {
                object value = property.GetValue(request, null);
                
                if (value != null)
                    restSharpRequest.AddParameter(property.Name, value);
            }

            return restSharpRequest;
        }
    }
}
