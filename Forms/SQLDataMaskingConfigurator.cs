using SQLDataMaskingConfigurator.Helpers;
using SQLDataMaskingConfigurator.Infrastructure;
using SQLDataMaskingConfigurator.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLDataMaskingConfigurator.Forms
{
    public partial class SQLDataMaskingConfigurator : Form
    {
        #region Private Class Members 

        private readonly Logger Logger;
        private readonly ConfigHelper configHelper;
        private readonly IRepoMasking repoMasking;

        private bool HasViewRefreshed;
        private readonly ConcurrentQueue<string> SqlCommandsQueue = new ConcurrentQueue<string>();
        private readonly List<string> TableListToFetchData = new List<string>();
        private readonly List<string> TableColumnsToMask = new List<string>();
        private readonly Dictionary<string, string> TableColumnsToMaskDictionary = new Dictionary<string, string>();
        private DataSet OriginalDataSetToMask;
        private int TotalRecordCount = 0;
        private readonly System.Timers.Timer TimerProgressUpdator;

        private string ServerName { get; set; }
        private string DatabaseName { get; set; }
        private string TableName { get; set; }

        #endregion

        #region Form Load & Constructor

        public SQLDataMaskingConfigurator()
        {
            InitializeComponent();
            configHelper = new ConfigHelper();
            Logger = new Logger(configHelper);
            repoMasking = new RepoMasking(configHelper, Logger);

            AssemblyName asm = Assembly.GetExecutingAssembly().GetName();
            string version = string.Format("{0}.{1}", asm.Version.Major, asm.Version.Minor);
            Text = string.Format("SQL Data Masking Configurator (v{0})", version);
            TimerProgressUpdator = new System.Timers.Timer();
        }
        private void SQLDataMaskingConfigurator_Load(object sender, EventArgs e)
        {
            SetControlsVisibility();
            ResetGlobalObjectsForDataMasking();
            GetSqlMaskingConnection();
            cmbTable.ContextMenu = new ContextMenu();
        }


        #endregion

        #region Form Control Events

        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (Utility.ShowConfirmationBox("Are you sure you want to close this application?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Close();
            }
        }
        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
        private void btnDatabase_Click(object sender, EventArgs e)
        {
            GetSqlMaskingConnection();
        }
        private void txtTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Utility.AllowOnlyNumericEntry(e.KeyChar);
        }
        private void txtFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Utility.AllowOnlyNumericEntry(e.KeyChar);
        }
        private void cmbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = ((KeyValuePair<string, string>)cmbTable.SelectedItem).Key;
            if (selectedTable != Constants.Selector)
            {
                HasViewRefreshed = false;
                BindDataGridViews(selectedTable);
                TableName = selectedTable;
                BindConnectionLabelDescription();
            }
            else
            {
                ResetGlobalObjectsForDataMasking();
            }
        }
        private void cmbTable_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
        private void dgvConstraints_SelectionChanged(object sender, EventArgs e)
        {
            dgvConstraints.ClearSelection();
        }
        private void dgvDependent_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDependent.SelectedRows.Count > 0 && HasViewRefreshed)
            {
                string selectedTable = Convert.ToString(dgvDependent.SelectedRows[0].Cells["REF_TABLE"].Value);
                if (selectedTable != Constants.Selector)
                {
                    cmbTable.SelectedValue = selectedTable;
                }
            }
        }
        private void dgvReferences_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReferences.SelectedRows.Count > 0 && HasViewRefreshed)
            {
                string selectedTable = Convert.ToString(dgvReferences.SelectedRows[0].Cells["TABLE"].Value);
                if (selectedTable != Constants.Selector)
                {
                    cmbTable.SelectedValue = selectedTable;
                }
            }
        }
        private void SQLDataMaskingConfigurator_Resize(object sender, EventArgs e)
        {
            //lblProgress.Top = progressBar.Top + ((progressBar.Height - lblProgress.Height) / 2);
            lblProgress.Top = progressBar.Top - lblProgress.Height;
            lblProgress.Left = progressBar.Left + ((progressBar.Width - lblProgress.Width) / 2);
        }
        private void dgvConfiguration_SelectionChanged(object sender, EventArgs e)
        {
            ProcessGridColumnSelection();
        }
        private void btnViewData_Click(object sender, EventArgs e)
        {
            using (ViewTableData viewTableData = new ViewTableData(configHelper, Logger))
            {
                viewTableData.currentTable = TableName;
                viewTableData.ShowDialog();
            }
        }
        private void btnMask_Click(object sender, EventArgs e)
        {
            SqlDataMaskingProcess();
        }
        private void TimerProgressUpdator_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UpdateProgressBarVirtually();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "TimerProgressUpdator_Elapsed");
            }
        }


        #endregion

        #region Private Methods

        private void SqlDataMaskingProcess()
        {
            try
            {
                if (dgvConfiguration.SelectedRows.Count < 1) { Utility.ShowMessageBox("Please select column(s) for data masking.", MessageBoxButtons.OK); return; }

                if (Utility.ShowConfirmationBox("Are you sure you want to start the data masking process? " + Environment.NewLine + "This action cannot be undone.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bool HasToResetAllRecords = false;

                    /////14Apr20: As discussed Data Masking Operation would only be in Forward Only Mode.

                    //if (Utility.ShowConfirmationBox(@"Press [YES] if you want to perform data masking operation for All Records" + Environment.NewLine + Environment.NewLine +
                    //            "Else press [NO] for the New or Un-Processed Records only.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //{
                    //    HasToResetAllRecords = true;
                    //}

                    int TableProcessingSize = configHelper.Model.RecordProcessingBatchSize;
                    bool IsErrorOccurred = false;
                    bool IsCompleteMessage = true;

                    Application.UseWaitCursor = true;
                    UseWaitCursor = true;
                    TimerProgressUpdator.Elapsed += new System.Timers.ElapsedEventHandler(TimerProgressUpdator_Elapsed);
                    TimerProgressUpdator.Enabled = true;
                    TimerProgressUpdator.AutoReset = true;
                    TimerProgressUpdator.Interval = (5 * 1000) * TableColumnsToMask.Count();
                    TimerProgressUpdator.Start();

                    ResetGlobalObjectsForDataMasking(true);
                    BlockFormWindow(false, false);
                    Task.Run(() =>
                    {
                        foreach (string _currentTable in TableListToFetchData)
                        {
                            if (!HasToResetAllRecords)
                            {
                                DataTable dtTable = repoMasking.GetTableData(TableNameWithSchema: _currentTable, CountOnly: true, IsMaskedCount: true, IsMaskedValue: false);

                                if (dtTable != null && dtTable.Rows.Count > 0)
                                {
                                    TotalRecordCount = Convert.ToInt32(dtTable.Rows[0][0]);
                                    if (TotalRecordCount < 1)
                                    {
                                        Utility.ShowMessageBox("Data masking has already been performed on the selected table. Please select different table to perform masking.", MessageBoxButtons.OK);
                                        IsCompleteMessage = false;
                                    }
                                }
                            }
                            BindProgressStatus(0, HasToReset: true, InThread: true, Message: "Configuring initializtion: ");
                            IsErrorOccurred = !repoMasking.AddOrResetMaskColumnInTable(
                                TableListToFetchData: new List<string>() { _currentTable },
                                WithUpdateSqlCommand: true,
                                HasToReset: HasToResetAllRecords,
                                HasToPushMaskingFunctionInDb: true);

                            if (!IsErrorOccurred)
                            {
                                for (int i = 0; i < (TotalRecordCount / configHelper.Model.RecordProcessingBatchSize) + 1; i++)
                                {
                                    //Data Masking Table data push into DB
                                    IsErrorOccurred = !repoMasking.PushDataIntoDb(_currentTable, TableColumnsToMaskDictionary, configHelper.Model.RecordProcessingBatchSize);

                                    if (IsErrorOccurred)
                                    { break; }
                                }
                            }
                        }
                        if (IsErrorOccurred)
                        {
                            Application.UseWaitCursor = false;
                            UseWaitCursor = false;
                            TimerProgressUpdator.Enabled = false;
                            TimerProgressUpdator.AutoReset = false;
                            TimerProgressUpdator.Stop();

                            Utility.ShowErrorMessageBox("Error occurred while data masking, kindly check error log for more details.");
                            ResetProgressBarObjectsForDataMaskingInThread();
                            BlockFormWindow(true, true);
                        }
                        else
                        {
                            BindProgressStatus(0, true, true, "Data Masking completed:");
                            Application.UseWaitCursor = false;
                            UseWaitCursor = false;
                            TimerProgressUpdator.Enabled = false;
                            TimerProgressUpdator.AutoReset = false;
                            TimerProgressUpdator.Stop();

                            if (IsCompleteMessage)
                            {
                                if (Utility.ShowMessageBox("Data masking successfully completed.", MessageBoxButtons.OK) == DialogResult.OK)
                                {
                                    BlockFormWindow(true, true);
                                }
                            }
                            else { BlockFormWindow(true, true); }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "btnStartMasking_Click");
                Utility.ShowErrorMessageBox("Error occurred while data masking, kindly check error log for more details.");
            }
        }
        private void SetControlsVisibility()
        {
            lblDatabaseConnection.Text = string.Empty;
            pnlDetails.Enabled = false;
            lblProgress.Visible = false;
            lnkLabelRecordCount.Visible = false;
            dgvConfiguration.DataSource = null;
            dgvReferences.DataSource = null;
            dgvDependent.DataSource = null;
            dgvConstraints.DataSource = null;
        }
        private void GetSqlMaskingConnection()
        {
            using (FrmDatabase frmDatabase = new FrmDatabase(configHelper, Logger))
            {
                DialogResult dialogResult = frmDatabase.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    SetControlsVisibility();
                    ServerName = frmDatabase.ServerName;
                    DatabaseName = frmDatabase.DatabaseName;
                    BindConnectionLabelDescription();

                    cmbTable.DataSource = new BindingSource(repoMasking.GetTablesDictionary(), null);
                    cmbTable.DisplayMember = "Key";
                    cmbTable.ValueMember = "Key";
                }
            }
        }
        private void ProcessGridColumnSelection()
        {
            ResetGlobalObjectsForDataMasking();
            if (dgvConfiguration.SelectedRows.Count > 0 && !string.IsNullOrEmpty(TableName))
            {
                if (!TableListToFetchData.Contains(TableName))
                {
                    TableListToFetchData.Add(TableName);
                }
                foreach (DataGridViewRow dr in dgvConfiguration.SelectedRows)
                {
                    string selectedColumn = Convert.ToString(dr.Cells["COLUMN_NAME"].Value).Trim().ToUpper();
                    string selectedColumnDataType = Convert.ToString(dr.Cells["DATA_TYPE"].Value).Trim().ToUpper();

                    if (!TableColumnsToMask.Contains(selectedColumn)) { TableColumnsToMask.Add(selectedColumn); }
                    if (!TableColumnsToMaskDictionary.Keys.Contains(selectedColumn)) { TableColumnsToMaskDictionary.Add(selectedColumn, selectedColumnDataType); }

                    ///// If Required DataMasking for Child Tables References
                    //foreach (DataGridViewRow drRef in dgvReferences.Rows)
                    //{
                    //    string refColumn = Convert.ToString(drRef.Cells["COLUMN"].Value).Trim().ToUpper();
                    //    if (selectedColumn.Equals(refColumn))
                    //    {
                    //        string refTableName = Convert.ToString(drRef.Cells["TABLE"].Value).Trim().ToUpper();
                    //        if (!TableListToFetchData.Contains(refTableName))
                    //        {
                    //            TableListToFetchData.Add(refTableName);
                    //        }
                    //    }
                    //}
                }
                lnkLabelRecordCount.Visible = true;
                lnkLabelRecordCount.Text = string.Format("Total selected column(s): {0}", TableColumnsToMask.Count());
                toolTipColumns.SetToolTip(lnkLabelRecordCount, string.Format("{0}", string.Join(", ", TableColumnsToMask)));

                Task.Run(() =>
                {
                    OriginalDataSetToMask = repoMasking.GetTablesData(TableListToFetchData, true);
                    if (OriginalDataSetToMask != null && OriginalDataSetToMask.Tables.Count > 0 && OriginalDataSetToMask.Tables[0].Rows.Count > 0)
                    {
                        TotalRecordCount = Convert.ToInt32(OriginalDataSetToMask.Tables[0].Rows[0][0]);
                        BindProgressStatus(0, InThread: true);
                    }
                });
            }
        }
        private void UpdateProgressBarVirtually()
        {
            double timerIntervalInMilliseconds = TimerProgressUpdator.Interval;
            int networkLatencyPerMilliSecond = TotalRecordCount / 1000000;
            int processingColumnarLatency = networkLatencyPerMilliSecond * TableColumnsToMask.Count;

            int actualRecordProcessingPerMilliSecond = configHelper.Model.RecordProgressPerMilliSecond > processingColumnarLatency ?
                configHelper.Model.RecordProgressPerMilliSecond - processingColumnarLatency :
                configHelper.Model.RecordProgressPerMilliSecond;
            int progress = (actualRecordProcessingPerMilliSecond * (((int)timerIntervalInMilliseconds) / TableColumnsToMask.Count()));

            BindProgressStatus(progress, false, true, "Record Processing: ", false);
        }
        private void BindConnectionLabelDescription()
        {
            string _desc = string.Format("Server={0},      Database={1},      Table={2}", ServerName, DatabaseName, TableName);
            lblDatabaseConnection.Text = _desc;
            lblDatabaseConnection.BackColor = Color.LightGray;
            pnlDetails.Enabled = true;
            ResetGlobalObjectsForDataMasking();
        }
        private void BindDataGridViews(string SelectedTable)
        {
            DataTable dtColumns = repoMasking.GetTableColumnDetails(SelectedTable);
            repoMasking.BindGridViewDefault(dtColumns, dgvConfiguration);

            DataTable dtReferences = repoMasking.GetTableReferenceDetails(SelectedTable);
            repoMasking.BindGridViewDefault(dtReferences, dgvReferences, hasSortingAllowed: false);

            DataTable dtDependent = repoMasking.GetTableDependentWithDetails(SelectedTable);
            repoMasking.BindGridViewDefault(dtDependent, dgvDependent, hasSortingAllowed: false);

            DataTable dtConstraints = repoMasking.GetTableConstraintsDetails(SelectedTable);
            repoMasking.BindGridViewDefault(dtConstraints, dgvConstraints);
            dgvConstraints.Columns["CONSTRAINT_NAME"].Visible = false;

            HasViewRefreshed = true;
        }
        private void BlockFormWindow(bool InThread, bool IsEnable)
        {
            if (InThread)
            {
                btnDatabase.Invoke((Action)(() => btnDatabase.Enabled = IsEnable));
                cmbTable.Invoke((Action)(() => cmbTable.Enabled = IsEnable));
                dgvConfiguration.Invoke((Action)(() => dgvConfiguration.Enabled = IsEnable));
                dgvReferences.Invoke((Action)(() => dgvReferences.Enabled = IsEnable));
                dgvDependent.Invoke((Action)(() => dgvDependent.Enabled = IsEnable));
                dgvConstraints.Invoke((Action)(() => dgvConstraints.Enabled = IsEnable));
                btnViewData.Invoke((Action)(() => btnViewData.Enabled = IsEnable));
                btnMask.Invoke((Action)(() => btnMask.Enabled = IsEnable));
                btnAbout.Invoke((Action)(() => btnAbout.Enabled = IsEnable));
                btnQuit.Invoke((Action)(() => btnQuit.Enabled = IsEnable));

                lblProgress.Invoke((Action)(() => lblProgress.Enabled = IsEnable));
                progressBar.Invoke((Action)(() => progressBar.Enabled = IsEnable));
                if (IsEnable)
                {
                    cmbTable.Invoke((Action)(() => cmbTable.SelectedValue = Constants.Selector));
                    dgvConfiguration.Invoke((Action)(() => { dgvConfiguration.ClearSelection(); dgvConfiguration.DataSource = null; }));
                    dgvReferences.Invoke((Action)(() => { dgvReferences.ClearSelection(); dgvReferences.DataSource = null; }));
                    dgvDependent.Invoke((Action)(() => { dgvDependent.ClearSelection(); dgvDependent.DataSource = null; }));
                    dgvConstraints.Invoke((Action)(() => { dgvConstraints.ClearSelection(); dgvConstraints.DataSource = null; }));
                }
            }
            else
            {
                btnDatabase.Enabled = IsEnable;
                cmbTable.Enabled = IsEnable;
                dgvConfiguration.Enabled = IsEnable;
                dgvReferences.Enabled = IsEnable;
                dgvDependent.Enabled = IsEnable;
                dgvConstraints.Enabled = IsEnable;
                btnViewData.Enabled = IsEnable;
                btnMask.Enabled = IsEnable;
                btnAbout.Enabled = IsEnable;
                btnQuit.Enabled = IsEnable;

                lblProgress.Enabled = !IsEnable;
                progressBar.Enabled = !IsEnable;
                if (IsEnable)
                {
                    cmbTable.SelectedValue = Constants.Selector;
                    dgvConfiguration.ClearSelection(); dgvConfiguration.DataSource = null;
                    dgvReferences.ClearSelection(); dgvReferences.DataSource = null;
                    dgvDependent.ClearSelection(); dgvDependent.DataSource = null;
                    dgvConstraints.ClearSelection(); dgvConstraints.DataSource = null;
                }
            }
        }
        private void ResetProgressBarObjectsForDataMaskingInThread()
        {
            progressBar.Invoke((Action)(() =>
            {
                progressBar.Step = 1;
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
            }));
            lblProgress.Invoke((Action)(() => lblProgress.Visible = false));
            //lnkLabelRecordCount.Invoke((Action)(() => lnkLabelRecordCount.Visible = false));
        }
        private void ResetGlobalObjectsForDataMasking(bool IsOnlyProgressBarRelated = false)
        {
            if (IsOnlyProgressBarRelated)
            {
                progressBar.Step = 1;
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
                lblProgress.Visible = false;
            }
            else
            {
                if (OriginalDataSetToMask != null)
                {
                    OriginalDataSetToMask.Clear(); OriginalDataSetToMask.Dispose(); OriginalDataSetToMask = null;
                }
                TableListToFetchData.Clear();
                TableColumnsToMask.Clear();
                TableColumnsToMaskDictionary.Clear();
                TotalRecordCount = 0;

                progressBar.Step = 1;
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
                lblProgress.Visible = false;
                lnkLabelRecordCount.Visible = false;
            }
        }
        private void BindProgressStatus(int ProgressRecords, bool HasProcessCompleted = false, bool InThread = true, string Message = "", bool HasToReset = false)
        {
            TotalRecordCount = TotalRecordCount <= 1 ? 1 : TotalRecordCount;
            ProgressRecords = ProgressRecords <= 0 ? 0 : (ProgressRecords > TotalRecordCount ? TotalRecordCount : ProgressRecords);

            if (InThread)
            {
                progressBar.Invoke((Action)(() =>
                {
                    progressBar.Maximum = TotalRecordCount;
                    if (HasToReset) { progressBar.Value = 0; }
                }));
            }
            else
            {
                progressBar.Maximum = TotalRecordCount;
                if (HasToReset) { progressBar.Value = 0; }
            }
            if (InThread)
            {
                progressBar.Invoke((Action)(() => progressBar.Value += (progressBar.Value + ProgressRecords > progressBar.Maximum) ? 0 : ProgressRecords));
            }
            else
            {
                progressBar.Value += (progressBar.Value + ProgressRecords > progressBar.Maximum) ? 0 : ProgressRecords;
            }
            if (HasProcessCompleted)
            {
                if (InThread)
                {
                    progressBar.Invoke((Action)(() => progressBar.Value += (progressBar.Maximum - progressBar.Value)));
                }
                else
                {
                    progressBar.Value += (progressBar.Maximum - progressBar.Value);
                }
            }

            double PercentageCount = (((double)progressBar.Value * 100) / progressBar.Maximum);
            if (double.IsNaN(PercentageCount))
            {
                PercentageCount = 0;
            }
            if (PercentageCount > 1) { PercentageCount = (int)PercentageCount; }

            string _desc = string.Format("{0}/{1} ({2}%)", progressBar.Value, progressBar.Maximum, PercentageCount);
            if (!string.IsNullOrEmpty(Message))
            {
                _desc = string.Format("{0}" + _desc, Message + "     ");
            }
            if (InThread)
            {
                lblProgress.Invoke((Action)(() => { lblProgress.Visible = true; lblProgress.Text = _desc; }));
            }
            else
            {
                lblProgress.Visible = true; lblProgress.Text = _desc;
            }
        }

        #endregion

    }
}
