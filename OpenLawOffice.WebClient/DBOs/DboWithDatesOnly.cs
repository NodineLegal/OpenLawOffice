using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLawOffice.WebClient.DBOs
{
    public abstract class DboWithDatesOnly : DboBase
    {
        [Required]
        public DateTime UtcCreated { get; set; }

        [Required]
        public DateTime UtcModified { get; set; }

        public DateTime? UtcDisabled { get; set; }
    }
}