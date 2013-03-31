namespace OpenLawOffice.WinClient.Consumers.Security
{
    public class Area 
        : ConsumerBase<Common.Rest.Requests.Security.Area, Common.Rest.Responses.Security.Area>
    {
        public override string Resource { get { return "Security/Areas/"; } }
    }
}
