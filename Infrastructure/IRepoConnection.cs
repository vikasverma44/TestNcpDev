namespace SQLDataMaskingConfigurator.Infrastructure
{
    interface IRepoConnection
    {
        #region Database Form Connection
        string GetDataSourceString(string _dataSource, string _port);
        string GetConnectionStringCreated(string dataSource, bool hasWindowAuth, string txtDatabase, string txtUser, string txtPassword);
        bool ValidateDbConnection(string connectionString, string txtDatabase);

        #endregion
    }
}
