using System;
using System.Collections.Generic;

namespace OpenLawOffice.Common.Rest.Responses.Matters
{
    public class Matter : Core
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<MatterTag> MatterTags { get; set; }
    }
}
