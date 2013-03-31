using System.Net;
using System.Collections.Generic;

namespace OpenLawOffice.Common.Rest.Responses
{
    public class ListResponseContainer<T> : ResponseContainerBase
        where T : ResponseBase
    {
        public List<T> Data { get; set; }
    }
}
