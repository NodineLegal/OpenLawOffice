using System;

namespace OpenLawOffice.Common.Rest.Responses.Matters
{
    public class MatterTag : Tagging.TagBase
    {
        public Matter Matter { get; set; }
    }
}
