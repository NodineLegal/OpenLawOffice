using System;

namespace OpenLawOffice.Common.Rest.Responses.Tagging
{
    public abstract class TagBase : Core
    {
        public Guid Id { get; set; }
        public TagCategory TagCategory { get; set; }
        public string Tag { get; set; }
    }
}
