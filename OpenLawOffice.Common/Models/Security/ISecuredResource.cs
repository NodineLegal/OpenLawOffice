namespace OpenLawOffice.Common.Models.Security
{
    using System;

    public interface ISecuredResource
    {
        Guid? Id { get; set; }

        SecuredResource SecuredResource { get; set; }
    }
}