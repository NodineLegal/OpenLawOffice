namespace OpenLawOffice.WinClient.Consumers.Security
{
    public class Authentication
        : ConsumerBase<Common.Rest.Requests.Security.Authentication, Common.Rest.Responses.Security.Authentication>
    {
        public override string Resource { get { return "Authentication/"; } }
    }
}
