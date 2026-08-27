// stroevkaI/Forms/PenasEditor.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;

namespace stroevkaI.Forms
{
    public partial class PenasEditor : UserControl
    {
        private readonly PenasRepository _repository;
        private readonly int _subdivisionId;
        private List<Pena> _currentData;
        private bool _isSaving = false;
        private System.Windows.Forms.Timer _statusTimer;
        private Label _statusLabel;

        public event EventHandler DataChanged;
        public event EventHandler SaveRequested;

        public PenasEditor()
        {
            InitializeComponent();
            _repository = new PenasRepository(new stroevkaContext());
            SetupDataGridView();
            InitializeStatusLabel();
        }

        public PenasEditor(int subdivisionId) : this()
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
                if (dgvPenas.Focused || dgvPenas.ContainsFocus)
                {
                    if (dgvPenas.CurrentCell != null)
                    {
                        int row = dgvPenas.CurrentCell.RowIndex;
                        int col = dgvPenas.CurrentCell.ColumnIndex;

                        if (dgvPenas.IsCurrentCellInEditMode)
                        {
                            dgvPenas.EndEdit();
                        }

                        int targetRow = row;
                        int targetCol = -1;

                        if (col == 2)
                        {
                            targetCol = 3;
                            targetRow = row;
                        }
                        else if (col == 3)
                        {
                            targetCol = 2;
                            targetRow = (row + 1 < dgvPenas.Rows.Count) ? row + 1 : row;
                        }

                        if (targetCol >= 0 && targetRow < dgvPenas.Rows.Count)
                        {
                            dgvPenas.CurrentCell = dgvPenas.Rows[targetRow].Cells[targetCol];
                            dgvPenas.BeginEdit(true);
                            var textBox = dgvPenas.EditingControl as TextBox;
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
            //dgvPenas.Columns.Clear();

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
                HeaderText = "Пенообразователь",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            };

            var colInwork = new DataGridViewTextBoxColumn
            {
                Name = "colInwork",
                HeaderText = "В работе",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            var colInrezerv = new DataGridViewTextBoxColumn
            {
                Name = "colInrezerv",
                HeaderText = "В резерве",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

         //   dgvPenas.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colInwork, colInrezerv });

            // ========== НАСТРОЙКА ЦВЕТОВ ==========

            // Отключаем визуальные стили, чтобы управлять цветами самим
            dgvPenas.EnableHeadersVisualStyles = false;

            // Заголовки колонок
            dgvPenas.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            dgvPenas.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvPenas.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            dgvPenas.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dgvPenas.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvPenas.ColumnHeadersHeight = 25;

            // Цвет фона ячеек
            dgvPenas.BackgroundColor = Color.White;

            // Цвет выделенной ячейки — СВЕТЛО-ГОЛУБОЙ
            dgvPenas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 216, 230); // Светло-голубой
            dgvPenas.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Альтернативный цвет строк
            dgvPenas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            // Отключаем изменение цвета заголовка строки при выделении
            dgvPenas.RowHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dgvPenas.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

            // ========== ОСТАЛЬНЫЕ НАСТРОЙКИ ==========

            dgvPenas.AllowUserToAddRows = false;
            dgvPenas.AllowUserToDeleteRows = false;
            dgvPenas.ReadOnly = false;
            dgvPenas.RowHeadersVisible = false;
            dgvPenas.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvPenas.MultiSelect = false;
            dgvPenas.BorderStyle = BorderStyle.Fixed3D;
            dgvPenas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPenas.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvPenas.RowTemplate.Height = 22;

            // Отключаем стандартную навигацию Enter
            dgvPenas.StandardTab = false;

            dgvPenas.CellEndEdit += DgvPenas_CellEndEdit;
            dgvPenas.CellEnter += Dgv_CellEnter;
            dgvPenas.LostFocus += DgvPenas_LostFocus;
        }

        private void DgvPenas_LostFocus(object sender, EventArgs e)
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

            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
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
            _currentData = _repository.LoadPenas(_subdivisionId);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            int selectedId = -1;
            if (dgvPenas.CurrentRow?.Tag is Pena currentItem)
            {
                selectedId = currentItem.Id;
            }

            dgvPenas.Rows.Clear();
            if (_currentData == null) return;

            int selectedRowIndex = -1;

            foreach (var item in _currentData.OrderBy(p => p.Norder))
            {
                int idx = dgvPenas.Rows.Add();
                var row = dgvPenas.Rows[idx];
                row.Cells["colId"].Value = item.Id;
                row.Cells["colName"].Value = item.Mname;
                row.Cells["colInwork"].Value = item.Inwork;
                row.Cells["colInrezerv"].Value = item.Inrezerv;
                row.Tag = item;

                if (selectedId > 0 && item.Id == selectedId)
                {
                    selectedRowIndex = idx;
                }
            }

            if (selectedRowIndex >= 0 && selectedRowIndex < dgvPenas.Rows.Count)
            {
                dgvPenas.CurrentCell = dgvPenas.Rows[selectedRowIndex].Cells[1];
            }
            else
            {
                dgvPenas.ClearSelection();
            }
        }

        private List<Pena> GetDataFromGrid()
        {
            var result = new List<Pena>();
            foreach (DataGridViewRow row in dgvPenas.Rows)
            {
                if (row.IsNewRow || !(row.Tag is Pena item)) continue;

                if (row.Cells["colInwork"].Value != null)
                {
                    if (int.TryParse(row.Cells["colInwork"].Value.ToString(), out int inwork))
                        item.Inwork = inwork;
                }
                if (row.Cells["colInrezerv"].Value != null)
                {
                    if (int.TryParse(row.Cells["colInrezerv"].Value.ToString(), out int inrezerv))
                        item.Inrezerv = inrezerv;
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
                bool result = _repository.SavePenas(data);

                if (result)
                {
                    _currentData = _repository.LoadPenas(_subdivisionId);

                    foreach (DataGridViewRow row in dgvPenas.Rows)
                    {
                        if (row.Tag is Pena oldItem && oldItem.Id == 0)
                        {
                            var newItem = _currentData.FirstOrDefault(p => p.Mname == oldItem.Mname);
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

        private void DgvPenas_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvPenas.Rows[e.RowIndex];
            if (row.Tag is Pena item)
            {
                bool changed = false;

                if (row.Cells["colInwork"].Value != null)
                {
                    if (int.TryParse(row.Cells["colInwork"].Value.ToString(), out int inwork))
                    {
                        if (item.Inwork != inwork)
                        {
                            item.Inwork = inwork;
                            changed = true;
                        }
                    }
                }
                if (row.Cells["colInrezerv"].Value != null)
                {
                    if (int.TryParse(row.Cells["colInrezerv"].Value.ToString(), out int inrezerv))
                    {
                        if (item.Inrezerv != inrezerv)
                        {
                            item.Inrezerv = inrezerv;
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
                MessageBox.Show("Данные пены сохранены.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка сохранения пены.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDataChanged() => DataChanged?.Invoke(this, EventArgs.Empty);
        private void OnSaveRequested() => SaveRequested?.Invoke(this, EventArgs.Empty);
    }
}