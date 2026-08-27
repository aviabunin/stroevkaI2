namespace stroevkaI.Controls
{
    partial class PsgListViewControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.CheckBox showFullNamesCheckBox;
        private System.Windows.Forms.Panel infoPanel;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button backButton;

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
            this.listBox = new System.Windows.Forms.ListBox();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.showFullNamesCheckBox = new System.Windows.Forms.CheckBox();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.infoLabel = new System.Windows.Forms.Label();
            this.backButton = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // listBox
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.listBox.IntegralHeight = false;
            this.listBox.Location = new System.Drawing.Point(0, 23);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(280, 297);
            this.listBox.TabIndex = 1;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
            this.listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseDoubleClick);

            // searchBox
            this.searchBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.searchBox.Location = new System.Drawing.Point(0, 0);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(280, 21);
            this.searchBox.TabIndex = 0;
            this.searchBox.Text = "Поиск...";
            this.searchBox.Enter += new System.EventHandler(this.SearchBox_Enter);
            this.searchBox.Leave += new System.EventHandler(this.SearchBox_Leave);
            this.searchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);

            // showFullNamesCheckBox
            this.showFullNamesCheckBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.showFullNamesCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showFullNamesCheckBox.Location = new System.Drawing.Point(0, 0);
            this.showFullNamesCheckBox.Name = "showFullNamesCheckBox";
            this.showFullNamesCheckBox.Size = new System.Drawing.Size(280, 25);
            this.showFullNamesCheckBox.TabIndex = 2;
            this.showFullNamesCheckBox.Text = "Полные названия";
            this.showFullNamesCheckBox.UseVisualStyleBackColor = true;
            this.showFullNamesCheckBox.Checked = false;
            this.showFullNamesCheckBox.CheckedChanged += new System.EventHandler(this.ShowFullNamesCheckBox_CheckedChanged);

            // infoPanel
            this.infoPanel.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.infoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.infoPanel.Location = new System.Drawing.Point(0, 320);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(5);
            this.infoPanel.Size = new System.Drawing.Size(280, 40);
            this.infoPanel.TabIndex = 3;

            // infoLabel
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.infoLabel.ForeColor = System.Drawing.Color.DarkGray;
            this.infoLabel.Location = new System.Drawing.Point(5, 5);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(268, 28);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Выберите гарнизон";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.infoPanel.Controls.Add(this.infoLabel);

            // backButton
            this.backButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.backButton.Location = new System.Drawing.Point(0, 360);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(280, 25);
            this.backButton.TabIndex = 4;
            this.backButton.Text = "◄ Назад к территориальному";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Visible = false;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);

            // PsgListViewControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.showFullNamesCheckBox);

            // Расположение элементов
            this.showFullNamesCheckBox.Dock = DockStyle.Bottom;
            this.backButton.Dock = DockStyle.Bottom;
            this.infoPanel.Dock = DockStyle.Bottom;

            this.Name = "PsgListViewControl";
            this.Size = new System.Drawing.Size(280, 400);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}