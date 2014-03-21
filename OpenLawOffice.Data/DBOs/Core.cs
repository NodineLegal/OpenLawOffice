using System.ComponentModel.DataAnnotations;

namespace OpenLawOffice.Data.DBOs
{
    public abstract class Core : DboWithDatesOnly
    {
        [ColumnMapping(Name = "created_by_user_id")]
        public int CreatedByUserId { get; set; }

        [ColumnMapping(Name = "modified_by_user_id")]
        public int ModifiedByUserId { get; set; }

        [ColumnMapping(Name = "disabled_by_user_id")]
        public int? DisabledByUserId { get; set; }
    }
}