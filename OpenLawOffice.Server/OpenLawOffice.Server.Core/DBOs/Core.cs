using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OpenLawOffice.Server.Core.DBOs
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
