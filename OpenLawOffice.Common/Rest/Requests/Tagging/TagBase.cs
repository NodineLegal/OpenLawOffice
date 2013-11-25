using System;

namespace OpenLawOffice.Common.Rest.Requests.Tagging
{
    public abstract class TagBase : RequestBase, IHasGuidId
    {
        public Guid? Id { get; set; }
        public int? TagCategoryId { get; set; }
        public string TagCategoryName { get; set; }
        public string Tag { get; set; }
    }
}
