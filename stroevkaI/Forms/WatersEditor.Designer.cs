// stroevkaI/Forms/WatersEditor.Designer.cs
namespace stroevkaI.Forms
{
    partial class WatersEditor
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvWaters;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSave;

        private void InitializeComponent()
        {
            this.dgvWaters = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvWaters)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();

            // 
            // dgvWaters
            // 
            this.dgvWaters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWaters.Location = new System.Drawing.Point(0, 35);
            this.dgvWaters.Name = "dgvWaters";
            this.dgvWaters.Size = new System.Drawing.Size(300, 150);
            this.dgvWaters.TabIndex = 0;

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
            this.btnSave.Size = new System.Drawing.Size(65, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // 
            // WatersEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvWaters);
            this.Controls.Add(this.panelButtons);
            this.Name = "WatersEditor";
            this.Size = new System.Drawing.Size(300, 185);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaters)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}