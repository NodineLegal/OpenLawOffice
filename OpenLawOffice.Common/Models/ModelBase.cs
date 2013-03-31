namespace OpenLawOffice.Common.Models
{
    public abstract class ModelBase
    {
        public bool? IsStub { get; set; }

        public abstract void BuildMappings();
    }
}
