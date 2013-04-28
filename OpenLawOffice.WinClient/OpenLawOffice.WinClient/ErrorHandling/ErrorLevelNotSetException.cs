using System;

namespace OpenLawOffice.WinClient.ErrorHandling
{
    public class ErrorLevelNotSetException : Exception
    {
        public ErrorLevelNotSetException()
            : base("Must set the error level before attempting to log.")
        {
        }
    }
}
