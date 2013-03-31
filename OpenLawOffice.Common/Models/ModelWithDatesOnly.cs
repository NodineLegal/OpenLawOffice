using System;

namespace OpenLawOffice.Common.Models
{
    public abstract class ModelWithDatesOnly : ModelBase
    {
        public DateTime? UtcCreated { get; set; }
        public DateTime? UtcModified { get; set; }
        public DateTime? UtcDisabled { get; set; }
    }
}
