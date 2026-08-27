// stroevkaI/Forms/EquipmentEditor.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;

namespace stroevkaI.Forms
{
    public partial class EquipmentEditor : UserControl
    {
        private readonly EquipmentRepository _repository;
        private readonly int _subdivisionId;
        private readonly string _subdivisionName;

        // Данные для каждой таблицы
        private List<Pena> _penas;
        private List<Sizod> _sizods;
        private List<Water> _waters;
        private List<Kostym> _kostyms;

        private bool _isEditingEnabled = false;
        private const string ADMIN_PASSWORD = "111111";

        // События для связи с формой
        public event EventHandler DataChanged;
        public event EventHandler SaveRequested;

        // Конструктор для дизайнера
        public EquipmentEditor()
        {
            InitializeComponent();
            SetupDataGridViews();
            _repository = new EquipmentRepository(new stroevkaContext());
        }

        // Основной конструктор
        public EquipmentEditor(int subdivisionId) : this()
        {
            _subdivisionId = subdivisionId;

            var pch = FireEquipsPivotRepository.getPchById(_subdivisionId);
            _subdivisionName = pch?.Пч ?? $"ПЧ {_subdivisionId}";

            LoadAllData();
        }



        private void SetupDataGridViews()
        {
            // Настройка всех DataGridView
            SetupGridView(dgvPena, new[] { "Наименование", "В работе", "В резерве" });
            SetupGridView(dgvSizod, new[] { "Наименование", "Расчёт", "Резерв", "Пост ГДЗС", "База ГДЗС" });
            SetupGridView(dgvWaters, new[] { "Наименование", "Всего", "Неисправно" });
            SetupGridView(dgvKostym, new[] { "Наименование", "Количество" });

            // Подписываемся на события
            dgvPena.CellEnter += (s, e) => OnCellEnter(s, e, dgvPena);
            dgvSizod.CellEnter += (s, e) => OnCellEnter(s, e, dgvSizod);
            dgvWaters.CellEnter += (s, e) => OnCellEnter(s, e, dgvWaters);
            dgvKostym.CellEnter += (s, e) => OnCellEnter(s, e, dgvKostym);

            dgvPena.CellEndEdit += (s, e) => OnCellEndEdit(s, e, dgvPena);
            dgvSizod.CellEndEdit += (s, e) => OnCellEndEdit(s, e, dgvSizod);
            dgvWaters.CellEndEdit += (s, e) => OnCellEndEdit(s, e, dgvWaters);
            dgvKostym.CellEndEdit += (s, e) => OnCellEndEdit(s, e, dgvKostym);
        }

        private void SetupGridView(DataGridView dgv, string[] columnNames)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = System.Drawing.Color.White;
            dgv.BorderStyle = BorderStyle.Fixed3D;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv.ReadOnly = true;

            dgv.Columns.Clear();
            foreach (var name in columnNames)
            {
                var column = new DataGridViewTextBoxColumn();
                column.Name = name;
                column.HeaderText = name;
                column.ReadOnly = true;
                dgv.Columns.Add(column);
            }

            // Настройка ширины колонок
            if (dgv.Columns.Count > 0)
            {
                dgv.Columns[0].Width = 180;
                dgv.Columns[0].DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            }

            for (int i = 1; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].Width = 80;
                dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[i].DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            }

