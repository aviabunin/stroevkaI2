using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using stroevkaI.Models;

namespace stroevkaI.Forms
{
    public partial class SredstvaEditor : UserControl
    {
        #region Поля
        private List<Sredstva> data;
        private List<Sredstva> displayData;
        private bool isEdit = false;
        private int subdivisionId;
        private FirePsgStat currentPch;

        private Sredstva selectedItem;
        private string selectedState;
        private bool isMenuShowing = false;
        private ContextMenuStrip transferContextMenu;

        private int selectedRowIndex = -1;
        #endregion

        public event EventHandler DataChanged;

        public SredstvaEditor()
        {
            InitializeComponent();
            SubscribeEvents();
            SetupGrid();
            CreateContextMenu();
        }

        public SredstvaEditor(FirePsgStat _currentPch) : this()
        {
            InitSredstvaEditor(_currentPch);
        }

        public SredstvaEditor(int _subdivisionId) : this()
        {
            subdivisionId = _subdivisionId;
            LoadSredstvaById(subdivisionId);
        }

        private void SubscribeEvents()
        {
            btnAdd.Click += BtnAdd_Click;
            btnDelete.Click += BtnDelete_Click;
            btnEdit.Click += BtnEdit_Click;

            dgvSredstva.DataBindingComplete += DgvSredstva_DataBindingComplete;
            dgvSredstva.CellClick += DgvSredstva_CellClick;
            dgvSredstva.CellFormatting += DgvSredstva_CellFormatting;
            dgvSredstva.SelectionChanged += DgvSredstva_SelectionChanged;
        }

        private void SetupGrid()
        {
            dgvSredstva.ReadOnly = true;
            dgvSredstva.AllowUserToAddRows = false;
            dgvSredstva.AllowUserToDeleteRows = false;
            dgvSredstva.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSredstva.MultiSelect = false;
            dgvSredstva.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvSredstva.RowHeadersVisible = false;
            dgvSredstva.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dgvSredstva.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F);
        }

        private void CreateContextMenu()
        {
            transferContextMenu = new ContextMenuStrip();
            transferContextMenu.Closed += (s, e) =>
            {
                isMenuShowing = false;
            };
        }

