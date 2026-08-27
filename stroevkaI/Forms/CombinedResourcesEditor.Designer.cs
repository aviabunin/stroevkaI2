// stroevkaI/Forms/CombinedResourcesEditor.Designer.cs
namespace stroevkaI.Forms
{
    partial class CombinedResourcesEditor
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel tableLayout;
        private System.Windows.Forms.Panel panelWaters;
        private System.Windows.Forms.Panel panelPenas;
        private System.Windows.Forms.Panel panelSizods;
        private System.Windows.Forms.Panel panelKostyms;

        private System.Windows.Forms.DataGridView dgvWaters;
        private System.Windows.Forms.DataGridView dgvPenas;
        private System.Windows.Forms.DataGridView dgvSizods;
        private System.Windows.Forms.DataGridView dgvKostyms;

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkEditMode;

        private void InitializeComponent()
        {
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panelWaters = new System.Windows.Forms.Panel();
            this.panelPenas = new System.Windows.Forms.Panel();
            this.panelSizods = new System.Windows.Forms.Panel();
            this.panelKostyms = new System.Windows.Forms.Panel();

            this.dgvWaters = new System.Windows.Forms.DataGridView();
            this.dgvPenas = new System.Windows.Forms.DataGridView();
            this.dgvSizods = new System.Windows.Forms.DataGridView();
            this.dgvKostyms = new System.Windows.Forms.DataGridView();

            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkEditMode = new System.Windows.Forms.CheckBox();

            this.tableLayout.SuspendLayout();
            this.panelWaters.SuspendLayout();
            this.panelPenas.SuspendLayout();
            this.panelSizods.SuspendLayout();
            this.panelKostyms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPenas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSizods)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKostyms)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();

            // 
            // tableLayout
            // 
            this.tableLayout.ColumnCount = 2;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout.Controls.Add(this.panelWaters, 0, 0);
            this.tableLayout.Controls.Add(this.panelPenas, 1, 0);
            this.tableLayout.Controls.Add(this.panelSizods, 0, 1);
            this.tableLayout.Controls.Add(this.panelKostyms, 1, 1);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.Location = new System.Drawing.Point(0, 40);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.RowCount = 2;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout.Size = new System.Drawing.Size(650, 425);
            this.tableLayout.TabIndex = 0;

            // 
            // panelWaters
            // 
            this.panelWaters.Controls.Add(this.dgvWaters);
            this.panelWaters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWaters.Location = new System.Drawing.Point(3, 3);
            this.panelWaters.Name = "panelWaters";
            this.panelWaters.Padding = new System.Windows.Forms.Padding(2);
            this.panelWaters.Size = new System.Drawing.Size(319, 206);
            this.panelWaters.TabIndex = 0;

            // 
            // panelPenas
            // 
            this.panelPenas.Controls.Add(this.dgvPenas);
            this.panelPenas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPenas.Location = new System.Drawing.Point(328, 3);
            this.panelPenas.Name = "panelPenas";
            this.panelPenas.Padding = new System.Windows.Forms.Padding(2);
            this.panelPenas.Size = new System.Drawing.Size(319, 206);
            this.panelPenas.TabIndex = 1;

            // 
            // panelSizods
            // 
            this.panelSizods.Controls.Add(this.dgvSizods);
            this.panelSizods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSizods.Location = new System.Drawing.Point(3, 215);
            this.panelSizods.Name = "panelSizods";
            this.panelSizods.Padding = new System.Windows.Forms.Padding(2);
            this.panelSizods.Size = new System.Drawing.Size(319, 207);
            this.panelSizods.TabIndex = 2;

            // 
            // panelKostyms
            // 
            this.panelKostyms.Controls.Add(this.dgvKostyms);
            this.panelKostyms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelKostyms.Location = new System.Drawing.Point(328, 215);
            this.panelKostyms.Name = "panelKostyms";
            this.panelKostyms.Padding = new System.Windows.Forms.Padding(2);
            this.panelKostyms.Size = new System.Drawing.Size(319, 207);
            this.panelKostyms.TabIndex = 3;

            // 
            // dgvWaters
            // 
            this.dgvWaters.ColumnCount = 4;
            this.dgvWaters.Columns.Add("colWatersId", "Id");
            this.dgvWaters.Columns.Add("colWatersName", "Источник");
            this.dgvWaters.Columns.Add("colWatersTotal", "Всего");
            this.dgvWaters.Columns.Add("colWatersFault", "Неиспр.");
            this.dgvWaters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWaters.Location = new System.Drawing.Point(2, 2);
            this.dgvWaters.Name = "dgvWaters";
            this.dgvWaters.Size = new System.Drawing.Size(315, 202);
            this.dgvWaters.TabIndex = 0;

            // 
            // dgvPenas
            // 
            this.dgvPenas.ColumnCount = 4;
            this.dgvPenas.Columns.Add("colPenasId", "Id");
            this.dgvPenas.Columns.Add("colPenasName", "Пенообразователь");
            this.dgvPenas.Columns.Add("colPenasInwork", "В работе");
            this.dgvPenas.Columns.Add("colPenasInrezerv", "В резерве");
            this.dgvPenas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPenas.Location = new System.Drawing.Point(2, 2);
            this.dgvPenas.Name = "dgvPenas";
            this.dgvPenas.Size = new System.Drawing.Size(315, 202);
            this.dgvPenas.TabIndex = 0;

            // 
            // dgvSizods
            // 
            this.dgvSizods.ColumnCount = 6;
            this.dgvSizods.Columns.Add("colSizodsId", "Id");
            this.dgvSizods.Columns.Add("colSizodsName", "Средство");
            this.dgvSizods.Columns.Add("colSizodsRaschet", "Расчет");
            this.dgvSizods.Columns.Add("colSizodsRezerv", "Резерв");
            this.dgvSizods.Columns.Add("colSizodsPostGdzs", "Пост ГДЗС");
            this.dgvSizods.Columns.Add("colSizodsBazaGdzs", "База ГДЗС");
            this.dgvSizods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSizods.Location = new System.Drawing.Point(2, 2);
            this.dgvSizods.Name = "dgvSizods";
            this.dgvSizods.Size = new System.Drawing.Size(315, 203);
            this.dgvSizods.TabIndex = 0;

            // 
            // dgvKostyms
            // 
            this.dgvKostyms.ColumnCount = 3;
            this.dgvKostyms.Columns.Add("colKostymsId", "Id");
            this.dgvKostyms.Columns.Add("colKostymsName", "Марка");
            this.dgvKostyms.Columns.Add("colKostymsCount", "Кол-во");
            this.dgvKostyms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKostyms.Location = new System.Drawing.Point(2, 2);
            this.dgvKostyms.Name = "dgvKostyms";
            this.dgvKostyms.Size = new System.Drawing.Size(315, 203);
            this.dgvKostyms.TabIndex = 0;

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
            this.chkEditMode.TabIndex = 1;
            this.chkEditMode.Text = "Редактирование постоянных";
            this.chkEditMode.UseVisualStyleBackColor = true;
            this.chkEditMode.CheckedChanged += new System.EventHandler(this.ChkEditMode_CheckedChanged);

            // 
            // CombinedResourcesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayout);
            this.Controls.Add(this.panelButtons);
            this.Name = "CombinedResourcesEditor";
            this.Size = new System.Drawing.Size(650, 465);
            this.tableLayout.ResumeLayout(false);
            this.panelWaters.ResumeLayout(false);
            this.panelPenas.ResumeLayout(false);
            this.panelSizods.ResumeLayout(false);
            this.panelKostyms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPenas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSizods)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKostyms)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}