// stroevkaI/Forms/ContactsEditor.Designer.cs
namespace stroevkaI.Forms
{
    partial class ContactsEditor
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgvContacts = new System.Windows.Forms.DataGridView();
            this.dgvPersonals = new System.Windows.Forms.DataGridView();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.btnCloseRightPanel = new System.Windows.Forms.Button();  // Новая кнопка
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbPostFilter = new System.Windows.Forms.ComboBox();
            this.lblPostFilter = new System.Windows.Forms.Label();
            this.chkEditMode = new System.Windows.Forms.CheckBox();
            this.btnCancelContact = new System.Windows.Forms.Button();
            this.btnSaveContact = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCreateAllKarauls = new System.Windows.Forms.Button();
            this.btnDeleteContact = new System.Windows.Forms.Button();
            this.btnAddContact = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();

            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContacts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonals)).BeginInit();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();

            // splitContainer - ВЕРТИКАЛЬНЫЙ
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 80);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.splitContainer.Size = new System.Drawing.Size(1000, 600);
            this.splitContainer.SplitterDistance = 650;  // Больше места для левой панели
            this.splitContainer.TabIndex = 0;

            // dgvContacts - левая панель
            this.dgvContacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvContacts.AllowUserToAddRows = false;
            this.dgvContacts.AllowUserToDeleteRows = false;
            this.dgvContacts.ReadOnly = true;
            this.dgvContacts.RowHeadersVisible = false;
            this.dgvContacts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContacts.MultiSelect = false;
            this.dgvContacts.TabIndex = 0;
            this.dgvContacts.SelectionChanged += new System.EventHandler(this.DgvContacts_SelectionChanged);
            this.dgvContacts.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvContacts_CellDoubleClick);
            this.splitContainer.Panel1.Controls.Add(this.dgvContacts);

            // dgvPersonals - правая панель
            this.dgvPersonals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPersonals.AllowUserToAddRows = false;
            this.dgvPersonals.AllowUserToDeleteRows = false;
            this.dgvPersonals.ReadOnly = true;
            this.dgvPersonals.RowHeadersVisible = false;
            this.dgvPersonals.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPersonals.MultiSelect = false;
            this.dgvPersonals.TabIndex = 0;
            this.dgvPersonals.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvPersonals_CellDoubleClick);
            this.splitContainer.Panel2.Controls.Add(this.dgvPersonals);

            // buttonPanel
            this.buttonPanel.Controls.Add(this.btnCloseRightPanel);
            this.buttonPanel.Controls.Add(this.txtSearch);
            this.buttonPanel.Controls.Add(this.cmbPostFilter);
            this.buttonPanel.Controls.Add(this.lblPostFilter);
            this.buttonPanel.Controls.Add(this.chkEditMode);
            this.buttonPanel.Controls.Add(this.btnCancelContact);
            this.buttonPanel.Controls.Add(this.btnSaveContact);
            this.buttonPanel.Controls.Add(this.btnRefresh);
            this.buttonPanel.Controls.Add(this.btnCreateAllKarauls);
            this.buttonPanel.Controls.Add(this.btnDeleteContact);
            this.buttonPanel.Controls.Add(this.btnAddContact);
            this.buttonPanel.Controls.Add(this.lblTitle);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPanel.Location = new System.Drawing.Point(0, 0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(1000, 80);
            this.buttonPanel.TabIndex = 1;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(10, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 17);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Контакты караула";

            // btnAddContact
            this.btnAddContact.Location = new System.Drawing.Point(200, 6);
            this.btnAddContact.Name = "btnAddContact";
            this.btnAddContact.Size = new System.Drawing.Size(90, 28);
            this.btnAddContact.TabIndex = 2;
            this.btnAddContact.Text = "Добавить";
            this.btnAddContact.UseVisualStyleBackColor = true;
            this.btnAddContact.Click += new System.EventHandler(this.BtnAddContact_Click);

            // btnDeleteContact
            this.btnDeleteContact.Location = new System.Drawing.Point(296, 6);
            this.btnDeleteContact.Name = "btnDeleteContact";
            this.btnDeleteContact.Size = new System.Drawing.Size(90, 28);
            this.btnDeleteContact.TabIndex = 3;
            this.btnDeleteContact.Text = "Удалить";
            this.btnDeleteContact.UseVisualStyleBackColor = true;
            this.btnDeleteContact.Click += new System.EventHandler(this.BtnDeleteContact_Click);

            // btnCreateAllKarauls
            this.btnCreateAllKarauls.Location = new System.Drawing.Point(392, 6);
            this.btnCreateAllKarauls.Name = "btnCreateAllKarauls";
            this.btnCreateAllKarauls.Size = new System.Drawing.Size(170, 28);
            this.btnCreateAllKarauls.TabIndex = 4;
            this.btnCreateAllKarauls.Text = "Создать для всех караулов";
            this.btnCreateAllKarauls.UseVisualStyleBackColor = true;
            this.btnCreateAllKarauls.Click += new System.EventHandler(this.BtnCreateAllKarauls_Click);

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(568, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 28);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);

            // btnSaveContact
            this.btnSaveContact.Enabled = false;
            this.btnSaveContact.Location = new System.Drawing.Point(710, 6);
            this.btnSaveContact.Name = "btnSaveContact";
            this.btnSaveContact.Size = new System.Drawing.Size(90, 28);
            this.btnSaveContact.TabIndex = 6;
            this.btnSaveContact.Text = "Сохранить";
            this.btnSaveContact.UseVisualStyleBackColor = true;
            this.btnSaveContact.Click += new System.EventHandler(this.BtnSaveContact_Click);

            // btnCancelContact
            this.btnCancelContact.Enabled = false;
            this.btnCancelContact.Location = new System.Drawing.Point(806, 6);
            this.btnCancelContact.Name = "btnCancelContact";
            this.btnCancelContact.Size = new System.Drawing.Size(90, 28);
            this.btnCancelContact.TabIndex = 7;
            this.btnCancelContact.Text = "Отмена";
            this.btnCancelContact.UseVisualStyleBackColor = true;
            this.btnCancelContact.Click += new System.EventHandler(this.BtnCancelContact_Click);

            // lblPostFilter
            this.lblPostFilter.AutoSize = true;
            this.lblPostFilter.Location = new System.Drawing.Point(10, 42);
            this.lblPostFilter.Name = "lblPostFilter";
            this.lblPostFilter.Size = new System.Drawing.Size(114, 13);
            this.lblPostFilter.TabIndex = 9;
            this.lblPostFilter.Text = "Фильтр по должности:";

            // cmbPostFilter
            this.cmbPostFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPostFilter.Location = new System.Drawing.Point(130, 39);
            this.cmbPostFilter.Name = "cmbPostFilter";
            this.cmbPostFilter.Size = new System.Drawing.Size(200, 21);
            this.cmbPostFilter.TabIndex = 10;
            this.cmbPostFilter.SelectedIndexChanged += new System.EventHandler(this.CmbPostFilter_SelectedIndexChanged);

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(350, 39);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(150, 20);
            this.txtSearch.TabIndex = 11;
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);

            // chkEditMode - перенесён справа от полей поиска
            this.chkEditMode.AutoSize = true;
            this.chkEditMode.Location = new System.Drawing.Point(520, 41);
            this.chkEditMode.Name = "chkEditMode";
            this.chkEditMode.Size = new System.Drawing.Size(129, 17);
            this.chkEditMode.TabIndex = 8;
            this.chkEditMode.Text = "Режим редактирования";
            this.chkEditMode.UseVisualStyleBackColor = true;
            this.chkEditMode.CheckedChanged += new System.EventHandler(this.ChkEditMode_CheckedChanged);

            // btnCloseRightPanel - кнопка закрытия правой панели
            this.btnCloseRightPanel.Location = new System.Drawing.Point(900, 6);
            this.btnCloseRightPanel.Name = "btnCloseRightPanel";
            this.btnCloseRightPanel.Size = new System.Drawing.Size(80, 28);
            this.btnCloseRightPanel.TabIndex = 12;
            this.btnCloseRightPanel.Text = "Закрыть ▶";
            this.btnCloseRightPanel.UseVisualStyleBackColor = true;
            this.btnCloseRightPanel.Click += new System.EventHandler(this.BtnCloseRightPanel_Click);
            this.btnCloseRightPanel.Visible = false;  // Скрыта по умолчанию

            // ContactsEditor
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.buttonPanel);
            this.Name = "ContactsEditor";
            this.Size = new System.Drawing.Size(1000, 680);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContacts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonals)).EndInit();
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // --- Компоненты ---
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dgvContacts;
        private System.Windows.Forms.DataGridView dgvPersonals;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAddContact;
        private System.Windows.Forms.Button btnDeleteContact;
        private System.Windows.Forms.Button btnCreateAllKarauls;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSaveContact;
        private System.Windows.Forms.Button btnCancelContact;
        private System.Windows.Forms.CheckBox chkEditMode;
        private System.Windows.Forms.Label lblPostFilter;
        private System.Windows.Forms.ComboBox cmbPostFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnCloseRightPanel;  // Новая кнопка
    }
}