// stroevkaI/Forms/KostymsEditor.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;

namespace stroevkaI.Forms
{
    public partial class KostymsEditor : UserControl
    {
        private readonly KostymsRepository _repository;
        private readonly int _subdivisionId;
        private List<Kostym> _currentData;
        private bool _isSaving = false;
        private System.Windows.Forms.Timer _statusTimer;
        private Label _statusLabel;

        public event EventHandler DataChanged;
        public event EventHandler SaveRequested;

        public KostymsEditor()
        {
            InitializeComponent();
            _repository = new KostymsRepository(new stroevkaContext());
            SetupDataGridView();
            InitializeStatusLabel();
        }

        public KostymsEditor(int subdivisionId) : this()
        {
            _subdivisionId = subdivisionId;
            LoadData();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository?.Dispose();
                _statusTimer?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        // Переопределяем ProcessCmdKey для перехвата Enter на уровне UserControl
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (dgvKostyms.Focused || dgvKostyms.ContainsFocus)
                {
                    if (dgvKostyms.CurrentCell != null)
                    {
                        int row = dgvKostyms.CurrentCell.RowIndex;
                        int col = dgvKostyms.CurrentCell.ColumnIndex;

                        if (dgvKostyms.IsCurrentCellInEditMode)
                        {
                            dgvKostyms.EndEdit();
                        }

                        // Определяем целевую ячейку
                        int targetRow = row;
                        int targetCol = -1;

                        if (col == 2) // Кол-во -> следующая строка (Кол-во)
                        {
                            targetCol = 2;
                            targetRow = (row + 1 < dgvKostyms.Rows.Count) ? row + 1 : row;
                        }

                        if (targetCol >= 0 && targetRow < dgvKostyms.Rows.Count)
                        {
                            dgvKostyms.CurrentCell = dgvKostyms.Rows[targetRow].Cells[targetCol];
                            dgvKostyms.BeginEdit(true);
                            var textBox = dgvKostyms.EditingControl as TextBox;
                            if (textBox != null)
                            {
                                textBox.SelectAll();
                            }
                        }

                        ShowSavedStatus();
                        return true;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void InitializeStatusLabel()
        {
            _statusLabel = new Label
            {
                Text = "",
                AutoSize = true,
                Location = new Point(80, 10),
                ForeColor = Color.Green,
                Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold)
            };
            panelButtons.Controls.Add(_statusLabel);

            _statusTimer = new System.Windows.Forms.Timer
            {
                Interval = 1500
            };
            _statusTimer.Tick += (s, e) => {
                _statusLabel.Text = "";
                _statusTimer.Enabled = false;
            };
        }

        private void SetupDataGridView()
        {
            dgvKostyms.Columns.Clear();

            var colId = new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "Id",
                Visible = false,
                ReadOnly = true
            };

            var colName = new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Марка",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            };

            var colCount = new DataGridViewTextBoxColumn
            {
                Name = "colCount",
                HeaderText = "Кол-во",
                Width = 70,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            dgvKostyms.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colCount });

            // ========== НАСТРОЙКА ЦВЕТОВ ==========

            dgvKostyms.EnableHeadersVisualStyles = false;

            dgvKostyms.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            dgvKostyms.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvKostyms.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            dgvKostyms.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dgvKostyms.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvKostyms.ColumnHeadersHeight = 25;

            dgvKostyms.BackgroundColor = Color.White;

            dgvKostyms.DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 216, 230);
            dgvKostyms.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvKostyms.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            dgvKostyms.RowHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dgvKostyms.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

            // ========== ОСТАЛЬНЫЕ НАСТРОЙКИ ==========

            dgvKostyms.AllowUserToAddRows = false;
            dgvKostyms.AllowUserToDeleteRows = false;
            dgvKostyms.ReadOnly = false;
            dgvKostyms.RowHeadersVisible = false;
            dgvKostyms.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvKostyms.MultiSelect = false;
            dgvKostyms.BorderStyle = BorderStyle.Fixed3D;
            dgvKostyms.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvKostyms.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvKostyms.RowTemplate.Height = 22;

            dgvKostyms.StandardTab = false;

            dgvKostyms.CellEndEdit += DgvKostyms_CellEndEdit;
            dgvKostyms.CellEnter += Dgv_CellEnter;
            dgvKostyms.LostFocus += DgvKostyms_LostFocus;
        }

