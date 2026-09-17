namespace stroevkaI.Controls
{
    partial class StroevkaHelpDoc
    {
        private System.ComponentModel.IContainer components = null;
        private RichTextBox rtbHelp;
        private Panel panelButtons;
        private Button btnEdit;
        private Button btnSave;
        private Button btnReload;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.rtbHelp = new System.Windows.Forms.RichTextBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();

            this.panelButtons.SuspendLayout();
            this.SuspendLayout();

            // panelButtons
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Height = 35;
            this.panelButtons.Controls.Add(this.btnReload);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Controls.Add(this.btnEdit);

            // btnEdit
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.Location = new System.Drawing.Point(5, 5);
            this.btnEdit.Size = new System.Drawing.Size(130, 25);
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);

            // btnSave
            this.btnSave.Text = "Сохранить";
            this.btnSave.Location = new System.Drawing.Point(140, 5);
            this.btnSave.Size = new System.Drawing.Size(100, 25);
            this.btnSave.Enabled = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // btnReload
            this.btnReload.Text = "Перезагрузить";
            this.btnReload.Location = new System.Drawing.Point(245, 5);
            this.btnReload.Size = new System.Drawing.Size(120, 25);
            this.btnReload.Click += new System.EventHandler(this.BtnReload_Click);

            // rtbHelp
            this.rtbHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHelp.Font = new System.Drawing.Font("Consolas", 10F);
            this.rtbHelp.ReadOnly = true;
            this.rtbHelp.BackColor = System.Drawing.Color.White;
            this.rtbHelp.WordWrap = true;
            this.rtbHelp.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.rtbHelp.DetectUrls = true;

            // StroevkaHelpDoc
            this.Controls.Add(this.rtbHelp);
            this.Controls.Add(this.panelButtons);
            this.Name = "StroevkaHelpDoc";
            this.Size = new System.Drawing.Size(700, 500);

            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
