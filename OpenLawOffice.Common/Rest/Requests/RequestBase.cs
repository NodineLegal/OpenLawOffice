using System;

namespace OpenLawOffice.Common.Rest.Requests
{
    public abstract class RequestBase
    {
        public DateTime? Received { get; set; }
        public Guid? AuthToken { get; set; }
        //public string SortField { get; set; }
        //public SortDirectionType SortDirection { get; set; }
        //public int? ListCount { get; set; }
    }
}
