// stroevkaI/Forms/KostymsEditor.Designer.cs
namespace stroevkaI.Forms
{
    partial class KostymsEditor
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvKostyms;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSave;

        private void InitializeComponent()
        {
            this.dgvKostyms = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvKostyms)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();

            // 
            // dgvKostyms
            // 
            this.dgvKostyms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKostyms.Location = new System.Drawing.Point(0, 35);
            this.dgvKostyms.Name = "dgvKostyms";
            this.dgvKostyms.Size = new System.Drawing.Size(300, 150);
            this.dgvKostyms.TabIndex = 0;

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
            // KostymsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvKostyms);
            this.Controls.Add(this.panelButtons);
            this.Name = "KostymsEditor";
            this.Size = new System.Drawing.Size(300, 185);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKostyms)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}