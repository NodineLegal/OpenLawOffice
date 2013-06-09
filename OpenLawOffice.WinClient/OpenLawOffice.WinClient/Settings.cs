namespace OpenLawOffice.WinClient
{
    public class Settings
    {
        public string HostBaseUrl { get; set; }

        public Settings()
        {
            HostBaseUrl = "http://localhost:12320/";
        }
    }
}
