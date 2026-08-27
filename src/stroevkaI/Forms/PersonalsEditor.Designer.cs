// stroevkaI/Forms/PersonalsEditor.Designer.cs
namespace stroevkaI.Forms
{
    partial class PersonalsEditor
    {
        private System.ComponentModel.IContainer components = null;

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
            this.panelButtons = new System.Windows.Forms.Panel();
            this.lblCount = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.cmbPostFilter = new System.Windows.Forms.ComboBox();
            this.lblPostFilter = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDeletePersonal = new System.Windows.Forms.Button();
            this.btnAddPersonal = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvPersonals = new System.Windows.Forms.DataGridView();

            // Колонки
            this.IdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PostColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AllowedPostsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TfMobilColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TfWorkColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TfDomColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonals)).BeginInit();
            this.SuspendLayout();

            // panelButtons
            this.panelButtons.Controls.Add(this.lblTitle);
            this.panelButtons.Controls.Add(this.btnAddPersonal);
            this.panelButtons.Controls.Add(this.btnDeletePersonal);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.lblPostFilter);
            this.panelButtons.Controls.Add(this.cmbPostFilter);
            this.panelButtons.Controls.Add(this.lblSearch);
            this.panelButtons.Controls.Add(this.txtSearch);
            this.panelButtons.Controls.Add(this.lblCount);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1100, 80);  // Увеличена ширина
            this.panelButtons.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(205, 17);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Редактор сотрудников ПЧ";

            // btnAddPersonal
            this.btnAddPersonal.Location = new System.Drawing.Point(230, 8);
            this.btnAddPersonal.Name = "btnAddPersonal";
            this.btnAddPersonal.Size = new System.Drawing.Size(90, 28);
            this.btnAddPersonal.TabIndex = 1;
            this.btnAddPersonal.Text = "Добавить";
            this.btnAddPersonal.UseVisualStyleBackColor = true;
            this.btnAddPersonal.Click += new System.EventHandler(this.BtnAddPersonal_Click);

            // btnDeletePersonal
            this.btnDeletePersonal.Location = new System.Drawing.Point(326, 8);
            this.btnDeletePersonal.Name = "btnDeletePersonal";
            this.btnDeletePersonal.Size = new System.Drawing.Size(90, 28);
            this.btnDeletePersonal.TabIndex = 2;
            this.btnDeletePersonal.Text = "Удалить";
            this.btnDeletePersonal.UseVisualStyleBackColor = true;
            this.btnDeletePersonal.Click += new System.EventHandler(this.BtnDeletePersonal_Click);

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(422, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 28);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);

            // lblPostFilter
            this.lblPostFilter.AutoSize = true;
            this.lblPostFilter.Location = new System.Drawing.Point(10, 45);
            this.lblPostFilter.Name = "lblPostFilter";
            this.lblPostFilter.Size = new System.Drawing.Size(123, 13);
            this.lblPostFilter.TabIndex = 4;
            this.lblPostFilter.Text = "Фильтр по должности:";

            // cmbPostFilter
            this.cmbPostFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPostFilter.Location = new System.Drawing.Point(140, 42);
            this.cmbPostFilter.Name = "cmbPostFilter";
            this.cmbPostFilter.Size = new System.Drawing.Size(200, 21);
            this.cmbPostFilter.TabIndex = 5;
            this.cmbPostFilter.SelectedIndexChanged += new System.EventHandler(this.CmbPostFilter_SelectedIndexChanged);

            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(360, 45);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(41, 13);
            this.lblSearch.TabIndex = 6;
            this.lblSearch.Text = "Поиск:";

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(408, 42);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(150, 20);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);

            // lblCount
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblCount.Location = new System.Drawing.Point(580, 45);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(53, 15);
            this.lblCount.TabIndex = 8;
            this.lblCount.Text = "Всего: 0";

            // dgvPersonals
            this.dgvPersonals.AllowUserToAddRows = false;
            this.dgvPersonals.AllowUserToDeleteRows = false;
            this.dgvPersonals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.IdColumn,
                this.FullNameColumn,
                this.PostColumn,
                this.AllowedPostsColumn,
                this.TfMobilColumn,
                this.TfWorkColumn,
                this.TfDomColumn
            });
            this.dgvPersonals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPersonals.Location = new System.Drawing.Point(0, 80);
            this.dgvPersonals.Name = "dgvPersonals";
            this.dgvPersonals.ReadOnly = true;
            this.dgvPersonals.RowHeadersVisible = false;
            this.dgvPersonals.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPersonals.Size = new System.Drawing.Size(1100, 520);  // Увеличена ширина
            this.dgvPersonals.TabIndex = 1;
            this.dgvPersonals.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvPersonals_CellDoubleClick);

            // IdColumn
            this.IdColumn.HeaderText = "ID";
            this.IdColumn.Name = "IdColumn";
            this.IdColumn.ReadOnly = true;
            this.IdColumn.Width = 40;

            // FullNameColumn
            this.FullNameColumn.HeaderText = "ФИО";
            this.FullNameColumn.Name = "FullNameColumn";
            this.FullNameColumn.ReadOnly = true;
            this.FullNameColumn.Width = 180;

            // PostColumn
            this.PostColumn.HeaderText = "Должность";
            this.PostColumn.Name = "PostColumn";
            this.PostColumn.ReadOnly = true;
            this.PostColumn.Width = 150;

            // AllowedPostsColumn
            this.AllowedPostsColumn.HeaderText = "Разрешённые должности";
            this.AllowedPostsColumn.Name = "AllowedPostsColumn";
            this.AllowedPostsColumn.ReadOnly = true;
            this.AllowedPostsColumn.Width = 250;

            // TfMobilColumn
            this.TfMobilColumn.HeaderText = "Мобильный";
            this.TfMobilColumn.Name = "TfMobilColumn";
            this.TfMobilColumn.ReadOnly = true;
            this.TfMobilColumn.Width = 120;

            // TfWorkColumn
            this.TfWorkColumn.HeaderText = "Рабочий";
            this.TfWorkColumn.Name = "TfWorkColumn";
            this.TfWorkColumn.ReadOnly = true;
            this.TfWorkColumn.Width = 120;

            // TfDomColumn
            this.TfDomColumn.HeaderText = "Домашний";
            this.TfDomColumn.Name = "TfDomColumn";
            this.TfDomColumn.ReadOnly = true;
            this.TfDomColumn.Width = 120;

            // PersonalsEditor
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvPersonals);
            this.Controls.Add(this.panelButtons);
            this.Name = "PersonalsEditor";
            this.Size = new System.Drawing.Size(1100, 600);  // Увеличена ширина

            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonals)).EndInit();
            this.ResumeLayout(false);
        }

        // --- Компоненты ---
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.ComboBox cmbPostFilter;
        private System.Windows.Forms.Label lblPostFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDeletePersonal;
        private System.Windows.Forms.Button btnAddPersonal;
        private System.Windows.Forms.DataGridView dgvPersonals;

        // Колонки
        private System.Windows.Forms.DataGridViewTextBoxColumn IdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PostColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AllowedPostsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TfMobilColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TfWorkColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TfDomColumn;
    }
}