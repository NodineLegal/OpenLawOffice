using System.Net;

namespace OpenLawOffice.Common.Rest.Responses
{
    public class ResponseContainer<T> : ResponseContainer
        where T : ResponseBase
    {
        public T Data { get; set; }
    }

    public abstract class ResponseContainer
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public Error Error { get; set; }
        public bool WasSuccessful
        {
            get
            {
                if (HttpStatusCode == System.Net.HttpStatusCode.OK && Error == null)
                    return true;
                else
                {
                    if (Error == null)
                        Error = new Error();
                    return false;
                }
            }
        }
    }
}
