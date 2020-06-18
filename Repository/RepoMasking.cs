using SQLDataMaskingConfigurator.Helpers;
using SQLDataMaskingConfigurator.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace SQLDataMaskingConfigurator.Repository
{
    internal partial class RepoMasking : IRepoMasking
    {
        private readonly DbService dbService;
        private readonly ConfigHelper ConfigHelper;
        private readonly Logger Logger;

        public RepoMasking(ConfigHelper configHelper, Logger logger)
        {
            ConfigHelper = configHelper;
            Logger = logger;
            dbService = new DbService(ConfigHelper, Logger);
        }

        #region SQLDataMaskingConfigurator Form

        public DataSet GetTablesData(List<string> _TableListToFetchData, bool IsCountOnly = false)
        {
            return dbService.GetTablesData(_TableListToFetchData, true);
        }

        public DataTable GetTableColumnDetails(string TableNameWithSchema)
        {
            return dbService.GetTableColumnDetails(TableNameWithSchema);
        }

        public DataTable GetTableReferenceDetails(string TableNameWithSchema)
        {
            return dbService.GetTableReferenceDetails(TableNameWithSchema);
        }

        public DataTable GetTableDependentWithDetails(string TableNameWithSchema)
        {
            return dbService.GetTableDependentWithDetails(TableNameWithSchema);
        }

        public DataTable GetTableConstraintsDetails(string TableNameWithSchema)
        {
            return dbService.GetTableConstraintsDetails(TableNameWithSchema);
        }

        public Dictionary<string, string> GetListTables()
        {
            return dbService.GetListTables();
        }

        public DataTable GetTableData(string TableNameWithSchema, bool CountOnly = false, bool IsMaskedCount = false, bool IsMaskedValue = false)
        {
            return dbService.GetTableData(TableNameWithSchema, CountOnly, IsMaskedCount, IsMaskedValue);
        }

        public bool AddOrResetMaskColumnInTable(List<string> TableListToFetchData, bool WithUpdateSqlCommand = true, bool HasToReset = true, bool HasToPushMaskingFunctionInDb = false)
        {
            return dbService.AddOrResetMaskColumnInTable(TableListToFetchData, WithUpdateSqlCommand, HasToReset, HasToPushMaskingFunctionInDb);
        }

        public bool PushDataIntoDb(string tableName, Dictionary<string, string> columnsToMask, int batchSize = 0)
        {
            return dbService.PushDataIntoDb(tableName, columnsToMask, batchSize);
        }

        public void BindGridViewDefault(DataTable dtSource, DataGridView sourceGrid, DataGridViewAutoSizeColumnsMode columnFillType = DataGridViewAutoSizeColumnsMode.Fill, bool hasSortingAllowed = true)
        {
            if (dtSource != null)
            {
                sourceGrid.AutoGenerateColumns = true;
                sourceGrid.AutoSizeColumnsMode = columnFillType;
                DataView defaultView = dtSource.DefaultView;
                BindingSource dataSource = new BindingSource(defaultView, null);
                sourceGrid.DataSource = dataSource;
                if (!hasSortingAllowed)
                {
                    sourceGrid.Columns.Cast<DataGridViewColumn>()
                        .ToList()
                        .ForEach(f =>
                        {
                            f.SortMode = DataGridViewColumnSortMode.NotSortable;
                        });
                }
                sourceGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                sourceGrid.EnableHeadersVisualStyles = false;
                sourceGrid.ClearSelection();
            }
        }

        public Dictionary<string, string> GetTablesDictionary()
        {
            Dictionary<string, string> tableList = new Dictionary<string, string> { { Constants.Selector, Constants.Selector } };
            foreach (KeyValuePair<string, string> t in GetListTables())
            {
                tableList.Add(t.Key.ToString(), t.Value.ToString());
            }
            return tableList;
        }

        #endregion



    }
}
