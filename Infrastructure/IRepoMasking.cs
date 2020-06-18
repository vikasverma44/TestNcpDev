using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SQLDataMaskingConfigurator.Infrastructure
{
    interface IRepoMasking
    {
        #region SQLDataMaskingConfigurator Form

        DataSet GetTablesData(List<string> _TableListToFetchData, bool IsCountOnly = false);

        DataTable GetTableColumnDetails(string TableNameWithSchema);

        DataTable GetTableReferenceDetails(string TableNameWithSchema);

        DataTable GetTableDependentWithDetails(string TableNameWithSchema);

        DataTable GetTableConstraintsDetails(string TableNameWithSchema);

        Dictionary<string, string> GetListTables();

        DataTable GetTableData(string TableNameWithSchema, bool CountOnly = false, bool IsMaskedCount = false, bool IsMaskedValue = false);

        bool AddOrResetMaskColumnInTable(List<string> TableListToFetchData, bool WithUpdateSqlCommand = true, bool HasToReset = true, bool HasToPushMaskingFunctionInDb = false);

        bool PushDataIntoDb(string tableName, Dictionary<string, string> columnsToMask, int batchSize = 0);

        void BindGridViewDefault(DataTable dtSource, DataGridView sourceGrid, DataGridViewAutoSizeColumnsMode columnFillType = DataGridViewAutoSizeColumnsMode.Fill, bool hasSortingAllowed = true);

        Dictionary<string, string> GetTablesDictionary();

        #endregion
    }
}
