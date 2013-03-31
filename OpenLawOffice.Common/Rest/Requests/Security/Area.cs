namespace OpenLawOffice.Common.Rest.Requests.Security
{
    public class Area : RequestBase, IHasIntId
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool? ShowAll { get; set; }
    }
}
