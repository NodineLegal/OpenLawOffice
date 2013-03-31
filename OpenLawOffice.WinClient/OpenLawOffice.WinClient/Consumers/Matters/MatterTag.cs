namespace OpenLawOffice.WinClient.Consumers.Matters
{
    public class MatterTag
        : ConsumerBase<Common.Rest.Requests.Matters.MatterTag, Common.Rest.Responses.Matters.MatterTag>
    {
        public override string Resource { get { return "Matters/MatterTags/"; } }
    }
}
