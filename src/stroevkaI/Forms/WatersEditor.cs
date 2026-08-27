// stroevkaI/Forms/WatersEditor.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;

namespace stroevkaI.Forms
{
    public partial class WatersEditor : UserControl
    {
        private readonly WatersRepository _repository;
        private readonly int _subdivisionId;
        private List<Water> _currentData;
        private bool _isSaving = false;
        private System.Windows.Forms.Timer _statusTimer;
        private Label _statusLabel;

        public event EventHandler DataChanged;
        public event EventHandler SaveRequested;

        public WatersEditor()
        {
            InitializeComponent();
            _repository = new WatersRepository(new stroevkaContext());
            SetupDataGridView();
            InitializeStatusLabel();
        }

        public WatersEditor(int subdivisionId) : this()
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
                if (dgvWaters.Focused || dgvWaters.ContainsFocus)
                {
                    if (dgvWaters.CurrentCell != null)
                    {
                        int row = dgvWaters.CurrentCell.RowIndex;
                        int col = dgvWaters.CurrentCell.ColumnIndex;

                        if (dgvWaters.IsCurrentCellInEditMode)
                        {
                            dgvWaters.EndEdit();
                        }

                        // Определяем целевую ячейку
                        int targetRow = row;
                        int targetCol = -1;

                        if (col == 2) // Всего -> Неиспр.
                        {
                            targetCol = 3;
                            targetRow = row;
                        }
                        else if (col == 3) // Неиспр. -> следующая строка (Всего)
                        {
                            targetCol = 2;
                            targetRow = (row + 1 < dgvWaters.Rows.Count) ? row + 1 : row;
                        }

                        if (targetCol >= 0 && targetRow < dgvWaters.Rows.Count)
                        {
                            dgvWaters.CurrentCell = dgvWaters.Rows[targetRow].Cells[targetCol];
                            dgvWaters.BeginEdit(true);
                            var textBox = dgvWaters.EditingControl as TextBox;
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
            dgvWaters.Columns.Clear();

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
                HeaderText = "Источник",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            };

            var colTotal = new DataGridViewTextBoxColumn
            {
                Name = "colTotal",
                HeaderText = "Всего",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            var colFault = new DataGridViewTextBoxColumn
            {
                Name = "colFault",
                HeaderText = "Неиспр.",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            dgvWaters.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colTotal, colFault });

            // ========== НАСТРОЙКА ЦВЕТОВ ==========

            dgvWaters.EnableHeadersVisualStyles = false;

            dgvWaters.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            dgvWaters.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvWaters.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            dgvWaters.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dgvWaters.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvWaters.ColumnHeadersHeight = 25;

            dgvWaters.BackgroundColor = Color.White;

            dgvWaters.DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 216, 230);
            dgvWaters.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvWaters.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            dgvWaters.RowHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dgvWaters.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

            // ========== ОСТАЛЬНЫЕ НАСТРОЙКИ ==========

            dgvWaters.AllowUserToAddRows = false;
            dgvWaters.AllowUserToDeleteRows = false;
            dgvWaters.ReadOnly = false;
            dgvWaters.RowHeadersVisible = false;
            dgvWaters.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvWaters.MultiSelect = false;
            dgvWaters.BorderStyle = BorderStyle.Fixed3D;
            dgvWaters.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvWaters.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvWaters.RowTemplate.Height = 22;

            dgvWaters.StandardTab = false;

            dgvWaters.CellEndEdit += DgvWaters_CellEndEdit;
            dgvWaters.CellEnter += Dgv_CellEnter;
            dgvWaters.LostFocus += DgvWaters_LostFocus;
        }

        private void DgvWaters_LostFocus(object sender, EventArgs e)
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
            _currentData = _repository.LoadWaters(_subdivisionId);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            int selectedId = -1;
            if (dgvWaters.CurrentRow?.Tag is Water currentItem)
            {
                selectedId = currentItem.Id;
            }

            dgvWaters.Rows.Clear();
            if (_currentData == null) return;

            int selectedRowIndex = -1;

            foreach (var item in _currentData.OrderBy(w => w.Norder))
            {
                int idx = dgvWaters.Rows.Add();
                var row = dgvWaters.Rows[idx];
                row.Cells["colId"].Value = item.Id;
                row.Cells["colName"].Value = item.Mname;
                row.Cells["colTotal"].Value = item.Total;
                row.Cells["colFault"].Value = item.Fault;
                row.Tag = item;

                if (selectedId > 0 && item.Id == selectedId)
                {
                    selectedRowIndex = idx;
                }
            }

            if (selectedRowIndex >= 0 && selectedRowIndex < dgvWaters.Rows.Count)
            {
                dgvWaters.CurrentCell = dgvWaters.Rows[selectedRowIndex].Cells[1];
            }
            else
            {
                dgvWaters.ClearSelection();
            }
        }

        private List<Water> GetDataFromGrid()
        {
            var result = new List<Water>();
            foreach (DataGridViewRow row in dgvWaters.Rows)
            {
                if (row.IsNewRow || !(row.Tag is Water item)) continue;

                if (row.Cells["colTotal"].Value != null)
                {
                    if (int.TryParse(row.Cells["colTotal"].Value.ToString(), out int total))
                        item.Total = total;
                }
                if (row.Cells["colFault"].Value != null)
                {
                    if (int.TryParse(row.Cells["colFault"].Value.ToString(), out int fault))
                        item.Fault = fault;
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
                bool result = _repository.SaveWaters(data);

                if (result)
                {
                    _currentData = _repository.LoadWaters(_subdivisionId);

                    foreach (DataGridViewRow row in dgvWaters.Rows)
                    {
                        if (row.Tag is Water oldItem && oldItem.Id == 0)
                        {
                            var newItem = _currentData.FirstOrDefault(w => w.Mname == oldItem.Mname);
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

        private void DgvWaters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvWaters.Rows[e.RowIndex];
            if (row.Tag is Water item)
            {
                bool changed = false;

                if (row.Cells["colTotal"].Value != null)
                {
                    if (int.TryParse(row.Cells["colTotal"].Value.ToString(), out int total))
                    {
                        if (item.Total != total)
                        {
                            item.Total = total;
                            changed = true;
                        }
                    }
                }
                if (row.Cells["colFault"].Value != null)
                {
                    if (int.TryParse(row.Cells["colFault"].Value.ToString(), out int fault))
                    {
                        if (item.Fault != fault)
                        {
                            item.Fault = fault;
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
                MessageBox.Show("Данные воды сохранены.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка сохранения воды.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDataChanged() => DataChanged?.Invoke(this, EventArgs.Empty);
        private void OnSaveRequested() => SaveRequested?.Invoke(this, EventArgs.Empty);
    }
}