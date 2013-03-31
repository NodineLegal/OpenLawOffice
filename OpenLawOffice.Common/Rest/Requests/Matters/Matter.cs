namespace OpenLawOffice.Common.Rest.Requests.Matters
{
    public class Matter : RequestBase, IHasLongId
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TagQuery { get; set; }
    }
}
