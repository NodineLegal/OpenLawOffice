namespace OpenLawOffice.WinClient.Consumers.Security
{
    public class SecuredResource
        : ConsumerBase<Common.Rest.Requests.Security.SecuredResource, Common.Rest.Responses.Security.SecuredResource>
    {
        public override string Resource { get { return "Security/SecuredResources/"; } }
    }
}