            dgv.RowTemplate.Height = 25;
        }

        private void OnCellEnter(object sender, DataGridViewCellEventArgs e, DataGridView dgv)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 1) return;
            if (_isEditingEnabled)
            {
                var row = dgv.Rows[e.RowIndex];
                if (!row.ReadOnly)
                {
                    dgv.BeginEdit(true);
                    var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (cell.IsInEditMode)
                    {
                        var textBox = dgv.EditingControl as TextBox;
                        textBox?.SelectAll();
                    }
                }
            }
        }

        private void OnCellEndEdit(object sender, DataGridViewCellEventArgs e, DataGridView dgv)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 1) return;
            var row = dgv.Rows[e.RowIndex];
            if (row.Tag == null) return;

            // Обновляем данные в зависимости от типа
            if (dgv == dgvPena && row.Tag is Pena pena)
            {
                UpdatePenaFromRow(row, pena);
            }
            else if (dgv == dgvSizod && row.Tag is Sizod sizod)
            {
                UpdateSizodFromRow(row, sizod);
            }
            else if (dgv == dgvWaters && row.Tag is Water water)
            {
                UpdateWaterFromRow(row, water);
            }
            else if (dgv == dgvKostym && row.Tag is Kostym kostym)
            {
                UpdateKostymFromRow(row, kostym);
            }

            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdatePenaFromRow(DataGridViewRow row, Pena item)
        {
            int val1 = 0, val2 = 0;
            if (row.Cells[1].Value != null)
                int.TryParse(row.Cells[1].Value.ToString(), out val1);
            if (row.Cells[2].Value != null)
                int.TryParse(row.Cells[2].Value.ToString(), out val2);
            item.Inwork = val1;
            item.Inrezerv = val2;
        }

        private void UpdateSizodFromRow(DataGridViewRow row, Sizod item)
        {
            int val1 = 0, val2 = 0, val3 = 0, val4 = 0;
            if (row.Cells[1].Value != null)
                int.TryParse(row.Cells[1].Value.ToString(), out val1);
            if (row.Cells[2].Value != null)
                int.TryParse(row.Cells[2].Value.ToString(), out val2);
            if (row.Cells[3].Value != null)
                int.TryParse(row.Cells[3].Value.ToString(), out val3);
            if (row.Cells[4].Value != null)
                int.TryParse(row.Cells[4].Value.ToString(), out val4);
            item.Raschet = val1;
            item.Rezerv = val2;
            item.PostGdzs = val3;
            item.BazaGdzs = val4;
        }

        private void UpdateWaterFromRow(DataGridViewRow row, Water item)
        {
            int val1 = 0, val2 = 0;
            if (row.Cells[1].Value != null)
                int.TryParse(row.Cells[1].Value.ToString(), out val1);
            if (row.Cells[2].Value != null)
                int.TryParse(row.Cells[2].Value.ToString(), out val2);
            item.Total = val1;
            item.Fault = val2;
        }

        private void UpdateKostymFromRow(DataGridViewRow row, Kostym item)
        {
            int val1 = 0;
            if (row.Cells[1].Value != null)
                int.TryParse(row.Cells[1].Value.ToString(), out val1);
            item.N = val1;
        }

        public void LoadAllData()
        {
            _penas = _repository.LoadPenas(_subdivisionId);
            _sizods = _repository.LoadSizods(_subdivisionId);
            _waters = _repository.LoadWaters(_subdivisionId);
            _kostyms = _repository.LoadKostyms(_subdivisionId);

            RefreshGrids();
            UpdateTitle();
        }

        private void RefreshGrids()
        {
            RefreshPenaGrid();
            RefreshSizodGrid();
            RefreshWatersGrid();
            RefreshKostymGrid();
        }

        private void RefreshPenaGrid()
        {
            dgvPena.Rows.Clear();
            foreach (var item in _penas.OrderBy(p => p.Norder))
            {
                int rowIndex = dgvPena.Rows.Add();
                var row = dgvPena.Rows[rowIndex];
                row.Cells[0].Value = item.Mname;
                row.Cells[1].Value = item.Inwork;
                row.Cells[2].Value = item.Inrezerv;
                row.Tag = item;
                row.ReadOnly = !_isEditingEnabled;
            }
        }

        private void RefreshSizodGrid()
        {
            dgvSizod.Rows.Clear();
            foreach (var item in _sizods.OrderBy(s => s.Norder))
            {
                int rowIndex = dgvSizod.Rows.Add();
                var row = dgvSizod.Rows[rowIndex];
                row.Cells[0].Value = item.Mname;
                row.Cells[1].Value = item.Raschet;
                row.Cells[2].Value = item.Rezerv;
                row.Cells[3].Value = item.PostGdzs;
                row.Cells[4].Value = item.BazaGdzs;
                row.Tag = item;
                row.ReadOnly = !_isEditingEnabled;
            }
        }

        private void RefreshWatersGrid()
        {
            dgvWaters.Rows.Clear();
            foreach (var item in _waters.OrderBy(w => w.Norder))
            {
                int rowIndex = dgvWaters.Rows.Add();
                var row = dgvWaters.Rows[rowIndex];
                row.Cells[0].Value = item.Mname;
                row.Cells[1].Value = item.Total;
                row.Cells[2].Value = item.Fault;
                row.Tag = item;
                row.ReadOnly = !_isEditingEnabled;
            }
        }

        private void RefreshKostymGrid()
        {
            dgvKostym.Rows.Clear();
            foreach (var item in _kostyms.OrderBy(k => k.Norder))
            {
                int rowIndex = dgvKostym.Rows.Add();
                var row = dgvKostym.Rows[rowIndex];
                row.Cells[0].Value = item.Mname;
                row.Cells[1].Value = item.N;
                row.Tag = item;
                row.ReadOnly = !_isEditingEnabled;
            }
        }

        private void UpdateTitle()
        {
            int total = _penas.Count + _sizods.Count + _waters.Count + _kostyms.Count;
            lblTitle.Text = $"Оборудование - {total} записей (Пена: {_penas.Count}, СИЗОД: {_sizods.Count}, Вода: {_waters.Count}, Костюмы: {_kostyms.Count})";
        }

        private List<T> GetDataFromGrid<T>(DataGridView dgv) where T : class
        {
            var result = new List<T>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Tag is T item)
                    result.Add(item);
            }
            return result;
        }

        private void OnSaveRequested()
        {
            SaveRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var penas = GetDataFromGrid<Pena>(dgvPena);
            var sizods = GetDataFromGrid<Sizod>(dgvSizod);
            var waters = GetDataFromGrid<Water>(dgvWaters);
            var kostyms = GetDataFromGrid<Kostym>(dgvKostym);

            bool success = true;
            success &= _repository.SavePenas(penas);
            success &= _repository.SaveSizods(sizods);
            success &= _repository.SaveWaters(waters);
            success &= _repository.SaveKostyms(kostyms);

            if (success)
            {
                MessageBox.Show("Все данные сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAllData();
                OnSaveRequested();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChkEditMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEditMode.Checked)
            {
                using (var passwordForm = new PasswordInputForm())
                {
                    if (passwordForm.ShowDialog() != DialogResult.OK)
                    {
                        chkEditMode.Checked = false;
                        return;
                    }
                    if (passwordForm.Password != ADMIN_PASSWORD)
                    {
                        chkEditMode.Checked = false;
                        MessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                _isEditingEnabled = true;
            }
            else
            {
                _isEditingEnabled = false;
                LoadAllData();
            }

            btnSave.Enabled = _isEditingEnabled;
            RefreshGrids();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTitle();
        }
    }
}