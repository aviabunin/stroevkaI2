// stroevkaI/Forms/PenasEditor.Designer.cs
namespace stroevkaI.Forms
{
    partial class PenasEditor
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvPenas;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSave;

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvPenas = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInWork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInrezerv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPenas)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPenas
            // 
            this.dgvPenas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colName,
            this.colInWork,
            this.colInrezerv});
            this.dgvPenas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPenas.Location = new System.Drawing.Point(0, 35);
            this.dgvPenas.Name = "dgvPenas";
            this.dgvPenas.Size = new System.Drawing.Size(300, 150);
            this.dgvPenas.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(300, 35);
            this.panelButtons.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(5, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Visible = false;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Mname";
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            this.colName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colName.HeaderText = "Пенообразователь";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colInWork
            // 
            this.colInWork.DataPropertyName = "inWork";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colInWork.DefaultCellStyle = dataGridViewCellStyle2;
            this.colInWork.HeaderText = "В работе";
            this.colInWork.Name = "colInWork";
            this.colInWork.Width = 80;
            // 
            // colInrezerv
            // 
            this.colInrezerv.DataPropertyName = "inReserv";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colInrezerv.DefaultCellStyle = dataGridViewCellStyle3;
            this.colInrezerv.HeaderText = "В резерве";
            this.colInrezerv.Name = "colInrezerv";
            this.colInrezerv.Width = 80;
            // 
            // PenasEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvPenas);
            this.Controls.Add(this.panelButtons);
            this.Name = "PenasEditor";
            this.Size = new System.Drawing.Size(300, 185);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPenas)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colInWork;
        private DataGridViewTextBoxColumn colInrezerv;
    }
}