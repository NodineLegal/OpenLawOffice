namespace OpenLawOffice.Common.Rest.Requests.Matters
{
    public class MatterTagCategory : RequestBase, IHasIntId
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
