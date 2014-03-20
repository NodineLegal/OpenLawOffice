// -----------------------------------------------------------------------
// <copyright file="Database.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.Data
{
    using System.Data;
    using Npgsql;
    using System.Data.Common;
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Database : Common.Singleton<Database>
    {
        private DbProviderFactory _factory;

        public Database()
        {
            _factory = DbProviderFactories.GetFactory("Npgsql");
        }

        public IDbConnection GetConnection()
        {
            IDbConnection conn = _factory.CreateConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Postgres"].ConnectionString;
            return conn;
        }
    }
}
