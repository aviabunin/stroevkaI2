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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvSredstva = new System.Windows.Forms.DataGridView();
            this.sredstvoNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.brColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rezervColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remontColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.to1Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.to2Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.lblCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSredstva)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSredstva
            // 
            this.dgvSredstva.AllowUserToAddRows = false;
            this.dgvSredstva.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            this.dgvSredstva.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSredstva.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvSredstva.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSredstva.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sredstvoNameColumn,
            this.brColumn,
            this.rezervColumn,
            this.remontColumn,
            this.to1Column,
            this.to2Column,
            this.orderColumn});
            this.dgvSredstva.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSredstva.GridColor = System.Drawing.Color.White;
            this.dgvSredstva.Location = new System.Drawing.Point(0, 25);
            this.dgvSredstva.MultiSelect = false;
            this.dgvSredstva.Name = "dgvSredstva";
            this.dgvSredstva.ReadOnly = true;
            this.dgvSredstva.RowHeadersVisible = false;
            this.dgvSredstva.RowTemplate.Height = 23;
            this.dgvSredstva.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSredstva.Size = new System.Drawing.Size(577, 408);
            this.dgvSredstva.TabIndex = 2;
            //this.dgvSredstva.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSredstva_CellClick);
            // 
            // sredstvoNameColumn
            // 
            this.sredstvoNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sredstvoNameColumn.DataPropertyName = "NameSredstvo";
            this.sredstvoNameColumn.HeaderText = "Средство";
            this.sredstvoNameColumn.Name = "sredstvoNameColumn";
            this.sredstvoNameColumn.ReadOnly = true;
            // 
            // brColumn
            // 
            this.brColumn.DataPropertyName = "Br";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.brColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.brColumn.HeaderText = "Бр";
            this.brColumn.Name = "brColumn";
            this.brColumn.ReadOnly = true;
            this.brColumn.Width = 50;
            // 
            // rezervColumn
            // 
            this.rezervColumn.DataPropertyName = "Rezerv";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.rezervColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.rezervColumn.HeaderText = "рзрв";
            this.rezervColumn.Name = "rezervColumn";
            this.rezervColumn.ReadOnly = true;
            this.rezervColumn.Width = 50;
            // 
            // remontColumn
            // 
            this.remontColumn.DataPropertyName = "Remont";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.remontColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.remontColumn.HeaderText = "Рем";
            this.remontColumn.Name = "remontColumn";
            this.remontColumn.ReadOnly = true;
            this.remontColumn.Width = 50;
            // 
            // to1Column
            // 
            this.to1Column.DataPropertyName = "Tofirst";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.to1Column.DefaultCellStyle = dataGridViewCellStyle5;
            this.to1Column.HeaderText = "to1";
            this.to1Column.Name = "to1Column";
            this.to1Column.ReadOnly = true;
            this.to1Column.Width = 50;
            // 
            // to2Column
            // 
            this.to2Column.DataPropertyName = "Totow";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.to2Column.DefaultCellStyle = dataGridViewCellStyle6;
            this.to2Column.HeaderText = "to2";
            this.to2Column.Name = "to2Column";
            this.to2Column.ReadOnly = true;
            this.to2Column.Width = 50;
            // 
            // idColumn
            // 
            this.idColumn.DataPropertyName = "Id";
            this.idColumn.HeaderText = "id";
            this.idColumn.Name = "idColumn";
            this.idColumn.Visible = false;
            // 
            // orderColumn
            // 
            this.orderColumn.DataPropertyName = "Norder";
            this.orderColumn.HeaderText = "order";
            this.orderColumn.Name = "orderColumn";
            this.orderColumn.ReadOnly = true;
            this.orderColumn.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete,
            this.btnEdit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(577, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAdd.Enabled = false;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(61, 22);
            this.btnAdd.Text = "Добавить";
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDelete.Enabled = false;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(55, 22);
            this.btnDelete.Text = "Удалить";
            // 
            // btnEdit
            // 
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(90, 22);
            this.btnEdit.Text = "Редактировать";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(10, 28);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 13);
            this.lblCount.TabIndex = 1;
            // 
            // SredstvaEditor
            // 
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