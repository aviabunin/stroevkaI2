// stroevkaI/Forms/SostavEditor.Designer.cs
namespace stroevkaI.Forms
{
    partial class SostavEditor
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvSostav;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSaveConst; // Объявляем как поле класса
        private System.Windows.Forms.CheckBox chkEditMode;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;


        private void InitializeComponent()
        {
            this.dgvSostav = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveConst = new System.Windows.Forms.Button();
            this.chkEditMode = new System.Windows.Forms.CheckBox();
            this.lblTitle = new System.Windows.Forms.Label();

            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.dgvSostav)).BeginInit();
            this.SuspendLayout();

            // 
            // dgvSostav
            // 
            this.dgvSostav.AllowUserToAddRows = false;
            this.dgvSostav.AllowUserToDeleteRows = false;
            this.dgvSostav.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSostav.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colId,
                this.colName,
                this.colCount});
            this.dgvSostav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSostav.Location = new System.Drawing.Point(0, 40);
            this.dgvSostav.Name = "dgvSostav";
            this.dgvSostav.RowHeadersVisible = false;
            this.dgvSostav.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSostav.Size = new System.Drawing.Size(650, 400);
            this.dgvSostav.TabIndex = 0;
            this.dgvSostav.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSostav_CellClick);
            this.dgvSostav.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSostav_CellEndEdit);
            this.dgvSostav.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvSostav_ColumnHeaderMouseClick);

            // 
            // colId
            // 
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Visible = false;
            this.colId.Width = 50;

            // 
            // colName
            // 
            this.colName.HeaderText = "Параметр";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 350;

            // 
            // colCount
            // 
            this.colCount.HeaderText = "Количество";
            this.colCount.Name = "colCount";
            this.colCount.Width = 120;

            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Controls.Add(this.btnSaveConst);
            this.panelButtons.Controls.Add(this.chkEditMode);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(650, 40);
            this.panelButtons.TabIndex = 1;

            // 
            // btnSave (основное сохранение)
            // 
            this.btnSave.Enabled = true;
            this.btnSave.Location = new System.Drawing.Point(12, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // 
            // btnSaveConst (сохранение для ручного редактирования "Всего")
            // 
            this.btnSaveConst.Enabled = false;
            this.btnSaveConst.Location = new System.Drawing.Point(103, 10);
            this.btnSaveConst.Name = "btnSaveConst";
            this.btnSaveConst.Size = new System.Drawing.Size(120, 23);
            this.btnSaveConst.TabIndex = 1;
            this.btnSaveConst.Text = "Сохранить (Всего)";
            this.btnSaveConst.UseVisualStyleBackColor = true;
            this.btnSaveConst.Click += new System.EventHandler(this.BtnSave_Click);

            // 
            // chkEditMode
            // 
            this.chkEditMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEditMode.AutoSize = true;
            this.chkEditMode.Location = new System.Drawing.Point(450, 12);
            this.chkEditMode.Name = "chkEditMode";
            this.chkEditMode.Size = new System.Drawing.Size(180, 17);
            this.chkEditMode.TabIndex = 3;
            this.chkEditMode.Text = "Редактирование постоянных";
            this.chkEditMode.UseVisualStyleBackColor = true;
            this.chkEditMode.CheckedChanged += new System.EventHandler(this.ChkEditMode_CheckedChanged);

            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTitle.Location = new System.Drawing.Point(0, 440);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(650, 25);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Личный состав";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // SostavEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvSostav);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lblTitle);
            this.Name = "SostavEditor";
            this.Size = new System.Drawing.Size(650, 465);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSostav)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}