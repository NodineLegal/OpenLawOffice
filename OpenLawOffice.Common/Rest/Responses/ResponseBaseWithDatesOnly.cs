using System;

namespace OpenLawOffice.Common.Rest.Responses
{
    public abstract class ResponseBaseWithDatesOnly : ResponseBase
    {
        public DateTime UtcCreated { get; set; }
        public DateTime UtcModified { get; set; }
        public DateTime? UtcDisabled { get; set; }
    }
}
