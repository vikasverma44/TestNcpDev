using System.Collections.Generic;
using System.Data;

namespace SQLDataMaskingConfigurator.Infrastructure
{
    interface IRepoViewData
    {
        #region ViewTableData Form

        DataTable GetTableData(string selectedTable, int currentPageIndex, int pgSize);
        int GetTotalCount(string selectedTable);
        Dictionary<string, string> GetTablesDictionary();

        #endregion
    }
}
