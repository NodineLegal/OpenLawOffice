using System;

namespace OpenLawOffice.WinClient.ErrorHandling
{
    public class ErrorNotRegisteredException : Exception
    {
        public ErrorNotRegisteredException()
            : base("Errors must be created using the ErrorManager.Create method.")
        {
        }
    }
}
