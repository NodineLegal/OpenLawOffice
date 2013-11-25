namespace OpenLawOffice.WinClient.Consumers.Tagging
{
    public class TagCategory
        : ConsumerBase<Common.Rest.Requests.Tagging.TagCategory, Common.Rest.Responses.Tagging.TagCategory>
    {
        public override string Resource { get { return "Tagging/Categories/"; } }
    }
}
