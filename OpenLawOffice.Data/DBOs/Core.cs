using System.ComponentModel.DataAnnotations;

namespace OpenLawOffice.Data.DBOs
{
    public abstract class Core : DboWithDatesOnly
    {
        [Required]
        public int CreatedByUserId { get; set; }

        [Required]
        public int ModifiedByUserId { get; set; }

        public int? DisabledByUserId { get; set; }
    }
}