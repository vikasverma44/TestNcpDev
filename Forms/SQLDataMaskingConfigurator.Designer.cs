namespace SQLDataMaskingConfigurator.Forms
{
    partial class SQLDataMaskingConfigurator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.lblDatabaseConnection = new System.Windows.Forms.Label();
            this.btnDatabase = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.lnkLabelRecordCount = new System.Windows.Forms.LinkLabel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvDependent = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvConstraints = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvReferences = new System.Windows.Forms.DataGridView();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnViewData = new System.Windows.Forms.Button();
            this.btnMask = new System.Windows.Forms.Button();
            this.dgvConfiguration = new System.Windows.Forms.DataGridView();
            this.cmbTable = new System.Windows.Forms.ComboBox();
            this.toolTipColumns = new System.Windows.Forms.ToolTip(this.components);
            this.pnlDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDependent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConstraints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReferences)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfiguration)).BeginInit();
            this.SuspendLayout();
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnQuit.Location = new System.Drawing.Point(811, 697);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(233, 44);
            this.btnQuit.TabIndex = 7;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAbout.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAbout.Location = new System.Drawing.Point(33, 697);
            this.btnAbout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(125, 44);
            this.btnAbout.TabIndex = 6;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // lblDatabaseConnection
            // 
            this.lblDatabaseConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDatabaseConnection.Location = new System.Drawing.Point(229, 12);
            this.lblDatabaseConnection.Name = "lblDatabaseConnection";
            this.lblDatabaseConnection.Size = new System.Drawing.Size(840, 44);
            this.lblDatabaseConnection.TabIndex = 12;
            this.lblDatabaseConnection.Text = "DB Connection String :";
            this.lblDatabaseConnection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDatabase
            // 
            this.btnDatabase.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDatabase.Location = new System.Drawing.Point(33, 12);
            this.btnDatabase.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDatabase.Name = "btnDatabase";
            this.btnDatabase.Size = new System.Drawing.Size(149, 44);
            this.btnDatabase.TabIndex = 1;
            this.btnDatabase.Text = "Select Database";
            this.btnDatabase.UseVisualStyleBackColor = true;
            this.btnDatabase.Click += new System.EventHandler(this.btnDatabase_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(385, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "Select Table";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(20, 604);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1012, 20);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 13;
            this.progressBar.Value = 50;
            // 
            // pnlDetails
            // 
            this.pnlDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetails.Controls.Add(this.lnkLabelRecordCount);
            this.pnlDetails.Controls.Add(this.lblProgress);
            this.pnlDetails.Controls.Add(this.label4);
            this.pnlDetails.Controls.Add(this.label8);
            this.pnlDetails.Controls.Add(this.dgvDependent);
            this.pnlDetails.Controls.Add(this.label7);
            this.pnlDetails.Controls.Add(this.dgvConstraints);
            this.pnlDetails.Controls.Add(this.label6);
            this.pnlDetails.Controls.Add(this.label5);
            this.pnlDetails.Controls.Add(this.dgvReferences);
            this.pnlDetails.Controls.Add(this.txtTo);
            this.pnlDetails.Controls.Add(this.txtFrom);
            this.pnlDetails.Controls.Add(this.label3);
            this.pnlDetails.Controls.Add(this.label2);
            this.pnlDetails.Controls.Add(this.btnViewData);
            this.pnlDetails.Controls.Add(this.btnMask);
            this.pnlDetails.Controls.Add(this.dgvConfiguration);
            this.pnlDetails.Controls.Add(this.cmbTable);
            this.pnlDetails.Controls.Add(this.label1);
            this.pnlDetails.Controls.Add(this.progressBar);
            this.pnlDetails.Location = new System.Drawing.Point(13, 63);
            this.pnlDetails.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(1057, 628);
            this.pnlDetails.TabIndex = 14;
            // 
            // lnkLabelRecordCount
            // 
            this.lnkLabelRecordCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkLabelRecordCount.AutoSize = true;
            this.lnkLabelRecordCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLabelRecordCount.Location = new System.Drawing.Point(16, 538);
            this.lnkLabelRecordCount.Name = "lnkLabelRecordCount";
            this.lnkLabelRecordCount.Size = new System.Drawing.Size(82, 17);
            this.lnkLabelRecordCount.TabIndex = 24;
            this.lnkLabelRecordCount.TabStop = true;
            this.lnkLabelRecordCount.Text = "linkLabel1";
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.BackColor = System.Drawing.Color.Transparent;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(20, 578);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(1012, 23);
            this.lblProgress.TabIndex = 11;
            this.lblProgress.Text = "500/1000 (50%)";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(323, 535);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 23);
            this.label4.TabIndex = 19;
            this.label4.Text = "To";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Visible = false;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(16, 187);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(365, 23);
            this.label8.TabIndex = 14;
            this.label8.Text = "Dependent Upon";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvDependent
            // 
            this.dgvDependent.AllowUserToAddRows = false;
            this.dgvDependent.AllowUserToDeleteRows = false;
            this.dgvDependent.AllowUserToResizeRows = false;
            this.dgvDependent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDependent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDependent.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDependent.Location = new System.Drawing.Point(19, 212);
            this.dgvDependent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvDependent.MultiSelect = false;
            this.dgvDependent.Name = "dgvDependent";
            this.dgvDependent.ReadOnly = true;
            this.dgvDependent.RowHeadersVisible = false;
            this.dgvDependent.RowHeadersWidth = 51;
            this.dgvDependent.RowTemplate.Height = 24;
            this.dgvDependent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDependent.Size = new System.Drawing.Size(468, 160);
            this.dgvDependent.TabIndex = 9;
            this.dgvDependent.SelectionChanged += new System.EventHandler(this.dgvDependent_SelectionChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 374);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(365, 23);
            this.label7.TabIndex = 15;
            this.label7.Text = "Table Constraint(s)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvConstraints
            // 
            this.dgvConstraints.AllowUserToAddRows = false;
            this.dgvConstraints.AllowUserToDeleteRows = false;
            this.dgvConstraints.AllowUserToResizeRows = false;
            this.dgvConstraints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvConstraints.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvConstraints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConstraints.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvConstraints.Location = new System.Drawing.Point(19, 399);
            this.dgvConstraints.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvConstraints.MultiSelect = false;
            this.dgvConstraints.Name = "dgvConstraints";
            this.dgvConstraints.ReadOnly = true;
            this.dgvConstraints.RowHeadersVisible = false;
            this.dgvConstraints.RowHeadersWidth = 51;
            this.dgvConstraints.RowTemplate.Height = 24;
            this.dgvConstraints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConstraints.Size = new System.Drawing.Size(468, 121);
            this.dgvConstraints.TabIndex = 10;
            this.dgvConstraints.SelectionChanged += new System.EventHandler(this.dgvConstraints_SelectionChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(497, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(365, 23);
            this.label6.TabIndex = 23;
            this.label6.Text = "Column Detail(s)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(365, 23);
            this.label5.TabIndex = 13;
            this.label5.Text = "Referenced To ";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvReferences
            // 
            this.dgvReferences.AllowUserToAddRows = false;
            this.dgvReferences.AllowUserToDeleteRows = false;
            this.dgvReferences.AllowUserToResizeRows = false;
            this.dgvReferences.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvReferences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReferences.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvReferences.Location = new System.Drawing.Point(19, 62);
            this.dgvReferences.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvReferences.MultiSelect = false;
            this.dgvReferences.Name = "dgvReferences";
            this.dgvReferences.ReadOnly = true;
            this.dgvReferences.RowHeadersVisible = false;
            this.dgvReferences.RowHeadersWidth = 51;
            this.dgvReferences.RowTemplate.Height = 24;
            this.dgvReferences.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReferences.Size = new System.Drawing.Size(468, 124);
            this.dgvReferences.TabIndex = 8;
            this.dgvReferences.SelectionChanged += new System.EventHandler(this.dgvReferences_SelectionChanged);
            // 
            // txtTo
            // 
            this.txtTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTo.Location = new System.Drawing.Point(387, 535);
            this.txtTo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTo.MaxLength = 11;
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(100, 22);
            this.txtTo.TabIndex = 20;
            this.txtTo.Visible = false;
            this.txtTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTo_KeyPress);
            // 
            // txtFrom
            // 
            this.txtFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFrom.Location = new System.Drawing.Point(216, 535);
            this.txtFrom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFrom.MaxLength = 11;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(100, 22);
            this.txtFrom.TabIndex = 18;
            this.txtFrom.Visible = false;
            this.txtFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrom_KeyPress);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(151, 535);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "From";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(16, 535);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 23);
            this.label2.TabIndex = 16;
            this.label2.Text = "Set Record Set";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Visible = false;
            // 
            // btnViewData
            // 
            this.btnViewData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewData.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnViewData.Location = new System.Drawing.Point(797, 524);
            this.btnViewData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnViewData.Name = "btnViewData";
            this.btnViewData.Size = new System.Drawing.Size(235, 44);
            this.btnViewData.TabIndex = 5;
            this.btnViewData.Text = "View Table Data";
            this.btnViewData.UseVisualStyleBackColor = true;
            this.btnViewData.Click += new System.EventHandler(this.btnViewData_Click);
            // 
            // btnMask
            // 
            this.btnMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMask.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnMask.Location = new System.Drawing.Point(500, 524);
            this.btnMask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMask.Name = "btnMask";
            this.btnMask.Size = new System.Drawing.Size(292, 44);
            this.btnMask.TabIndex = 4;
            this.btnMask.Text = "Start Masking Process";
            this.btnMask.UseVisualStyleBackColor = true;
            this.btnMask.Click += new System.EventHandler(this.btnMask_Click);
            // 
            // dgvConfiguration
            // 
            this.dgvConfiguration.AllowUserToAddRows = false;
            this.dgvConfiguration.AllowUserToDeleteRows = false;
            this.dgvConfiguration.AllowUserToResizeRows = false;
            this.dgvConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvConfiguration.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvConfiguration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConfiguration.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvConfiguration.Location = new System.Drawing.Point(500, 62);
            this.dgvConfiguration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvConfiguration.Name = "dgvConfiguration";
            this.dgvConfiguration.ReadOnly = true;
            this.dgvConfiguration.RowHeadersVisible = false;
            this.dgvConfiguration.RowHeadersWidth = 51;
            this.dgvConfiguration.RowTemplate.Height = 24;
            this.dgvConfiguration.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConfiguration.Size = new System.Drawing.Size(532, 459);
            this.dgvConfiguration.TabIndex = 3;
            this.dgvConfiguration.SelectionChanged += new System.EventHandler(this.dgvConfiguration_SelectionChanged);
            // 
            // cmbTable
            // 
            this.cmbTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTable.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbTable.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTable.FormattingEnabled = true;
            this.cmbTable.Location = new System.Drawing.Point(500, 9);
            this.cmbTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbTable.Name = "cmbTable";
            this.cmbTable.Size = new System.Drawing.Size(529, 24);
            this.cmbTable.TabIndex = 2;
            this.cmbTable.SelectedIndexChanged += new System.EventHandler(this.cmbTable_SelectedIndexChanged);
            this.cmbTable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbTable_KeyDown);
            // 
            // toolTipColumns
            // 
            this.toolTipColumns.AutoPopDelay = 21000;
            this.toolTipColumns.InitialDelay = 500;
            this.toolTipColumns.ReshowDelay = 100;
            this.toolTipColumns.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipColumns.ToolTipTitle = "Selected column(s) for masking:";
            // 
            // SQLDataMaskingConfigurator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnQuit;
            this.ClientSize = new System.Drawing.Size(1084, 750);
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.btnDatabase);
            this.Controls.Add(this.lblDatabaseConnection);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnQuit);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(1098, 724);
            this.Name = "SQLDataMaskingConfigurator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL Data Masking Configurator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SQLDataMaskingConfigurator_Load);
            this.Resize += new System.EventHandler(this.SQLDataMaskingConfigurator_Resize);
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDependent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConstraints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReferences)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfiguration)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Label lblDatabaseConnection;
        private System.Windows.Forms.Button btnDatabase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.DataGridView dgvConfiguration;
        private System.Windows.Forms.ComboBox cmbTable;
        private System.Windows.Forms.Button btnViewData;
        private System.Windows.Forms.DataGridView dgvReferences;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvConstraints;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgvDependent;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnMask;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkLabelRecordCount;
        private System.Windows.Forms.ToolTip toolTipColumns;
    }
}

