using System;
using System.Collections.Generic;

namespace OpenLawOffice.Common.Rest.Responses.Matters
{
    public class Matter : Security.SecuredResource
    {
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public string Synopsis { get; set; }
        public List<MatterTag> Tags { get; set; }
    }
}
