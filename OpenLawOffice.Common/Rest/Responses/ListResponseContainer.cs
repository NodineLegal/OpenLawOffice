using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenLawOffice.Common.Rest.Responses
{
    public class ListResponseContainer<T>
        : ResponseContainer
        where T : ResponseBase
    {
        public List<T> Data { get; set; }
    }
}
