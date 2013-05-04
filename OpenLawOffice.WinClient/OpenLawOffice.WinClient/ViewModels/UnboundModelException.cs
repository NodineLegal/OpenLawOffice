using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public class UnboundModelException : Exception
    {
        public UnboundModelException(string message)
            : base(message)
        {
        }
    }
}
