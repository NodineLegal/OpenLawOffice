using System;

namespace OpenLawOffice.Common.Rest.Requests.Matters
{
    public class Matter : RequestBase, IHasGuidId
    {
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }

        public string TagQuery { get; set; }
    }
}
