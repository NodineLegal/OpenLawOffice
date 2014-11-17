using System;

namespace OpenLawOffice.Common.Net
{
    public class Response<T>
    {
        public bool Successful { get; set; }
        public DateTime RequestReceived { get; set; }
        public DateTime ResponseSent { get; set; }
        public string Error { get; set; }
        public T Package { get; set; }
    }
}
