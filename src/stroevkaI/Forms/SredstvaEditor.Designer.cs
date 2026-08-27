namespace stroevkaI.Forms
{
    partial class SredstvaEditor
    {
        private System.ComponentModel.IContainer components = null;

        public DataGridView dgvSredstva;
        private ToolStrip toolStrip1;
        private ToolStripButton btnAdd;
        private ToolStripButton btnDelete;
        private ToolStripButton btnEdit;
        private Label lblCount;

        // Колонки
        private DataGridViewTextBoxColumn sredstvoNameColumn;
        private DataGridViewTextBoxColumn brColumn;
        private DataGridViewTextBoxColumn rezervColumn;
        private DataGridViewTextBoxColumn remontColumn;
        private DataGridViewTextBoxColumn to1Column;
        private DataGridViewTextBoxColumn to2Column;
        private DataGridViewTextBoxColumn idColumn;
        private DataGridViewTextBoxColumn orderColumn;

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
            this.dgvSredstva = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.lblCount = new System.Windows.Forms.Label();

            this.sredstvoNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.brColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rezervColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remontColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.to1Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.to2Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.dgvSredstva)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();

            // toolStrip1
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.btnAdd,
                this.btnDelete,
                new System.Windows.Forms.ToolStripSeparator(),
                this.btnEdit
            });
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(577, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";

            // btnAdd
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAdd.Enabled = false;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(65, 22);
            this.btnAdd.Text = "Добавить";

            // btnDelete
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDelete.Enabled = false;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 22);
            this.btnDelete.Text = "Удалить";

            // btnEdit
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(83, 22);
            this.btnEdit.Text = "Редактировать";

            // lblCount
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(10, 28);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 13);
            this.lblCount.TabIndex = 1;

            // dgvSredstva
            this.dgvSredstva.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSredstva.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.sredstvoNameColumn,
                this.brColumn,
                this.rezervColumn,
                this.remontColumn,
                this.to1Column,
                this.to2Column,
                this.idColumn,
                this.orderColumn});
            this.dgvSredstva.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSredstva.Location = new System.Drawing.Point(0, 25);
            this.dgvSredstva.Name = "dgvSredstva";
            this.dgvSredstva.RowTemplate.Height = 23;
            this.dgvSredstva.Size = new System.Drawing.Size(577, 408);
            this.dgvSredstva.TabIndex = 2;
            this.dgvSredstva.AutoGenerateColumns = false;
            this.dgvSredstva.AllowUserToAddRows = false;
            this.dgvSredstva.AllowUserToDeleteRows = false;
            this.dgvSredstva.ReadOnly = true;
            this.dgvSredstva.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSredstva.MultiSelect = false;
            this.dgvSredstva.RowHeadersVisible = false;
            this.dgvSredstva.GridColor = System.Drawing.Color.White;
            this.dgvSredstva.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvSredstva.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;

            // sredstvoNameColumn
            this.sredstvoNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sredstvoNameColumn.DataPropertyName = "NameSredstvo";
            this.sredstvoNameColumn.HeaderText = "Средство";
            this.sredstvoNameColumn.Name = "sredstvoNameColumn";

            // brColumn
            this.brColumn.DataPropertyName = "Br";
            this.brColumn.HeaderText = "Бр";
            this.brColumn.Name = "brColumn";
            this.brColumn.Width = 50;
            this.brColumn.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // rezervColumn
            this.rezervColumn.DataPropertyName = "Rezerv";
            this.rezervColumn.HeaderText = "рзрв";
            this.rezervColumn.Name = "rezervColumn";
            this.rezervColumn.Width = 50;
            this.rezervColumn.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // remontColumn
            this.remontColumn.DataPropertyName = "Remont";
            this.remontColumn.HeaderText = "Рем";
            this.remontColumn.Name = "remontColumn";
            this.remontColumn.Width = 50;
            this.remontColumn.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // to1Column
            this.to1Column.DataPropertyName = "Tofirst";
            this.to1Column.HeaderText = "to1";
            this.to1Column.Name = "to1Column";
            this.to1Column.Width = 50;
            this.to1Column.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // to2Column
            this.to2Column.DataPropertyName = "Totow";
            this.to2Column.HeaderText = "to2";
            this.to2Column.Name = "to2Column";
            this.to2Column.Width = 50;
            this.to2Column.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // idColumn
            this.idColumn.DataPropertyName = "Id";
            this.idColumn.HeaderText = "id";
            this.idColumn.Name = "idColumn";
            this.idColumn.Visible = false;

            // orderColumn
            this.orderColumn.DataPropertyName = "Norder";
            this.orderColumn.HeaderText = "order";
            this.orderColumn.Name = "orderColumn";
            this.orderColumn.Visible = false;

            // SredstvaEditor
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvSredstva);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SredstvaEditor";
            this.Size = new System.Drawing.Size(577, 433);

            ((System.ComponentModel.ISupportInitialize)(this.dgvSredstva)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}