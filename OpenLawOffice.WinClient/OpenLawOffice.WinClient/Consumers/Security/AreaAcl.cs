namespace OpenLawOffice.WinClient.Consumers.Security
{
    public class AreaAcl
        : ConsumerBase<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
    {
        public override string Resource { get { return "Security/AreaAcls/"; } }
    }
}
