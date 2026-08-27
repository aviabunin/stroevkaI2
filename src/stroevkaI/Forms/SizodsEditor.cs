// stroevkaI/Forms/SizodsEditor.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;

namespace stroevkaI.Forms
{
    public partial class SizodsEditor : UserControl
    {
        private readonly SizodsRepository _repository;
        private readonly int _subdivisionId;
        private List<Sizod> _currentData;
        private bool _isSaving = false;
        private System.Windows.Forms.Timer _statusTimer;
        private Label _statusLabel;

        public event EventHandler DataChanged;
        public event EventHandler SaveRequested;

        public SizodsEditor()
        {
            InitializeComponent();
            _repository = new SizodsRepository(new stroevkaContext());
            SetupDataGridView();
            InitializeStatusLabel();
        }

        public SizodsEditor(int subdivisionId) : this()
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
                if (dgvSizods.Focused || dgvSizods.ContainsFocus)
                {
                    if (dgvSizods.CurrentCell != null)
                    {
                        int row = dgvSizods.CurrentCell.RowIndex;
                        int col = dgvSizods.CurrentCell.ColumnIndex;

                        if (dgvSizods.IsCurrentCellInEditMode)
                        {
                            dgvSizods.EndEdit();
                        }

                        // Определяем целевую ячейку (последовательный переход)
                        int targetRow = row;
                        int targetCol = -1;

                        // Колонки: 2-Расчет, 3-Резерв, 4-Пост ГДЗС, 5-База ГДЗС
                        if (col == 2) // Расчет -> Резерв
                        {
                            targetCol = 3;
                            targetRow = row;
                        }
                        else if (col == 3) // Резерв -> Пост ГДЗС
                        {
                            targetCol = 4;
                            targetRow = row;
                        }
                        else if (col == 4) // Пост ГДЗС -> База ГДЗС
                        {
                            targetCol = 5;
                            targetRow = row;
                        }
                        else if (col == 5) // База ГДЗС -> следующая строка (Расчет)
                        {
                            targetCol = 2;
                            targetRow = (row + 1 < dgvSizods.Rows.Count) ? row + 1 : row;
                        }

                        if (targetCol >= 0 && targetRow < dgvSizods.Rows.Count)
                        {
                            dgvSizods.CurrentCell = dgvSizods.Rows[targetRow].Cells[targetCol];
                            dgvSizods.BeginEdit(true);
                            var textBox = dgvSizods.EditingControl as TextBox;
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
            dgvSizods.Columns.Clear();

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
                HeaderText = "Средство",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            };

            var colRaschet = new DataGridViewTextBoxColumn
            {
                Name = "colRaschet",
                HeaderText = "Расчет",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            var colRezerv = new DataGridViewTextBoxColumn
            {
                Name = "colRezerv",
                HeaderText = "Резерв",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            var colPostGdzs = new DataGridViewTextBoxColumn
            {
                Name = "colPostGdzs",
                HeaderText = "Пост ГДЗС",
                Width = 70,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            var colBazaGdzs = new DataGridViewTextBoxColumn
            {
                Name = "colBazaGdzs",
                HeaderText = "База ГДЗС",
                Width = 70,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            dgvSizods.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colRaschet, colRezerv, colPostGdzs, colBazaGdzs });

            // ========== НАСТРОЙКА ЦВЕТОВ ==========

            dgvSizods.EnableHeadersVisualStyles = false;

            dgvSizods.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            dgvSizods.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvSizods.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            dgvSizods.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dgvSizods.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSizods.ColumnHeadersHeight = 25;

            dgvSizods.BackgroundColor = Color.White;

            dgvSizods.DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 216, 230);
            dgvSizods.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvSizods.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            dgvSizods.RowHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dgvSizods.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

            // ========== ОСТАЛЬНЫЕ НАСТРОЙКИ ==========

            dgvSizods.AllowUserToAddRows = false;
            dgvSizods.AllowUserToDeleteRows = false;
            dgvSizods.ReadOnly = false;
            dgvSizods.RowHeadersVisible = false;
            dgvSizods.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvSizods.MultiSelect = false;
            dgvSizods.BorderStyle = BorderStyle.Fixed3D;
            dgvSizods.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvSizods.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvSizods.RowTemplate.Height = 22;

            dgvSizods.StandardTab = false;

            dgvSizods.CellEndEdit += DgvSizods_CellEndEdit;
            dgvSizods.CellEnter += Dgv_CellEnter;
            dgvSizods.LostFocus += DgvSizods_LostFocus;
        }

        private void DgvSizods_LostFocus(object sender, EventArgs e)
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

            // Разрешаем редактирование для числовых колонок (2, 3, 4, 5)
            if (e.ColumnIndex >= 2 && e.ColumnIndex <= 5)
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
            _currentData = _repository.LoadSizods(_subdivisionId);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            int selectedId = -1;
            if (dgvSizods.CurrentRow?.Tag is Sizod currentItem)
            {
                selectedId = currentItem.Id;
            }

            dgvSizods.Rows.Clear();
            if (_currentData == null) return;

            int selectedRowIndex = -1;

            foreach (var item in _currentData.OrderBy(s => s.Norder))
            {
                int idx = dgvSizods.Rows.Add();
                var row = dgvSizods.Rows[idx];
                row.Cells["colId"].Value = item.Id;
                row.Cells["colName"].Value = item.Mname;
                row.Cells["colRaschet"].Value = item.Raschet;
                row.Cells["colRezerv"].Value = item.Rezerv;
                row.Cells["colPostGdzs"].Value = item.PostGdzs;
                row.Cells["colBazaGdzs"].Value = item.BazaGdzs;
                row.Tag = item;

                if (selectedId > 0 && item.Id == selectedId)
                {
                    selectedRowIndex = idx;
                }
            }

            if (selectedRowIndex >= 0 && selectedRowIndex < dgvSizods.Rows.Count)
            {
                dgvSizods.CurrentCell = dgvSizods.Rows[selectedRowIndex].Cells[1];
            }
            else
            {
                dgvSizods.ClearSelection();
            }
        }

        private List<Sizod> GetDataFromGrid()
        {
            var result = new List<Sizod>();
            foreach (DataGridViewRow row in dgvSizods.Rows)
            {
                if (row.IsNewRow || !(row.Tag is Sizod item)) continue;

                if (row.Cells["colRaschet"].Value != null)
                {
                    if (int.TryParse(row.Cells["colRaschet"].Value.ToString(), out int raschet))
                        item.Raschet = raschet;
                }
                if (row.Cells["colRezerv"].Value != null)
                {
                    if (int.TryParse(row.Cells["colRezerv"].Value.ToString(), out int rezerv))
                        item.Rezerv = rezerv;
                }
                if (row.Cells["colPostGdzs"].Value != null)
                {
                    if (int.TryParse(row.Cells["colPostGdzs"].Value.ToString(), out int postGdzs))
                        item.PostGdzs = postGdzs;
                }
                if (row.Cells["colBazaGdzs"].Value != null)
                {
                    if (int.TryParse(row.Cells["colBazaGdzs"].Value.ToString(), out int bazaGdzs))
                        item.BazaGdzs = bazaGdzs;
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
                bool result = _repository.SaveSizods(data);

                if (result)
                {
                    _currentData = _repository.LoadSizods(_subdivisionId);

                    foreach (DataGridViewRow row in dgvSizods.Rows)
                    {
                        if (row.Tag is Sizod oldItem && oldItem.Id == 0)
                        {
                            var newItem = _currentData.FirstOrDefault(s => s.Mname == oldItem.Mname);
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

        private void DgvSizods_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvSizods.Rows[e.RowIndex];
            if (row.Tag is Sizod item)
            {
                bool changed = false;

                if (row.Cells["colRaschet"].Value != null)
                {
                    if (int.TryParse(row.Cells["colRaschet"].Value.ToString(), out int raschet))
                    {
                        if (item.Raschet != raschet)
                        {
                            item.Raschet = raschet;
                            changed = true;
                        }
                    }
                }
                if (row.Cells["colRezerv"].Value != null)
                {
                    if (int.TryParse(row.Cells["colRezerv"].Value.ToString(), out int rezerv))
                    {
                        if (item.Rezerv != rezerv)
                        {
                            item.Rezerv = rezerv;
                            changed = true;
                        }
                    }
                }
                if (row.Cells["colPostGdzs"].Value != null)
                {
                    if (int.TryParse(row.Cells["colPostGdzs"].Value.ToString(), out int postGdzs))
                    {
                        if (item.PostGdzs != postGdzs)
                        {
                            item.PostGdzs = postGdzs;
                            changed = true;
                        }
                    }
                }
                if (row.Cells["colBazaGdzs"].Value != null)
                {
                    if (int.TryParse(row.Cells["colBazaGdzs"].Value.ToString(), out int bazaGdzs))
                    {
                        if (item.BazaGdzs != bazaGdzs)
                        {
                            item.BazaGdzs = bazaGdzs;
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
                MessageBox.Show("Данные СИЗОД сохранены.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка сохранения СИЗОД.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDataChanged() => DataChanged?.Invoke(this, EventArgs.Empty);
        private void OnSaveRequested() => SaveRequested?.Invoke(this, EventArgs.Empty);
    }
}