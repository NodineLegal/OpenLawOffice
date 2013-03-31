using System;

namespace OpenLawOffice.Common.Models
{
    public class CanAttribute : Attribute
    {
        public CanFlags Can { get; set; }

        public CanAttribute(CanFlags can)
        {
            Can = can;
        }
    }
}
