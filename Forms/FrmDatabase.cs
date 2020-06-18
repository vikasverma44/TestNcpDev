using SQLDataMaskingConfigurator.Helpers;
using SQLDataMaskingConfigurator.Infrastructure;
using SQLDataMaskingConfigurator.Repository;
using System;
using System.Windows.Forms;

namespace SQLDataMaskingConfigurator.Forms
{
    internal partial class FrmDatabase : Form
    {
        private readonly Logger Logger;
        private readonly ConfigHelper configHelper;
        private readonly IRepoConnection repoConnection;
        public string ServerName { get; private set; }
        public string DatabaseName { get; private set; }
        public FrmDatabase(ConfigHelper configHelper, Logger logger)
        {
            InitializeComponent();
            this.configHelper = configHelper;
            Logger = logger;
            repoConnection = new RepoConnection(configHelper, logger);
        }

        #region Form & Control Events

        private void FrmDatabase_Load(object sender, EventArgs e)
        {
            SetControlsVisibility();

            //Server: LPCD-3YXGSQ2\\SQLEXPRESS; LPCH -5CD7342X0Q;
            //Database: DataMasking; AdventureWorksDW2014; salesdb
            txtDataSource.Text = "LPCD-3YXGSQ2\\SQLEXPRESS";
            txtDatabase.Text = "salesdb";
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            lblDescription.Text = string.Empty;
            if (ValidateConnection(out string _message, out string connectionString))
            {
                lblDescription.Text = connectionString;
                Utility.ShowMessageBox(_message, MessageBoxButtons.OK);
                DialogResult = DialogResult.OK;
            }
            else
            {
                Utility.ShowErrorMessageBox(_message);
                DialogResult = DialogResult.None;
            }
        }
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            lblDescription.Text = string.Empty;
            if (ValidateConnection(out string _message, out string connectionString))
            {
                lblDescription.Text = connectionString;
                Utility.ShowMessageBox(_message, MessageBoxButtons.OK);
                DialogResult = DialogResult.None;
            }
            else
            {
                Utility.ShowErrorMessageBox(_message);
                DialogResult = DialogResult.None;
            }
        }
        private void chkWindowsAthentication_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWindowsAthentication.Checked)
            {
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                txtUser.Text = string.Empty;
                txtPassword.Text = string.Empty;
            }
            else
            {
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
            }
        }
        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Utility.AllowOnlyNumericEntry(e.KeyChar);
        }

        #endregion

        #region Private Methods

        private void SetControlsVisibility()
        {
            lblDescription.Text = string.Empty;
        }
        private bool ValidateConnection(out string Message, out string connStr)
        {
            connStr = string.Empty;
            if (!IsValidDialogData(out Message))
            {
                return false;
            }

            string dataSource = repoConnection.GetDataSourceString(txtDataSource.Text.Trim(), txtPort.Text.Trim());

            string connectionString = repoConnection.GetConnectionStringCreated(dataSource, chkWindowsAthentication.Checked,
                txtDatabase.Text.Trim(), txtUser.Text.Trim(), txtPassword.Text.Trim());

            if (repoConnection.ValidateDbConnection(connectionString, txtDatabase.Text.Trim()))
            {
                connStr = connectionString;
                ServerName = dataSource;
                DatabaseName = txtDatabase.Text.Trim();
                Message = "Successfully connected with database.";
                return true;
            }
            else
            {
                Message = "Database connection not established.";
                return false;
            }
        }

        private bool IsValidDialogData(out string Message)
        {
            Message = string.Empty;
            if (string.IsNullOrEmpty(txtDataSource.Text))
            {
                Message += "Invalid Data Source" + Environment.NewLine;
            }
            if (string.IsNullOrEmpty(txtDatabase.Text))
            {
                Message += "Invalid Database" + Environment.NewLine;
            }
            if (!chkWindowsAthentication.Checked)
            {
                if (string.IsNullOrEmpty(txtUser.Text))
                {
                    Message += "Invalid User Name" + Environment.NewLine;
                }
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    Message += "Invalid Password" + Environment.NewLine;
                }
            }
            return string.IsNullOrEmpty(Message);
        }

        #endregion


    }
}
