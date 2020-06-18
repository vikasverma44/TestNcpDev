using SQLDataMaskingConfigurator.Helpers;
using SQLDataMaskingConfigurator.Infrastructure;
using SQLDataMaskingConfigurator.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLDataMaskingConfigurator.Forms
{
    internal partial class ViewTableData : Form
    {
        private readonly IRepoViewData repoViewData;
        private readonly ConfigHelper configHelper;
        private readonly Logger logger;
        private int currentPageIndex = 1;
        private int totalPage = 0;
        private int pgSize = 0;
        public string currentTable { get; set; }

        public ViewTableData(ConfigHelper configHelper, Logger logger)
        {
            InitializeComponent();
            this.configHelper = configHelper;
            this.logger = logger;
            repoViewData = new RepoViewData(configHelper, logger);
        }
        private void ViewTableData_Load(object sender, EventArgs e)
        {
            SetPagingControls(false, false, false, false);
            cmbViewTable.DataSource = new BindingSource(repoViewData.GetTablesDictionary(), null);
            cmbViewTable.DisplayMember = "Key";
            cmbViewTable.ValueMember = "Key";
            cmbPgSize.DataSource = new BindingSource(GetPageSizeDictionary(), null);
            cmbPgSize.DisplayMember = "Value";
            cmbPgSize.ValueMember = "Key";

            Task task = Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(currentTable))
                {

                    cmbViewTable.Invoke((Action)(() =>
                    {
                        cmbViewTable.Enabled = false;
                        cmbViewTable.SelectedValue = currentTable;
                        cmbViewTable.Enabled = true;
                    }));
                }
            });

            dgvTableData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            cmbViewTable.ContextMenu = new ContextMenu();
            cmbPgSize.ContextMenu = new ContextMenu();
        }
        private void cmbViewTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSelectedIndexValue();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            try
            {
                Application.UseWaitCursor = true;
                UseWaitCursor = true;
                SetPagingControls(false, false, true, true);
                currentPageIndex = 1;
                string selectedTable = ((KeyValuePair<string, string>)cmbViewTable.SelectedItem).Key;
                if (selectedTable != Constants.Selector)
                {
                    dgvTableData.DataSource = repoViewData.GetTableData(selectedTable, currentPageIndex, pgSize);
                    lblPages.Text = "Page " + currentPageIndex + " of " + totalPage + " Page(s)";
                }
            }
            finally
            {
                Application.UseWaitCursor = false;
                UseWaitCursor = false;
            }
        }
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            try
            {
                Application.UseWaitCursor = true;
                UseWaitCursor = true;
                string selectedTable = ((KeyValuePair<string, string>)cmbViewTable.SelectedItem).Key;
                if (selectedTable != Constants.Selector)
                {
                    if (currentPageIndex < totalPage)
                    {
                        currentPageIndex++;
                        bool isLastPage = currentPageIndex == totalPage;
                        SetPagingControls(true, true, !isLastPage, !isLastPage);
                        dgvTableData.DataSource = repoViewData.GetTableData(selectedTable, currentPageIndex, pgSize);
                        lblPages.Text = "Page " + currentPageIndex + " of " + totalPage + " Page(s)";
                    }
                }
            }
            finally
            {
                Application.UseWaitCursor = false;
                UseWaitCursor = false;
            }
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                Application.UseWaitCursor = true;
                UseWaitCursor = true;
                string selectedTable = ((KeyValuePair<string, string>)cmbViewTable.SelectedItem).Key;
                if (selectedTable != Constants.Selector)
                {
                    if (currentPageIndex > 1)
                    {
                        currentPageIndex--;
                        bool isFirstPage = currentPageIndex == 1;
                        SetPagingControls(!isFirstPage, !isFirstPage, true, true);

                        dgvTableData.DataSource = repoViewData.GetTableData(selectedTable, currentPageIndex, pgSize);
                        lblPages.Text = "Page " + currentPageIndex + " of " + totalPage + " Page(s)";
                    }
                }
            }
            finally
            {
                Application.UseWaitCursor = false;
                UseWaitCursor = false;
            }
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            try
            {
                Application.UseWaitCursor = true;
                UseWaitCursor = true;
                SetPagingControls(true, true, false, false);
                currentPageIndex = totalPage;
                string selectedTable = ((KeyValuePair<string, string>)cmbViewTable.SelectedItem).Key;
                if (selectedTable != Constants.Selector)
                {
                    dgvTableData.DataSource = repoViewData.GetTableData(selectedTable, currentPageIndex, pgSize);
                    lblPages.Text = "Page " + currentPageIndex + " of " + totalPage + " Page(s)";
                }
            }
            finally
            {
                Application.UseWaitCursor = false;
                UseWaitCursor = false;
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void cmbPgSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgSize = Convert.ToInt32(((KeyValuePair<string, string>)cmbPgSize.SelectedItem).Value);
            SetSelectedIndexValue();
        }
        private void cmbViewTable_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
        private void cmbPgSize_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void SetSelectedIndexValue()
        {
            try
            {
                Application.UseWaitCursor = true;
                UseWaitCursor = true;
                string selectedTable = ((KeyValuePair<string, string>)cmbViewTable.SelectedItem).Key;
                currentPageIndex = 1;
                if (selectedTable != Constants.Selector)
                {
                    dgvTableData.Invoke((Action)(() => { dgvTableData.DataSource = repoViewData.GetTableData(selectedTable, currentPageIndex, pgSize); }));
                    int totalReordCount = repoViewData.GetTotalCount(selectedTable);
                    totalPage = GetTotalPage(totalReordCount);
                    lblPages.Invoke((Action)(() => { lblPages.Text = "Page " + currentPageIndex + " of " + totalPage + " Page(s)"; }));
                    lblTotalRecords.Invoke((Action)(() => { lblTotalRecords.Text = "Total Record(s):" + Convert.ToString(totalReordCount); }));
                    bool isTotalPageOne = totalPage < 2 ? true : false;
                    SetPagingControls(false, false, !isTotalPageOne, !isTotalPageOne);
                }
            }
            finally
            {
                Application.UseWaitCursor = false;
                UseWaitCursor = false;
            }
        }
        private Dictionary<string, string> GetPageSizeDictionary()
        {
            Dictionary<string, string> PgSize = new Dictionary<string, string>
            {
                { "1", "15" },
                { "2", "30" },
                { "3", "50" },
                { "4", "100" },
                { "5", "500" }
            };

            return PgSize;
        }
        private int GetTotalPage(int intCount)
        {
            int totalPage = intCount / pgSize;
            // if any row left after calculated pages, add one more page 
            if (intCount % pgSize > 0)
            {
                totalPage += 1;
            }

            return totalPage;
        }
        private void SetPagingControls(bool first, bool previous, bool next, bool last)
        {
            btnFirstPage.Invoke((Action)(() => { btnFirstPage.Enabled = first; }));
            btnPrevious.Invoke((Action)(() => { btnPrevious.Enabled = previous; }));
            btnNextPage.Invoke((Action)(() => { btnNextPage.Enabled = next; }));
            btnLastPage.Invoke((Action)(() => { btnLastPage.Enabled = last; }));
        }
    }
}
