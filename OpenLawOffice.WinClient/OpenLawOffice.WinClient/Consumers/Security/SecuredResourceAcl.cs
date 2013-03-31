namespace OpenLawOffice.WinClient.Consumers.Security
{
    public class SecuredResourceAcl
        : ConsumerBase<Common.Rest.Requests.Security.SecuredResourceAcl, Common.Rest.Responses.Security.SecuredResourceAcl>
    {
        public override string Resource { get { return "Security/SecuredResourceAcls/"; } }
    }
}
