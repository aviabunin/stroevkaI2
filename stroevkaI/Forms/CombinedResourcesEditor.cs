// stroevkaI/Forms/CombinedResourcesEditor.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;

namespace stroevkaI.Forms
{
    public partial class CombinedResourcesEditor : UserControl
    {
        private readonly int _subdivisionId;
        private readonly stroevkaContext _context;

        private readonly WatersRepository _watersRepository;
        private readonly PenasRepository _penasRepository;
        private readonly SizodsRepository _sizodsRepository;
        private readonly KostymsRepository _kostymsRepository;

        private List<Water> _watersData;
        private List<Pena> _penasData;
        private List<Sizod> _sizodsData;
        private List<Kostym> _kostymsData;

        private bool _isEditingEnabled = false;
        private const string ADMIN_PASSWORD = "111111";

        public event EventHandler DataChanged;
        public event EventHandler SaveRequested;

        public CombinedResourcesEditor()
        {
            InitializeComponent();
            _context = new stroevkaContext();
            _watersRepository = new WatersRepository(_context);
            _penasRepository = new PenasRepository(_context);
            _sizodsRepository = new SizodsRepository(_context);
            _kostymsRepository = new KostymsRepository(_context);
            SetupDataGridViews();
        }

        public CombinedResourcesEditor(int subdivisionId) : this()
        {
            _subdivisionId = subdivisionId;
            LoadData();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SetupDataGridViews()
        {
            var grids = new[] { dgvWaters, dgvPenas, dgvSizods, dgvKostyms };
            foreach (var grid in grids)
            {
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
                grid.ReadOnly = true;
                grid.RowHeadersVisible = false;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.MultiSelect = false;
                grid.BackgroundColor = Color.White;
                grid.BorderStyle = BorderStyle.Fixed3D;
                grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                grid.EditMode = DataGridViewEditMode.EditOnEnter;
                grid.RowTemplate.Height = 22;
                grid.ColumnHeadersHeight = 25;

                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);

                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            // Подписываемся на события CellEndEdit
            dgvWaters.CellEndEdit += DgvWaters_CellEndEdit;
            dgvPenas.CellEndEdit += DgvPenas_CellEndEdit;
            dgvSizods.CellEndEdit += DgvSizods_CellEndEdit;
            dgvKostyms.CellEndEdit += DgvKostyms_CellEndEdit;

            dgvWaters.CellEnter += Dgv_CellEnter;
            dgvPenas.CellEnter += Dgv_CellEnter;
            dgvSizods.CellEnter += Dgv_CellEnter;
            dgvKostyms.CellEnter += Dgv_CellEnter;
        }

        private void Dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (!_isEditingEnabled) return;

            var grid = sender as DataGridView;
            if (grid == null) return;

            var row = grid.Rows[e.RowIndex];
            if (row.IsNewRow) return;

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

        public void LoadData()
        {
            _watersData = _watersRepository.LoadWaters(_subdivisionId);
            _penasData = _penasRepository.LoadPenas(_subdivisionId);
            _sizodsData = _sizodsRepository.LoadSizods(_subdivisionId);
            _kostymsData = _kostymsRepository.LoadKostyms(_subdivisionId);

            RefreshGrids();
        }

        private void RefreshGrids()
        {
            RefreshWatersGrid();
            RefreshPenasGrid();
            RefreshSizodsGrid();
            RefreshKostymsGrid();
        }

        private void RefreshWatersGrid()
        {
            dgvWaters.Rows.Clear();
            if (_watersData == null) return;

            foreach (var item in _watersData.OrderBy(w => w.Norder))
            {
                int rowIndex = dgvWaters.Rows.Add();
                var row = dgvWaters.Rows[rowIndex];
                row.Cells[0].Value = item.Id;      // colWatersId
                row.Cells[1].Value = item.Mname;    // colWatersName
                row.Cells[2].Value = item.Total;    // colWatersTotal
                row.Cells[3].Value = item.Fault;    // colWatersFault
                row.Tag = item;
                row.ReadOnly = !_isEditingEnabled;
            }
            dgvWaters.ClearSelection();
        }

        private void RefreshPenasGrid()
        {
            dgvPenas.Rows.Clear();
            if (_penasData == null) return;

            foreach (var item in _penasData.OrderBy(p => p.Norder))
            {
                int rowIndex = dgvPenas.Rows.Add();
                var row = dgvPenas.Rows[rowIndex];
                row.Cells[0].Value = item.Id;       // colPenasId
                row.Cells[1].Value = item.Mname;     // colPenasName
                row.Cells[2].Value = item.Inwork;    // colPenasInwork
                row.Cells[3].Value = item.Inrezerv;  // colPenasInrezerv
                row.Tag = item;
                row.ReadOnly = !_isEditingEnabled;
            }
            dgvPenas.ClearSelection();
        }

        private void RefreshSizodsGrid()
        {
            dgvSizods.Rows.Clear();
            if (_sizodsData == null) return;

            foreach (var item in _sizodsData.OrderBy(s => s.Norder))
            {
                int rowIndex = dgvSizods.Rows.Add();
                var row = dgvSizods.Rows[rowIndex];
                row.Cells[0].Value = item.Id;          // colSizodsId
                row.Cells[1].Value = item.Mname;        // colSizodsName
                row.Cells[2].Value = item.Raschet;      // colSizodsRaschet
                row.Cells[3].Value = item.Rezerv;       // colSizodsRezerv
                row.Cells[4].Value = item.PostGdzs;     // colSizodsPostGdzs
                row.Cells[5].Value = item.BazaGdzs;     // colSizodsBazaGdzs
                row.Tag = item;
                row.ReadOnly = !_isEditingEnabled;
            }
            dgvSizods.ClearSelection();
        }

        private void RefreshKostymsGrid()
        {
            dgvKostyms.Rows.Clear();
            if (_kostymsData == null) return;

            foreach (var item in _kostymsData.OrderBy(k => k.Norder))
            {
                int rowIndex = dgvKostyms.Rows.Add();
                var row = dgvKostyms.Rows[rowIndex];
                row.Cells[0].Value = item.Id;      // colKostymsId
                row.Cells[1].Value = item.Mname;    // colKostymsName
                row.Cells[2].Value = item.N;        // colKostymsCount
                row.Tag = item;
                row.ReadOnly = !_isEditingEnabled;
            }
            dgvKostyms.ClearSelection();
        }

        private List<Water> GetWatersFromGrid()
        {
            var result = new List<Water>();
            foreach (DataGridViewRow row in dgvWaters.Rows)
            {
                if (row.IsNewRow || !(row.Tag is Water item)) continue;
                if (row.Cells[2].Value != null)  // colWatersTotal
                    int.TryParse(row.Cells[2].Value.ToString(), out int total);
                if (row.Cells[3].Value != null)  // colWatersFault
                    int.TryParse(row.Cells[3].Value.ToString(), out int fault);
                result.Add(item);
            }
            return result;
        }

        private List<Pena> GetPenasFromGrid()
        {
            var result = new List<Pena>();
            foreach (DataGridViewRow row in dgvPenas.Rows)
            {
                if (row.IsNewRow || !(row.Tag is Pena item)) continue;
                if (row.Cells[2].Value != null)  // colPenasInwork
                    int.TryParse(row.Cells[2].Value.ToString(), out int inwork);
                if (row.Cells[3].Value != null)  // colPenasInrezerv
                    int.TryParse(row.Cells[3].Value.ToString(), out int inrezerv);
                result.Add(item);
            }
            return result;
        }

        private List<Sizod> GetSizodsFromGrid()
        {
            var result = new List<Sizod>();
            foreach (DataGridViewRow row in dgvSizods.Rows)
            {
                if (row.IsNewRow || !(row.Tag is Sizod item)) continue;
                if (row.Cells[2].Value != null)  // colSizodsRaschet
                    int.TryParse(row.Cells[2].Value.ToString(), out int raschet);
                if (row.Cells[3].Value != null)  // colSizodsRezerv
                    int.TryParse(row.Cells[3].Value.ToString(), out int rezerv);
                if (row.Cells[4].Value != null)  // colSizodsPostGdzs
                    int.TryParse(row.Cells[4].Value.ToString(), out int postGdzs);
                if (row.Cells[5].Value != null)  // colSizodsBazaGdzs
                    int.TryParse(row.Cells[5].Value.ToString(), out int bazaGdzs);
                result.Add(item);
            }
            return result;
        }

        private List<Kostym> GetKostymsFromGrid()
        {
            var result = new List<Kostym>();
            foreach (DataGridViewRow row in dgvKostyms.Rows)
            {
                if (row.IsNewRow || !(row.Tag is Kostym item)) continue;
                if (row.Cells[2].Value != null)  // colKostymsCount
                    int.TryParse(row.Cells[2].Value.ToString(), out int count);
                result.Add(item);
            }
            return result;
        }

        private void DgvWaters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvWaters.Rows[e.RowIndex];
            if (row.Tag is Water item)
            {
                if (row.Cells[2].Value != null)
                    int.TryParse(row.Cells[2].Value.ToString(), out int total);
                if (row.Cells[3].Value != null)
                    int.TryParse(row.Cells[3].Value.ToString(), out int fault);
                OnDataChanged();
            }
        }

