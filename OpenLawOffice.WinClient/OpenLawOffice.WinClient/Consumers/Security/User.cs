namespace OpenLawOffice.WinClient.Consumers.Security
{
    public class User
        : ConsumerBase<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>
    {
        public override string Resource { get { return "Security/Users/"; } }
    }
}
