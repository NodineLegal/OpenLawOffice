namespace OpenLawOffice.Common.Rest.Requests.Tagging
{
    public class TagCategory : RequestBase, IHasIntId
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