        private void DgvPenas_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvPenas.Rows[e.RowIndex];
            if (row.Tag is Pena item)
            {
                if (row.Cells[2].Value != null)
                    int.TryParse(row.Cells[2].Value.ToString(), out int inwork);
                if (row.Cells[3].Value != null)
                    int.TryParse(row.Cells[3].Value.ToString(), out int inrezerv);
                OnDataChanged();
            }
        }

        private void DgvSizods_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvSizods.Rows[e.RowIndex];
            if (row.Tag is Sizod item)
            {
                if (row.Cells[2].Value != null)
                    int.TryParse(row.Cells[2].Value.ToString(), out int raschet);
                if (row.Cells[3].Value != null)
                    int.TryParse(row.Cells[3].Value.ToString(), out int rezerv);
                if (row.Cells[4].Value != null)
                    int.TryParse(row.Cells[4].Value.ToString(), out int postGdzs);
                if (row.Cells[5].Value != null)
                    int.TryParse(row.Cells[5].Value.ToString(), out int bazaGdzs);
                OnDataChanged();
            }
        }

        private void DgvKostyms_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvKostyms.Rows[e.RowIndex];
            if (row.Tag is Kostym item)
            {
                if (row.Cells[2].Value != null)
                    int.TryParse(row.Cells[2].Value.ToString(), out int count);
                OnDataChanged();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var waters = GetWatersFromGrid();
            var penas = GetPenasFromGrid();
            var sizods = GetSizodsFromGrid();
            var kostyms = GetKostymsFromGrid();

            bool success = true;

            if (!_watersRepository.SaveWaters(waters))
                success = false;
            if (!_penasRepository.SavePenas(penas))
                success = false;
            if (!_sizodsRepository.SaveSizods(sizods))
                success = false;
            if (!_kostymsRepository.SaveKostyms(kostyms))
                success = false;

            if (success)
            {
                MessageBox.Show("Все данные сохранены.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                OnSaveRequested();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении некоторых данных.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show("Неверный пароль.", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                _isEditingEnabled = true;
            }
            else
            {
                _isEditingEnabled = false;
                LoadData();
            }

            foreach (DataGridViewRow row in dgvWaters.Rows)
                row.ReadOnly = !_isEditingEnabled;
            foreach (DataGridViewRow row in dgvPenas.Rows)
                row.ReadOnly = !_isEditingEnabled;
            foreach (DataGridViewRow row in dgvSizods.Rows)
                row.ReadOnly = !_isEditingEnabled;
            foreach (DataGridViewRow row in dgvKostyms.Rows)
                row.ReadOnly = !_isEditingEnabled;

            btnSave.Enabled = _isEditingEnabled;
        }

        private void OnDataChanged()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveRequested()
        {
            SaveRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}