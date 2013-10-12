namespace OpenLawOffice.Server.Core
{
    public class Settings : Singleton<Settings>
    {
        //public string AddinDirectory { get; set; }
        public string PostgresConnectionString { get; set; }

        public Settings()
        {
            //AddinDirectory = @"F:\OpenLawOffice\OpenLawOffice.Server\build\Addins";
            PostgresConnectionString = "server=192.168.10.40;Port=5432;Database=openlawoffice;User Id=postgres;Password=postgres;";
        }
    }
}
