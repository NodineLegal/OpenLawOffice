using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace OpenLawOffice.WebClient.DBOs
{
    public abstract class Core : DboWithDatesOnly
    {
        [Required]
        [References(typeof(Security.User))]
        public int CreatedByUserId { get; set; }

        [Required]
        [References(typeof(Security.User))]
        public int ModifiedByUserId { get; set; }

        [References(typeof(Security.User))]
        public int? DisabledByUserId { get; set; }
    }
}