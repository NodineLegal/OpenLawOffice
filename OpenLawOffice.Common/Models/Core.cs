namespace OpenLawOffice.Common.Models
{
    public abstract class Core : ModelWithDatesOnly
    {
        public Security.User CreatedBy { get; set; }

        public Security.User ModifiedBy { get; set; }

        public Security.User DisabledBy { get; set; }
    }
}