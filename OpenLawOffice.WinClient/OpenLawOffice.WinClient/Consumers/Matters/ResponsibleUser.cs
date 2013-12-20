namespace OpenLawOffice.WinClient.Consumers.Matters
{
    public class ResponsibleUser
        : ConsumerBase<Common.Rest.Requests.Matters.ResponsibleUser, Common.Rest.Responses.Matters.ResponsibleUser>
    {
        public override string Resource { get { return "Matters/ResponsibleUsers/"; } }
    }
}