using SQLDataMaskingConfigurator.Helpers;
using SQLDataMaskingConfigurator.Infrastructure;
using System.Collections.Generic;
using System.Data;

namespace SQLDataMaskingConfigurator.Repository
{
    internal partial class RepoViewData : IRepoViewData
    {
        private readonly DbService dbService;
        private readonly ConfigHelper ConfigHelper;
        private readonly Logger Logger;

        public RepoViewData(ConfigHelper configHelper, Logger logger)
        {
            ConfigHelper = configHelper;
            Logger = logger;
            dbService = new DbService(ConfigHelper, Logger);
        }

        #region ViewTableData Form

        public DataTable GetTableData(string selectedTable, int currentPageIndex, int pgSize)
        {
            return dbService.GetTableData(selectedTable, currentPageIndex, pgSize);
        }
        public int GetTotalCount(string selectedTable)
        {
            return dbService.GetTotalCount(selectedTable);
        }
        public Dictionary<string, string> GetTablesDictionary()
        {
            Dictionary<string, string> tableList = new Dictionary<string, string> { { Constants.Selector, Constants.Selector } };
            foreach (KeyValuePair<string, string> t in dbService.GetListTables())
            {
                tableList.Add(t.Key.ToString(), t.Value.ToString());
            }
            return tableList;
        }

        #endregion

    }
}
