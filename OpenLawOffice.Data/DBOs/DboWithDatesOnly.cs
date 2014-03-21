using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLawOffice.Data.DBOs
{
    public abstract class DboWithDatesOnly : DboBase
    {
        [ColumnMapping(Name="utc_created")]
        public DateTime UtcCreated { get; set; }

        [ColumnMapping(Name = "utc_modified")]
        public DateTime UtcModified { get; set; }

        [ColumnMapping(Name = "utc_disabled")]
        public DateTime? UtcDisabled { get; set; }
    }
}