using System;

namespace OpenLawOffice.WinClient.Consumers
{
    public class InvalidServiceTargetException : Exception
    {
        public InvalidServiceTargetException(string message)
            : base(message)
        {
        }
    }
}