        private void DgvKostyms_LostFocus(object sender, EventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid?.IsCurrentCellInEditMode == true)
            {
                grid.EndEdit();
            }
        }

        private void ShowSavedStatus()
        {
            if (_statusLabel != null)
            {
                this.BeginInvoke(new Action(() => {
                    _statusLabel.Text = "✅ Сохранено";
                    _statusTimer.Enabled = true;
                }));
            }
        }

        private void Dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var grid = sender as DataGridView;
            if (grid == null) return;

            var row = grid.Rows[e.RowIndex];
            if (row.IsNewRow) return;

            if (e.ColumnIndex == 2) // Кол-во
            {
                grid.BeginEdit(true);
                var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.IsInEditMode)
                {
                    var textBox = grid.EditingControl as TextBox;
                    if (textBox != null)
                    {
                        textBox.SelectAll();
                    }
                }
            }
        }

        public void LoadData()
        {
            if (_repository == null) return;
            _currentData = _repository.LoadKostyms(_subdivisionId);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            int selectedId = -1;
            if (dgvKostyms.CurrentRow?.Tag is Kostym currentItem)
            {
                selectedId = currentItem.Id;
            }

            dgvKostyms.Rows.Clear();
            if (_currentData == null) return;

            int selectedRowIndex = -1;

            foreach (var item in _currentData.OrderBy(k => k.Norder))
            {
                int idx = dgvKostyms.Rows.Add();
                var row = dgvKostyms.Rows[idx];
                row.Cells["colId"].Value = item.Id;
                row.Cells["colName"].Value = item.Mname;
                row.Cells["colCount"].Value = item.N;
                row.Tag = item;

                if (selectedId > 0 && item.Id == selectedId)
                {
                    selectedRowIndex = idx;
                }
            }

            if (selectedRowIndex >= 0 && selectedRowIndex < dgvKostyms.Rows.Count)
            {
                dgvKostyms.CurrentCell = dgvKostyms.Rows[selectedRowIndex].Cells[1];
            }
            else
            {
                dgvKostyms.ClearSelection();
            }
        }

        private List<Kostym> GetDataFromGrid()
        {
            var result = new List<Kostym>();
            foreach (DataGridViewRow row in dgvKostyms.Rows)
            {
                if (row.IsNewRow || !(row.Tag is Kostym item)) continue;

                if (row.Cells["colCount"].Value != null)
                {
                    if (int.TryParse(row.Cells["colCount"].Value.ToString(), out int count))
                        item.N = count;
                }
                result.Add(item);
            }
            return result;
        }

        private bool SaveData(bool reloadData = false)
        {
            if (_isSaving) return true;
            _isSaving = true;

            try
            {
                var data = GetDataFromGrid();
                bool result = _repository.SaveKostyms(data);

                if (result)
                {
                    _currentData = _repository.LoadKostyms(_subdivisionId);

                    foreach (DataGridViewRow row in dgvKostyms.Rows)
                    {
                        if (row.Tag is Kostym oldItem && oldItem.Id == 0)
                        {
                            var newItem = _currentData.FirstOrDefault(k => k.Mname == oldItem.Mname);
                            if (newItem != null)
                            {
                                oldItem.Id = newItem.Id;
                                row.Cells["colId"].Value = newItem.Id;
                            }
                        }
                    }

                    OnSaveRequested();
                }
                return result;
            }
            finally
            {
                _isSaving = false;
            }
        }

        private void DgvKostyms_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvKostyms.Rows[e.RowIndex];
            if (row.Tag is Kostym item)
            {
                bool changed = false;

                if (row.Cells["colCount"].Value != null)
                {
                    if (int.TryParse(row.Cells["colCount"].Value.ToString(), out int count))
                    {
                        if (item.N != count)
                        {
                            item.N = count;
                            changed = true;
                        }
                    }
                }

                if (changed)
                {
                    SaveData(reloadData: false);
                    OnDataChanged();
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (SaveData(reloadData: true))
            {
                ShowSavedStatus();
                MessageBox.Show("Данные костюмов сохранены.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка сохранения костюмов.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDataChanged() => DataChanged?.Invoke(this, EventArgs.Empty);
        private void OnSaveRequested() => SaveRequested?.Invoke(this, EventArgs.Empty);
    }
}