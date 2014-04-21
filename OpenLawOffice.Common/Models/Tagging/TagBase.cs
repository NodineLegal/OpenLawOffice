using System;

namespace OpenLawOffice.Common.Models.Tagging
{
    public abstract class TagBase : Core
    {
        public Guid? Id { get; set; }

        public TagCategory TagCategory { get; set; }

        public string Tag { get; set; }
    }
}