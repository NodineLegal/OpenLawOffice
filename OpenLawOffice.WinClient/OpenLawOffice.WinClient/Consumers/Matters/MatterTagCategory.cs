namespace OpenLawOffice.WinClient.Consumers.Matters
{
    public class MatterTagCategory
        : ConsumerBase<Common.Rest.Requests.Matters.MatterTagCategory, Common.Rest.Responses.Matters.MatterTagCategory>
    {
        public override string Resource { get { return "Matters/MatterTagCategories/"; } }
    }
}
