namespace OpenLawOffice.WinClient.Consumers.Matters
{
    public class Matter
        : ConsumerBase<Common.Rest.Requests.Matters.Matter, Common.Rest.Responses.Matters.Matter>
    {
        public override string Resource { get { return "Matters/"; } }
    }
}
