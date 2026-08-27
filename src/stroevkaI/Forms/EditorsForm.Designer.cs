// stroevkaI/Forms/EditorsForm.Designer.cs
namespace stroevkaI.Forms
{
    partial class EditorsForm
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabContacts = new System.Windows.Forms.TabPage();
            this.tabSredstva = new System.Windows.Forms.TabPage();
            this.tabSostav = new System.Windows.Forms.TabPage();
            this.tabPersonals = new System.Windows.Forms.TabPage();
            this.tabResources = new System.Windows.Forms.TabPage(); // Добавляем вкладку

            this.tabControl1.SuspendLayout();
            this.SuspendLayout();

            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabContacts);
            this.tabControl1.Controls.Add(this.tabSredstva);
            this.tabControl1.Controls.Add(this.tabSostav);
            this.tabControl1.Controls.Add(this.tabPersonals);
            this.tabControl1.Controls.Add(this.tabResources); // Добавляем
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(760, 477);
            this.tabControl1.TabIndex = 0;

            // 
            // tabContacts
            // 
            this.tabContacts.Location = new System.Drawing.Point(4, 22);
            this.tabContacts.Name = "tabContacts";
            this.tabContacts.Padding = new System.Windows.Forms.Padding(3);
            this.tabContacts.Size = new System.Drawing.Size(752, 451);
            this.tabContacts.TabIndex = 0;
            this.tabContacts.Text = "Контакты";
            this.tabContacts.UseVisualStyleBackColor = true;

            // 
            // tabSredstva
            // 
            this.tabSredstva.Location = new System.Drawing.Point(4, 22);
            this.tabSredstva.Name = "tabSredstva";
            this.tabSredstva.Padding = new System.Windows.Forms.Padding(3);
            this.tabSredstva.Size = new System.Drawing.Size(752, 451);
            this.tabSredstva.TabIndex = 1;
            this.tabSredstva.Text = "Средства";
            this.tabSredstva.UseVisualStyleBackColor = true;

            // 
            // tabSostav
            // 
            this.tabSostav.Location = new System.Drawing.Point(4, 22);
            this.tabSostav.Name = "tabSostav";
            this.tabSostav.Padding = new System.Windows.Forms.Padding(3);
            this.tabSostav.Size = new System.Drawing.Size(752, 451);
            this.tabSostav.TabIndex = 2;
            this.tabSostav.Text = "Состав";
            this.tabSostav.UseVisualStyleBackColor = true;

            // 
            // tabPersonals
            // 
            this.tabPersonals.Location = new System.Drawing.Point(4, 22);
            this.tabPersonals.Name = "tabPersonals";
            this.tabPersonals.Padding = new System.Windows.Forms.Padding(3);
            this.tabPersonals.Size = new System.Drawing.Size(752, 451);
            this.tabPersonals.TabIndex = 3;
            this.tabPersonals.Text = "Сотрудники";
            this.tabPersonals.UseVisualStyleBackColor = true;

            // 
            // tabResources
            // 
            this.tabResources.Location = new System.Drawing.Point(4, 22);
            this.tabResources.Name = "tabResources";
            this.tabResources.Padding = new System.Windows.Forms.Padding(3);
            this.tabResources.Size = new System.Drawing.Size(752, 451);
            this.tabResources.TabIndex = 4;
            this.tabResources.Text = "Ресурсы";
            this.tabResources.UseVisualStyleBackColor = true;

            // 
            // EditorsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 477);
            this.Controls.Add(this.tabControl1);
            this.Name = "EditorsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редакторы";
            this.Load += new System.EventHandler(this.EditorsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabContacts;
        private System.Windows.Forms.TabPage tabSredstva;
        private System.Windows.Forms.TabPage tabSostav;
        private System.Windows.Forms.TabPage tabPersonals;
        private System.Windows.Forms.TabPage tabResources;
    }
}