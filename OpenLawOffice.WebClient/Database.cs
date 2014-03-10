namespace OpenLawOffice.WebClient
{
    using ServiceStack.OrmLite;
    using System.Data;

    public class Database : OpenLawOffice.Common.Singleton<Database>
    {
        private OrmLiteConnectionFactory _factory;

        public Database()
        {
            _factory = new OrmLiteConnectionFactory(Settings.Instance.PostgresConnectionString,
                ServiceStack.OrmLite.PostgreSQL.PostgreSQLDialectProvider.Instance);
        }

        public IDbConnection OpenConnection()
        {
            return _factory.OpenDbConnection();
        }
    }
}