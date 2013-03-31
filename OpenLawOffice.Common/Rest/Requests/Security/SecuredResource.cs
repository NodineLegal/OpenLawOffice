using System;

namespace OpenLawOffice.Common.Rest.Requests.Security
{
    public class SecuredResource : RequestBase, IHasGuidId
    {
        public Guid? Id { get; set; }
    }
}
