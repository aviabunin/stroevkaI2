// stroevkaI/Forms/EquipmentEditor.Designer.cs
namespace stroevkaI.Forms
{
    partial class EquipmentEditor
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPena;
        private System.Windows.Forms.TabPage tabSizod;
        private System.Windows.Forms.TabPage tabWaters;
        private System.Windows.Forms.TabPage tabKostym;
        private System.Windows.Forms.DataGridView dgvPena;
        private System.Windows.Forms.DataGridView dgvSizod;
        private System.Windows.Forms.DataGridView dgvWaters;
        private System.Windows.Forms.DataGridView dgvKostym;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkEditMode;
        private System.Windows.Forms.Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPena = new System.Windows.Forms.TabPage();
            this.dgvPena = new System.Windows.Forms.DataGridView();
            this.tabSizod = new System.Windows.Forms.TabPage();
            this.dgvSizod = new System.Windows.Forms.DataGridView();
            this.tabWaters = new System.Windows.Forms.TabPage();
            this.dgvWaters = new System.Windows.Forms.DataGridView();
            this.tabKostym = new System.Windows.Forms.TabPage();
            this.dgvKostym = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkEditMode = new System.Windows.Forms.CheckBox();
            this.lblTitle = new System.Windows.Forms.Label();

            this.tabControl1.SuspendLayout();
            this.tabPena.SuspendLayout();
            this.tabSizod.SuspendLayout();
            this.tabWaters.SuspendLayout();
            this.tabKostym.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPena)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSizod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKostym)).BeginInit();
            this.SuspendLayout();

            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPena);
            this.tabControl1.Controls.Add(this.tabSizod);
            this.tabControl1.Controls.Add(this.tabWaters);
            this.tabControl1.Controls.Add(this.tabKostym);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 40);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(650, 400);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);

            // 
            // tabPena
            // 
            this.tabPena.Controls.Add(this.dgvPena);
            this.tabPena.Location = new System.Drawing.Point(4, 22);
            this.tabPena.Name = "tabPena";
            this.tabPena.Padding = new System.Windows.Forms.Padding(3);
            this.tabPena.Size = new System.Drawing.Size(642, 374);
            this.tabPena.TabIndex = 0;
            this.tabPena.Text = "Пена";
            this.tabPena.UseVisualStyleBackColor = true;

            // 
            // dgvPena
            // 
            this.dgvPena.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPena.Location = new System.Drawing.Point(3, 3);
            this.dgvPena.Name = "dgvPena";
            this.dgvPena.Size = new System.Drawing.Size(636, 368);
            this.dgvPena.TabIndex = 0;

            // 
            // tabSizod
            // 
            this.tabSizod.Controls.Add(this.dgvSizod);
            this.tabSizod.Location = new System.Drawing.Point(4, 22);
            this.tabSizod.Name = "tabSizod";
            this.tabSizod.Padding = new System.Windows.Forms.Padding(3);
            this.tabSizod.Size = new System.Drawing.Size(642, 374);
            this.tabSizod.TabIndex = 1;
            this.tabSizod.Text = "СИЗОД";
            this.tabSizod.UseVisualStyleBackColor = true;

            // 
            // dgvSizod
            // 
            this.dgvSizod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSizod.Location = new System.Drawing.Point(3, 3);
            this.dgvSizod.Name = "dgvSizod";
            this.dgvSizod.Size = new System.Drawing.Size(636, 368);
            this.dgvSizod.TabIndex = 0;

            // 
            // tabWaters
            // 
            this.tabWaters.Controls.Add(this.dgvWaters);
            this.tabWaters.Location = new System.Drawing.Point(4, 22);
            this.tabWaters.Name = "tabWaters";
            this.tabWaters.Padding = new System.Windows.Forms.Padding(3);
            this.tabWaters.Size = new System.Drawing.Size(642, 374);
            this.tabWaters.TabIndex = 2;
            this.tabWaters.Text = "Вода";
            this.tabWaters.UseVisualStyleBackColor = true;

            // 
            // dgvWaters
            // 
            this.dgvWaters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWaters.Location = new System.Drawing.Point(3, 3);
            this.dgvWaters.Name = "dgvWaters";
            this.dgvWaters.Size = new System.Drawing.Size(636, 368);
            this.dgvWaters.TabIndex = 0;

            // 
            // tabKostym
            // 
            this.tabKostym.Controls.Add(this.dgvKostym);
            this.tabKostym.Location = new System.Drawing.Point(4, 22);
            this.tabKostym.Name = "tabKostym";
            this.tabKostym.Padding = new System.Windows.Forms.Padding(3);
            this.tabKostym.Size = new System.Drawing.Size(642, 374);
            this.tabKostym.TabIndex = 3;
            this.tabKostym.Text = "Костюмы";
            this.tabKostym.UseVisualStyleBackColor = true;

            // 
            // dgvKostym
            // 
            this.dgvKostym.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKostym.Location = new System.Drawing.Point(3, 3);
            this.dgvKostym.Name = "dgvKostym";
            this.dgvKostym.Size = new System.Drawing.Size(636, 368);
            this.dgvKostym.TabIndex = 0;

            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Controls.Add(this.chkEditMode);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(650, 40);
            this.panelButtons.TabIndex = 1;

            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(12, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // 
            // chkEditMode
            // 
            this.chkEditMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEditMode.AutoSize = true;
            this.chkEditMode.Location = new System.Drawing.Point(470, 12);
            this.chkEditMode.Name = "chkEditMode";
            this.chkEditMode.Size = new System.Drawing.Size(160, 17);
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
            this.lblTitle.Text = "Оборудование";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // EquipmentEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lblTitle);
            this.Name = "EquipmentEditor";
            this.Size = new System.Drawing.Size(650, 465);
            this.tabControl1.ResumeLayout(false);
            this.tabPena.ResumeLayout(false);
            this.tabSizod.ResumeLayout(false);
            this.tabWaters.ResumeLayout(false);
            this.tabKostym.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPena)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSizod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKostym)).EndInit();
            this.ResumeLayout(false);
        }
    }
}