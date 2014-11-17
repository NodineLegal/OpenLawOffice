using System;

namespace OpenLawOffice.Common.Net
{
    public class Request<T>
    {
        public Guid Token { get; set; }
        public T Package { get; set; }
    }
}
