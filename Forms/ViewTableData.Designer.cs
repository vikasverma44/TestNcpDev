namespace SQLDataMaskingConfigurator.Forms
{
    partial class ViewTableData
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
            this.dgvTableData = new System.Windows.Forms.DataGridView();
            this.cmbViewTable = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblPages = new System.Windows.Forms.Label();
            this.lblTotalRecords = new System.Windows.Forms.Label();
            this.cmbPgSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTableData
            // 
            this.dgvTableData.AllowUserToAddRows = false;
            this.dgvTableData.AllowUserToDeleteRows = false;
            this.dgvTableData.AllowUserToOrderColumns = true;
            this.dgvTableData.AllowUserToResizeRows = false;
            this.dgvTableData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTableData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTableData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTableData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvTableData.Location = new System.Drawing.Point(41, 81);
            this.dgvTableData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvTableData.MultiSelect = false;
            this.dgvTableData.Name = "dgvTableData";
            this.dgvTableData.ReadOnly = true;
            this.dgvTableData.RowHeadersVisible = false;
            this.dgvTableData.RowHeadersWidth = 51;
            this.dgvTableData.RowTemplate.Height = 24;
            this.dgvTableData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTableData.Size = new System.Drawing.Size(973, 416);
            this.dgvTableData.TabIndex = 7;
            // 
            // cmbViewTable
            // 
            this.cmbViewTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbViewTable.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbViewTable.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbViewTable.FormattingEnabled = true;
            this.cmbViewTable.Location = new System.Drawing.Point(313, 21);
            this.cmbViewTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbViewTable.Name = "cmbViewTable";
            this.cmbViewTable.Size = new System.Drawing.Size(529, 24);
            this.cmbViewTable.TabIndex = 1;
            this.cmbViewTable.SelectedIndexChanged += new System.EventHandler(this.cmbViewTable_SelectedIndexChanged);
            this.cmbViewTable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbViewTable_KeyDown);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(207, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 26);
            this.label1.TabIndex = 24;
            this.label1.Text = "Select Table";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnFirstPage.Location = new System.Drawing.Point(41, 523);
            this.btnFirstPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(91, 36);
            this.btnFirstPage.TabIndex = 2;
            this.btnFirstPage.Text = "First Page";
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNextPage.Location = new System.Drawing.Point(239, 523);
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(91, 36);
            this.btnNextPage.TabIndex = 3;
            this.btnNextPage.Text = "Next Page";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPrevious.Location = new System.Drawing.Point(140, 523);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(91, 36);
            this.btnPrevious.TabIndex = 4;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnLastPage
            // 
            this.btnLastPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnLastPage.Location = new System.Drawing.Point(337, 523);
            this.btnLastPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(91, 36);
            this.btnLastPage.TabIndex = 5;
            this.btnLastPage.Text = "Last Page";
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(924, 523);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(91, 36);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblPages
            // 
            this.lblPages.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblPages.AutoSize = true;
            this.lblPages.Location = new System.Drawing.Point(580, 533);
            this.lblPages.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(135, 17);
            this.lblPages.TabIndex = 33;
            this.lblPages.Text = "Page 0 of 0 Page(s)\r\n";
            // 
            // lblTotalRecords
            // 
            this.lblTotalRecords.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblTotalRecords.AutoSize = true;
            this.lblTotalRecords.Location = new System.Drawing.Point(756, 533);
            this.lblTotalRecords.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalRecords.Name = "lblTotalRecords";
            this.lblTotalRecords.Size = new System.Drawing.Size(127, 17);
            this.lblTotalRecords.TabIndex = 35;
            this.lblTotalRecords.Text = "Total Record(s):  0";
            // 
            // cmbPgSize
            // 
            this.cmbPgSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPgSize.FormattingEnabled = true;
            this.cmbPgSize.Location = new System.Drawing.Point(957, 25);
            this.cmbPgSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbPgSize.Name = "cmbPgSize";
            this.cmbPgSize.Size = new System.Drawing.Size(56, 24);
            this.cmbPgSize.TabIndex = 36;
            this.cmbPgSize.SelectedIndexChanged += new System.EventHandler(this.cmbPgSize_SelectedIndexChanged);
            this.cmbPgSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbPgSize_KeyDown);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(849, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 26);
            this.label2.TabIndex = 37;
            this.label2.Text = "Page Size:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ViewTableData
            // 
            this.AcceptButton = this.btnFirstPage;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(1067, 575);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbPgSize);
            this.Controls.Add(this.lblTotalRecords);
            this.Controls.Add(this.lblPages);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLastPage);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnFirstPage);
            this.Controls.Add(this.dgvTableData);
            this.Controls.Add(this.cmbViewTable);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(1082, 612);
            this.Name = "ViewTableData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ViewTableData";
            this.Load += new System.EventHandler(this.ViewTableData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvTableData;
        private System.Windows.Forms.ComboBox cmbViewTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblPages;
        private System.Windows.Forms.Label lblTotalRecords;
        private System.Windows.Forms.ComboBox cmbPgSize;
        private System.Windows.Forms.Label label2;
    }
}