        private void DgvSredstva_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSredstva.SelectedRows.Count > 0)
            {
                selectedRowIndex = dgvSredstva.SelectedRows[0].Index;
            }
        }

        private void DgvSredstva_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvSredstva.Rows.Count) return;

            var row = dgvSredstva.Rows[e.RowIndex];
            if (row == null || row.DataBoundItem == null) return;

            if (row.DataBoundItem is Sredstva s && s.Norder == -1)
            {
                if (e.ColumnIndex == 0)
                {
                    e.Value = s.NameSredstvo;
                    e.CellStyle.BackColor = Color.LightSteelBlue;
                    e.CellStyle.ForeColor = Color.DarkBlue;
                    e.CellStyle.Font = new Font(dgvSredstva.Font, FontStyle.Bold);
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = "";
                    e.CellStyle.BackColor = Color.LightSteelBlue;
                    e.CellStyle.ForeColor = Color.LightSteelBlue;
                    e.FormattingApplied = true;
                }
            }
        }

        private void DgvSredstva_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isEdit) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (isMenuShowing) return;

            var row = dgvSredstva.Rows[e.RowIndex];
            if (row.DataBoundItem is not Sredstva s || s.Norder == -1) return;

            selectedItem = s;

            string colName = dgvSredstva.Columns[e.ColumnIndex].Name;

            if (colName != "brColumn" && colName != "rezervColumn" &&
                colName != "remontColumn" && colName != "to1Column" &&
                colName != "to2Column")
            {
                return;
            }

            selectedState = colName switch
            {
                "brColumn" => "br",
                "rezervColumn" => "rezerv",
                "remontColumn" => "remont",
                "to1Column" => "tofirst",
                "to2Column" => "totow",
                _ => null
            };

            if (!string.IsNullOrEmpty(selectedState))
            {
                int value = GetStateValue(s, selectedState);
                if (value > 0)
                {
                    dgvSredstva.ClearSelection();
                    dgvSredstva.Rows[e.RowIndex].Selected = true;
                    ShowTransferMenuAtCursor(s, selectedState);
                }
            }
        }

        private void ShowTransferMenuAtCursor(Sredstva item, string fromState)
        {
            if (item == null || string.IsNullOrEmpty(fromState)) return;
            if (isMenuShowing) return;

            isMenuShowing = true;
            selectedItem = item;
            transferContextMenu.Items.Clear();

            var operations = TransferOperations.GetOperationsForState(fromState);
            if (operations.Count == 0)
            {
                var emptyItem = new ToolStripMenuItem($"Нет доступных переходов из {TransferOperations.GetStateDisplayName(fromState)}");
                emptyItem.Enabled = false;
                emptyItem.ForeColor = Color.Gray;
                transferContextMenu.Items.Add(emptyItem);
                transferContextMenu.Show(Cursor.Position);
                return;
            }

            var titleItem = new ToolStripMenuItem($"Перевод: {item.NameSredstvo}");
            titleItem.Enabled = false;
            titleItem.Font = new Font(titleItem.Font, FontStyle.Bold);
            transferContextMenu.Items.Add(titleItem);

            var fromName = TransferOperations.GetStateDisplayName(fromState);
            var fromColor = TransferOperations.GetStateColor(fromState);
            var fromCount = GetStateValue(item, fromState);

            var fromItem = new ToolStripMenuItem($"Из: {fromName} ({fromCount})");
            fromItem.Enabled = false;
            fromItem.ForeColor = fromColor;
            transferContextMenu.Items.Add(fromItem);
            transferContextMenu.Items.Add(new ToolStripSeparator());

            foreach (var op in operations)
            {
                var toStateName = TransferOperations.GetStateDisplayName(op.ToState);
                var toColor = TransferOperations.GetStateColor(op.ToState);

                var opItem = new ToolStripMenuItem($"{op.Icon} {toStateName}");
                opItem.Tag = op;
                opItem.ForeColor = toColor;
                opItem.Click += (s, e) =>
                {
                    var operation = (s as ToolStripMenuItem)?.Tag as TransferOperation;
                    if (operation != null)
                    {
                        ExecuteTransfer(selectedItem, operation);
                    }
                };
                transferContextMenu.Items.Add(opItem);
            }

            transferContextMenu.Show(Cursor.Position);
        }

        private void ExecuteTransfer(Sredstva item, TransferOperation operation)
        {
            if (item == null || operation == null) return;

            int fromValue = GetStateValue(item, operation.FromState);
            if (fromValue <= 0)
            {
                MessageBox.Show($"Нет техники в состоянии '{TransferOperations.GetStateDisplayName(operation.FromState)}'",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isMenuShowing = false;
                return;
            }

            string fromName = TransferOperations.GetStateDisplayName(operation.FromState);
            string toName = TransferOperations.GetStateDisplayName(operation.ToState);
            string itemName = item.NameSredstvo;

            var result = MessageBox.Show(
                $"Перевести '{itemName}'\nиз '{fromName}' в '{toName}'?",
                "Подтверждение перевода",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                isMenuShowing = false;
                return;
            }

            SetStateValue(item, operation.FromState, fromValue - 1);
            int toValue = GetStateValue(item, operation.ToState);
            SetStateValue(item, operation.ToState, toValue + 1);

            if (FireEquipsPivotRepository.SaveSredstva(item))
            {
                RefreshData();
                DataChanged?.Invoke(this, EventArgs.Empty);

                selectedState = null;
                selectedItem = null;
                isMenuShowing = false;
                transferContextMenu.Close();

                MessageBox.Show($"Перевод выполнен успешно!",
                    "Успешно",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении в БД",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                isMenuShowing = false;
            }
        }

        private int GetStateValue(Sredstva item, string state)
        {
            return state switch
            {
                "br" => (int)item.Br,
                "rezerv" => (int)item.Rezerv,
                "remont" => (int)item.Remont,
                "tofirst" => (int)item.Tofirst,
                "totow" => (int)item.Totow,
                _ => 0
            };
        }

        private void SetStateValue(Sredstva item, string state, int value)
        {
            switch (state)
            {
                case "br": item.Br = value; break;
                case "rezerv": item.Rezerv = value; break;
                case "remont": item.Remont = value; break;
                case "tofirst": item.Tofirst = value; break;
                case "totow": item.Totow = value; break;
            }
        }

        private List<Sredstva> BuildDisplayList(List<Sredstva> source)
        {
            if (source == null || source.Count == 0)
                return new List<Sredstva>();

            var vids = source.Select(s => s.SredstvoVid).Distinct().OrderBy(v => v).ToList();
            var result = source.ToList();

            foreach (var vid in vids)
            {
                result.Add(new Sredstva
                {
                    NameSredstvo = vid,
                    SredstvoVid = vid,
                    Norder = -1,
                    Id = -1,
                    Br = 0,
                    Rezerv = 0,
                    Remont = 0,
                    Tofirst = 0,
                    Totow = 0
                });
            }

            return result
                .OrderBy(s => s.SredstvoVid)
                .ThenBy(s => s.Norder == -1 ? 0 : 1)
                .ThenBy(s => s.Norder)
                .ToList();
        }

        public void InitSredstvaEditor(FirePsgStat _currentPch)
        {
            currentPch = _currentPch;
            if (currentPch != null)
            {
                subdivisionId = (int)currentPch.PchId;
                LoadSredstva();
            }
        }

        public void LoadSredstvaById(int _subdivisionId)
        {
            subdivisionId = _subdivisionId;
            currentPch = FireEquipsPivotRepository.getPchById(subdivisionId);
            LoadSredstva();
        }

        private void LoadSredstva()
        {
            if (subdivisionId == 0)
            {
                if (currentPch != null)
                {
                    subdivisionId = (int)currentPch.PchId;
                }
                else
                {
                    return;
                }
            }

            var dataFromDb = FireEquipsPivotRepository.LoadSredstva(subdivisionId);
            displayData = BuildDisplayList(dataFromDb);

            // Просто обновляем данные, колонки не трогаем
            dgvSredstva.DataSource = null;
            dgvSredstva.DataSource = displayData;

            UpdateCount();
            dgvSredstva.Refresh();

            // Восстанавливаем выделение
            RestoreSelectedRow();
        }

        private void SelectFirstNonGroupRow()
        {
            if (dgvSredstva.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvSredstva.Rows)
                {
                    if (row.DataBoundItem is Sredstva s && s.Norder != -1)
                    {
                        dgvSredstva.ClearSelection();
                        row.Selected = true;
                        selectedRowIndex = row.Index;
                        return;
                    }
                }
            }
        }

        private void RestoreSelectedRow()
        {
            if (selectedRowIndex >= 0 && selectedRowIndex < dgvSredstva.Rows.Count)
            {
                var row = dgvSredstva.Rows[selectedRowIndex];
                if (row.DataBoundItem is Sredstva s && s.Norder != -1)
                {
                    dgvSredstva.ClearSelection();
                    row.Selected = true;
                    return;
                }
            }
            SelectFirstNonGroupRow();
        }

        private void UpdateCount()
        {
            int cnt = displayData?.Count(d => d.Norder != -1) ?? 0;
            lblCount.Text = $"Записей: {cnt}";
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            ToggleEdit();
        }

        private void ToggleEdit()
        {
            isEdit = !isEdit;
            btnEdit.Text = isEdit ? "Выйти" : "Редактировать";
            btnAdd.Enabled = isEdit;
            btnDelete.Enabled = isEdit;

            if (isEdit)
            {
                RestoreSelectedRow();
            }
            else
            {
                dgvSredstva.ClearSelection();
                isMenuShowing = false;
                transferContextMenu.Close();
                selectedState = null;
                selectedItem = null;
                SelectFirstNonGroupRow();
            }

            dgvSredstva.Refresh();
        }

        private void AddRow()
        {
            int maxNorder = 0;
            if (displayData != null && displayData.Count > 0)
            {
                var maxItem = displayData.Where(x => x.Norder != -1);
                if (maxItem.Any())
                {
                    maxNorder = maxItem.Max(x => x.Norder);
                }
            }

            var item = new Sredstva
            {
                SubdivisionId = subdivisionId,
                NameSredstvo = "Новое",
                SredstvoVid = "",
                Br = 0,
                Rezerv = 0,
                Remont = 0,
                Tofirst = 0,
                Totow = 0,
                Norder = maxNorder + 1
            };

            if (FireEquipsPivotRepository.AddSredstva(item))
            {
                int currentIndex = dgvSredstva.CurrentRow?.Index ?? -1;
                LoadSredstva();
                DataChanged?.Invoke(this, EventArgs.Empty);

                if (currentIndex >= 0 && currentIndex < dgvSredstva.Rows.Count)
                {
                    dgvSredstva.ClearSelection();
                    dgvSredstva.Rows[currentIndex].Selected = true;
                }
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении записи", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteRow()
        {
            if (dgvSredstva.CurrentRow?.DataBoundItem is not Sredstva s || s.Norder == -1) return;

            int currentIndex = dgvSredstva.CurrentRow.Index;

            var confirmResult = MessageBox.Show(
                $"Вы действительно хотите удалить средство '{s.NameSredstvo}'?\n\nЭто действие нельзя отменить!",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmResult != DialogResult.Yes) return;

            using (var passwordForm = new PasswordInputForm())
            {
                var dialogResult = passwordForm.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    if (passwordForm.Password == "1111")
                    {
                        if (FireEquipsPivotRepository.DeleteSredstva(s.Id))
                        {
                            LoadSredstva();
                            DataChanged?.Invoke(this, EventArgs.Empty);

                            if (currentIndex < dgvSredstva.Rows.Count)
                            {
                                dgvSredstva.ClearSelection();
                                dgvSredstva.Rows[currentIndex].Selected = true;
                            }
                            else if (dgvSredstva.Rows.Count > 0)
                            {
                                int newIndex = Math.Min(currentIndex - 1, dgvSredstva.Rows.Count - 1);
                                if (newIndex >= 0)
                                {
                                    dgvSredstva.ClearSelection();
                                    dgvSredstva.Rows[newIndex].Selected = true;
                                }
                                else
                                {
                                    SelectFirstNonGroupRow();
                                }
                            }

                            MessageBox.Show($"Средство '{s.NameSredstvo}' успешно удалено",
                                "Успешно",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при удалении записи. Попробуйте еще раз.",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный пароль! Удаление отменено.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e) => AddRow();
        private void BtnDelete_Click(object sender, EventArgs e) => DeleteRow();

        private int ToInt(object v)
        {
            if (v == null) return 0;
            return int.TryParse(v.ToString(), out int i) ? i : 0;
        }

        private void DgvSredstva_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ApplyRowStyles();
            dgvSredstva.Refresh();
        }

        private void ApplyRowStyles()
        {
            foreach (DataGridViewRow row in dgvSredstva.Rows)
            {
                if (row.DataBoundItem is Sredstva s)
                {
                    if (s.Norder == -1)
                    {
                        row.DefaultCellStyle.Font = new Font(dgvSredstva.Font, FontStyle.Bold);
                        row.DefaultCellStyle.ForeColor = Color.DarkBlue;
                        row.DefaultCellStyle.BackColor = Color.LightSteelBlue;
                        row.ReadOnly = true;
                    }
                    else
                    {
                        row.DefaultCellStyle.Font = dgvSredstva.Font;
                        row.DefaultCellStyle.BackColor = (s.Br > 0 || s.Rezerv > 0) ?
                            Color.LightYellow : Color.White;
                        row.ReadOnly = true;
                    }
                }
            }
        }

        public void RefreshData() => LoadSredstva();
        public List<Sredstva> GetData() => displayData?.Where(d => d.Norder != -1).ToList();
    }
}