using System;

namespace OpenLawOffice.Common.Rest.Responses.Matters
{
    public class MatterTag : Core
    {
        public Guid Id { get; set; }
        public long MatterId { get; set; }
        public int? MatterTagCategoryId { get; set; }
        public string Tag { get; set; }
    }
}
