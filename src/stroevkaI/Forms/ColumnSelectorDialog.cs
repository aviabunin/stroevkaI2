using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace stroevkaI
{
    public partial class ColumnSelectorDialog : Form
    {
        public List<string> SelectedColumns { get; private set; }
        private const string FirstColumnName = "ПЧ"; // должно совпадать с именем в менеджере

        public ColumnSelectorDialog(List<string> allColumns, List<string> initiallyChecked)
        {
            InitializeComponent();

            // Удаляем обязательную колонку из списка для выбора
            var filteredAll = new List<string>(allColumns);
            filteredAll.Remove(FirstColumnName);
            var filteredChecked = new List<string>(initiallyChecked ?? new List<string>());
            filteredChecked.Remove(FirstColumnName);

            checkedListBox1.Items.Clear();
            foreach (var col in filteredAll)
            {
                checkedListBox1.Items.Add(col, filteredChecked.Contains(col));
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedColumns = new List<string>();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                SelectedColumns.Add(item.ToString());
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, true);
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, false);
        }
    }
}