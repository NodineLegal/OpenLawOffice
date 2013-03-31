namespace OpenLawOffice.Server.Core
{
    public class Settings : Singleton<Settings>
    {
        //public string AddinDirectory { get; set; }
        public string PostgresConnectionString { get; set; }

        public Settings()
        {
            //AddinDirectory = @"F:\OpenLawOffice\OpenLawOffice.Server\build\Addins";
            PostgresConnectionString = "server=localhost;Port=5432;Database=openlawoffice_1;User Id=postgres;Password=775j79d;";
        }
    }
}
