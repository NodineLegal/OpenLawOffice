using System;

namespace OpenLawOffice.Server.Core
{
    public class InvalidObjectException : Exception
    {
        public InvalidObjectException(string message)
            : base(message)
        {
        }
    }
}
