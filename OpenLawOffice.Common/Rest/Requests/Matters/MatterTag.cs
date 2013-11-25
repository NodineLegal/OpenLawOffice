using System;

namespace OpenLawOffice.Common.Rest.Requests.Matters
{
    public class MatterTag : Tagging.TagBase
    {
        public Guid? MatterId { get; set; }
    }
}
