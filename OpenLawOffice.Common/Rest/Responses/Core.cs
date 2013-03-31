namespace OpenLawOffice.Common.Rest.Responses
{
    public abstract class Core : ResponseBaseWithDatesOnly
    {
        public Security.User CreatedBy { get; set; }
        public Security.User ModifiedBy { get; set; }
        public Security.User DisabledBy { get; set; }
    }
}
