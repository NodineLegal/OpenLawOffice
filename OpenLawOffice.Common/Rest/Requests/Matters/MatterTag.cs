using System;

namespace OpenLawOffice.Common.Rest.Requests.Matters
{
    public class MatterTag : RequestBase, IHasGuidId
    {
        public Guid? Id { get; set; }
        public long? MatterId { get; set; }
        public int? MatterTagCategoryId { get; set; }
        public string Tag { get; set; }
    }
}
