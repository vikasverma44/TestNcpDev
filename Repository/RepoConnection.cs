using SQLDataMaskingConfigurator.Helpers;
using SQLDataMaskingConfigurator.Infrastructure;

namespace SQLDataMaskingConfigurator.Repository
{
    internal partial class RepoConnection : IRepoConnection
    {
        private readonly DbService dbService;
        private readonly ConfigHelper ConfigHelper;
        private readonly Logger Logger;

        public RepoConnection(ConfigHelper configHelper, Logger logger)
        {
            ConfigHelper = configHelper;
            Logger = logger;
            dbService = new DbService(ConfigHelper, Logger);
        }

        #region Database Form Connection

        public string GetDataSourceString(string _dataSource, string _port)
        {
            string dataSource;
            if (!string.IsNullOrEmpty(_port))
            {
                dataSource = ($"{ _dataSource},{ _port}");
            }
            else
            {
                dataSource = ($"{ _dataSource}");
            }

            return dataSource;
        }
        public string GetConnectionStringCreated(string dataSource, bool hasWindowAuth, string txtDatabase, string txtUser, string txtPassword)
        {
            string connectionString;
            if (hasWindowAuth)
            {
                connectionString = ($"Server={dataSource};Database={txtDatabase};Integrated Security=True;");
            }
            else
            {
                connectionString = ($"Server={dataSource};Database={txtDatabase};User Id={txtUser};Password={txtPassword};");
            }

            return connectionString;
        }
        public bool ValidateDbConnection(string connectionString, string txtDatabase)
        {
            return dbService.ValidateDatabaseConnection(connectionString, txtDatabase);
        }

        #endregion

    }
}
