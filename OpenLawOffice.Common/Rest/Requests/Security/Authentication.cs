namespace OpenLawOffice.Common.Rest.Requests.Security
{
    public class Authentication : RequestBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
