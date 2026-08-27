// TransferOperationForm.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using stroevkaI.Models;

namespace stroevkaI.Forms
{
    public partial class TransferOperationForm : Form
    {
        public TransferOperation SelectedOperation { get; private set; }
        public string SelectedState { get; private set; }

        private readonly List<TransferOperation> _operations;
        private readonly string _currentState;
        private readonly string _itemName;

        public TransferOperationForm(string itemName, string currentState)
        {
            InitializeComponent();
            _itemName = itemName;
            _currentState = currentState;
            _operations = TransferOperations.GetOperationsForState(currentState);

            LoadOperations();
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.Text = "Выберите операцию";
            this.Size = new Size(350, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Заголовок
            var lblTitle = new Label
            {
                Text = $"Перевод техники: {_itemName}",
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(320, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Информация о текущем состоянии
            var lblState = new Label
            {
                Text = $"Текущее состояние: {TransferOperations.GetStateDisplayName(_currentState)}",
                Location = new Point(10, 40),
                Size = new Size(320, 20),
                ForeColor = TransferOperations.GetStateColor(_currentState)
            };

            // Список операций
            var listView = new ListView
            {
                Location = new Point(10, 70),
                Size = new Size(320, 250),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false,
                HeaderStyle = ColumnHeaderStyle.None
            };
            listView.Columns.Add("Операция", 180);
            listView.Columns.Add("Переход", 120);
            listView.Name = "listView";

            // Кнопки
            var btnOk = new Button
            {
                Text = "Выбрать",
                Location = new Point(180, 330),
                Size = new Size(70, 30),
                DialogResult = DialogResult.OK,
                Enabled = false
            };

            var btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(260, 330),
                Size = new Size(70, 30),
                DialogResult = DialogResult.Cancel
            };

            // Добавляем элементы
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblState);
            this.Controls.Add(listView);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);

            // События
            listView.SelectedIndexChanged += (s, e) =>
            {
                btnOk.Enabled = listView.SelectedItems.Count > 0;
            };

            listView.DoubleClick += (s, e) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    btnOk.PerformClick();
                }
            };

            btnOk.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    var tag = listView.SelectedItems[0].Tag as TransferOperation;
                    if (tag != null)
                    {
                        SelectedOperation = tag;
                        SelectedState = tag.ToState;
                    }
                }
            };
        }

        private void LoadOperations()
        {
            var listView = this.Controls.Find("listView", true)[0] as ListView;
            if (listView == null) return;

            listView.Items.Clear();

            if (_operations.Count == 0)
            {
                var item = new ListViewItem("Нет доступных операций");
                item.ForeColor = Color.Gray;
                listView.Items.Add(item);
                return;
            }

            foreach (var op in _operations)
            {
                var item = new ListViewItem(op.DisplayName);
                item.SubItems.Add(op.Icon);
                item.Tag = op;

                // Цвет для перехода
                var fromColor = TransferOperations.GetStateColor(op.FromState);
                var toColor = TransferOperations.GetStateColor(op.ToState);
                item.ForeColor = fromColor;

                listView.Items.Add(item);
            }
        }

        private void SetupUI()
        {
            // Настройка внешнего вида
            this.BackColor = SystemColors.Control;
        }
    }
}
