using System;

namespace OpenLawOffice.Common.Rest.Requests.Matters
{
    public class ResponsibleUser : RequestBase
    {
        public int? Id { get; set; }
        public Guid? MatterId { get; set; }
        public int? UserId { get; set; }
        public string Responsibility { get; set; }
    }
}
