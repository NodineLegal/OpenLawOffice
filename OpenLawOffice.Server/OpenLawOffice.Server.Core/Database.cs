using System;
using System.Data;
using ServiceStack.OrmLite;

namespace OpenLawOffice.Server.Core
{
    public class Database : Singleton<Database>
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
