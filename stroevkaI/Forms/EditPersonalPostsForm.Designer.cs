// stroevkaI/Forms/EditPersonalPostsForm.Designer.cs
namespace stroevkaI.Forms
{
    partial class EditPersonalPostsForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.lblPersonal = new System.Windows.Forms.Label();
            this.lblSubdivision = new System.Windows.Forms.Label();
            this.clbPosts = new System.Windows.Forms.CheckedListBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();

            // lblPersonal
            this.lblPersonal.AutoSize = true;
            this.lblPersonal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblPersonal.Location = new System.Drawing.Point(12, 9);
            this.lblPersonal.Name = "lblPersonal";
            this.lblPersonal.Size = new System.Drawing.Size(100, 17);
            this.lblPersonal.TabIndex = 0;
            this.lblPersonal.Text = "Сотрудник: ";

            // lblSubdivision
            this.lblSubdivision.AutoSize = true;
            this.lblSubdivision.Location = new System.Drawing.Point(12, 32);
            this.lblSubdivision.Name = "lblSubdivision";
            this.lblSubdivision.Size = new System.Drawing.Size(100, 13);
            this.lblSubdivision.TabIndex = 1;
            this.lblSubdivision.Text = "Подразделение: ";

            // clbPosts
            this.clbPosts.CheckOnClick = true;
            this.clbPosts.FormattingEnabled = true;
            this.clbPosts.Location = new System.Drawing.Point(15, 58);
            this.clbPosts.Name = "clbPosts";
            this.clbPosts.Size = new System.Drawing.Size(400, 304);
            this.clbPosts.TabIndex = 2;
            this.clbPosts.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ClbPosts_ItemCheck);

            // lblCount
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(12, 370);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(68, 13);
            this.lblCount.TabIndex = 3;
            this.lblCount.Text = "Выбрано: 0";

            // panelButtons
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 400);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(430, 45);
            this.panelButtons.TabIndex = 4;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(260, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(341, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            // EditPersonalPostsForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 445);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.clbPosts);
            this.Controls.Add(this.lblSubdivision);
            this.Controls.Add(this.lblPersonal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditPersonalPostsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактирование должностей";
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // --- Компоненты ---
        private System.Windows.Forms.Label lblPersonal;
        private System.Windows.Forms.Label lblSubdivision;
        private System.Windows.Forms.CheckedListBox clbPosts;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}