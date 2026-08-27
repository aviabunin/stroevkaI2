namespace stroevkaI.Forms
{
    partial class CompareResultsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Label bottomLabel;
        private System.Windows.Forms.Label detailsHeader;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.lblSummary = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.detailsHeader = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.topPanel = new System.Windows.Forms.Panel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.bottomLabel = new System.Windows.Forms.Label();

            // Колонки для dgvResults
            this.colRowId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiffCount = new System.Windows.Forms.DataGridViewTextBoxColumn();

            // Колонки для dgvDetails
            this.colDetailColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailExcel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailGrid = new System.Windows.Forms.DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();

            // dgvResults
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResults.BackgroundColor = System.Drawing.Color.White;
            this.dgvResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colRowId,
                this.colName,
                this.colStatus,
                this.colDiffCount});
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dgvResults.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvResults.Location = new System.Drawing.Point(0, 0);
            this.dgvResults.MultiSelect = false;
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.Size = new System.Drawing.Size(576, 576);
            this.dgvResults.TabIndex = 0;
            this.dgvResults.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvResults_CellFormatting);
            this.dgvResults.SelectionChanged += new System.EventHandler(this.DgvResults_SelectionChanged);

            // colRowId
            this.colRowId.HeaderText = "RowId";
            this.colRowId.Name = "RowId";
            this.colRowId.ReadOnly = true;

            // colName
            this.colName.HeaderText = "Наименование";
            this.colName.Name = "Name";
            this.colName.ReadOnly = true;

            // colStatus
            this.colStatus.HeaderText = "Статус";
            this.colStatus.Name = "Status";
            this.colStatus.ReadOnly = true;

            // colDiffCount
            this.colDiffCount.HeaderText = "Кол-во различий";
            this.colDiffCount.Name = "DiffCount";
            this.colDiffCount.ReadOnly = true;

            // dgvDetails
            this.dgvDetails.AllowUserToAddRows = false;
            this.dgvDetails.AllowUserToDeleteRows = false;
            this.dgvDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None; // управляем вручную
            this.dgvDetails.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colDetailColumn,
                this.colDetailField,
                this.colDetailExcel,
                this.colDetailGrid});
            this.dgvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dgvDetails.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvDetails.Location = new System.Drawing.Point(0, 30);
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.ReadOnly = true;
            this.dgvDetails.RowHeadersVisible = false;
            this.dgvDetails.Size = new System.Drawing.Size(408, 546);
            this.dgvDetails.TabIndex = 1;

            // colDetailColumn – ширина 100, по центру
            this.colDetailColumn.HeaderText = "Колонка";
            this.colDetailColumn.Name = "colDetailColumn";
            this.colDetailColumn.ReadOnly = true;
            this.colDetailColumn.Width = 100;
            this.colDetailColumn.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDetailColumn.HeaderCell.Style.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // colDetailField – заполняет остаток
            this.colDetailField.DataPropertyName = "FieldName";
            this.colDetailField.HeaderText = "Поле";
            this.colDetailField.Name = "colDetailField";
            this.colDetailField.ReadOnly = true;
            this.colDetailField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDetailField.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            // colDetailExcel – ширина 100, по центру
            this.colDetailExcel.HeaderText = "Значение в Excel";
            this.colDetailExcel.Name = "colDetailExcel";
            this.colDetailExcel.ReadOnly = true;
            this.colDetailExcel.Width = 100;
            this.colDetailExcel.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDetailExcel.HeaderCell.Style.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // colDetailGrid – ширина 100, по центру
            this.colDetailGrid.HeaderText = "Значение в гриде";
            this.colDetailGrid.Name = "colDetailGrid";
            this.colDetailGrid.ReadOnly = true;
            this.colDetailGrid.Width = 100;
            this.colDetailGrid.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDetailGrid.HeaderCell.Style.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // lblSummary
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblSummary.Location = new System.Drawing.Point(10, 10);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(972, 40);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "Результаты сравнения:";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // splitContainer
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 60);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Panel1.Controls.Add(this.dgvResults);
            this.splitContainer.Panel1MinSize = 200;
            this.splitContainer.Panel2.Controls.Add(this.detailsHeader);
            this.splitContainer.Panel2.Controls.Add(this.dgvDetails);
            this.splitContainer.Panel2MinSize = 150;
            this.splitContainer.Size = new System.Drawing.Size(992, 578);
            this.splitContainer.SplitterDistance = 578;
            this.splitContainer.TabIndex = 0;

            // detailsHeader
            this.detailsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.detailsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.detailsHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.detailsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.detailsHeader.Location = new System.Drawing.Point(0, 0);
            this.detailsHeader.Name = "detailsHeader";
            this.detailsHeader.Size = new System.Drawing.Size(408, 30);
            this.detailsHeader.TabIndex = 0;
            this.detailsHeader.Text = "  Детали различий (выберите строку в списке выше)";
            this.detailsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // btnExport
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(1622, 15);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(150, 30);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Экспорт отчета";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);

            // topPanel
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.topPanel.Controls.Add(this.lblSummary);
            this.topPanel.Controls.Add(this.btnExport);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(10);
            this.topPanel.Size = new System.Drawing.Size(992, 60);
            this.topPanel.TabIndex = 1;

            // bottomPanel
            this.bottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.bottomPanel.Controls.Add(this.bottomLabel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 638);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(992, 35);
            this.bottomPanel.TabIndex = 2;

            // bottomLabel
            this.bottomLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bottomLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.bottomLabel.Location = new System.Drawing.Point(0, 0);
            this.bottomLabel.Name = "bottomLabel";
            this.bottomLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.bottomLabel.Size = new System.Drawing.Size(992, 35);
            this.bottomLabel.TabIndex = 0;
            this.bottomLabel.Text = "Строки с различиями выделены красным, совпадающие - зеленым, отсутствующие в гриде - серым";
            this.bottomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // CompareResultsForm
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(992, 673);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.bottomPanel);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "CompareResultsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Результаты сравнения с Excel";

            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private DataGridViewTextBoxColumn colRowId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colDiffCount;
        private DataGridViewTextBoxColumn colDetailColumn;
        private DataGridViewTextBoxColumn colDetailField;
        private DataGridViewTextBoxColumn colDetailExcel;
        private DataGridViewTextBoxColumn colDetailGrid;
    }
}