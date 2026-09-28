// stroevkaI/Forms/CombinedResourcesEditor.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;
using stroevkaI.Services;

namespace stroevkaI.Forms
{
    public partial class CombinedResourcesEditor : UserControl,IDataEditor
    {
        private readonly int _subdivisionId;

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
            SetupDataGridViews();
        }

        public CombinedResourcesEditor(int subdivisionId) : this()
        {
            _subdivisionId = subdivisionId;
            LoadData();
        }

        // --------------------------------------------------------------
        // Стилизация 4 гридов — как в WatersEditor / PenasEditor / ...
        // --------------------------------------------------------------
        private void SetupDataGridViews()
        {
            // ===== dgvWaters =====
            dgvWaters.Columns.Clear();
            dgvWaters.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colWatersId",
                HeaderText = "Id",
                Visible = false,
                ReadOnly = true
            });
            dgvWaters.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colWatersName",
                HeaderText = "Источник",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });
            dgvWaters.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colWatersTotal",
                HeaderText = "Всего",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvWaters.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colWatersFault",
                HeaderText = "Неиспр.",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // ===== dgvPenas =====
            dgvPenas.Columns.Clear();
            dgvPenas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPenasId",
                HeaderText = "Id",
                Visible = false,
                ReadOnly = true
            });
            dgvPenas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPenasName",
                HeaderText = "Пенообразователь",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });
            dgvPenas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPenasInwork",
                HeaderText = "В работе",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvPenas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPenasInrezerv",
                HeaderText = "В резерве",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // ===== dgvSizods =====
            dgvSizods.Columns.Clear();
            dgvSizods.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSizodsId",
                HeaderText = "Id",
                Visible = false,
                ReadOnly = true
            });
            dgvSizods.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSizodsName",
                HeaderText = "Средство",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });
            dgvSizods.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSizodsRaschet",
                HeaderText = "Расчёт",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvSizods.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSizodsRezerv",
                HeaderText = "Резерв",
                Width = 60,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvSizods.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSizodsPostGdzs",
                HeaderText = "Пост ГДЗС",
                Width = 70,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvSizods.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSizodsBazaGdzs",
                HeaderText = "База ГДЗС",
                Width = 70,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // ===== dgvKostyms =====
            dgvKostyms.Columns.Clear();
            dgvKostyms.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colKostymsId",
                HeaderText = "Id",
                Visible = false,
                ReadOnly = true
            });
            dgvKostyms.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colKostymsName",
                HeaderText = "Марка",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });
            dgvKostyms.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colKostymsCount",
                HeaderText = "Кол-во",
                Width = 70,
                ReadOnly = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // ===== Общий стиль для всех 4 =====
            var grids = new[] { dgvWaters, dgvPenas, dgvSizods, dgvKostyms };
            foreach (var grid in grids)
            {
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
                grid.ReadOnly = true;              // при включённом режиме снимем ниже
                grid.RowHeadersVisible = false;
                grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
                grid.MultiSelect = false;
                grid.BorderStyle = BorderStyle.Fixed3D;
                grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                grid.EditMode = DataGridViewEditMode.EditOnEnter;
                grid.RowTemplate.Height = 22;
                grid.ColumnHeadersHeight = 25;
                grid.StandardTab = false;

                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                grid.ColumnHeadersDefaultCellStyle.Font =
                    new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
                grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
                grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

                grid.BackgroundColor = Color.White;

                grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 216, 230);
                grid.DefaultCellStyle.SelectionForeColor = Color.Black;

                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

                grid.RowHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
                grid.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            }

            // ===== Подписки =====
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
            if (grid.Rows[e.RowIndex].IsNewRow) return;

            grid.BeginEdit(true);
            var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.IsInEditMode)
            {
                if (grid.EditingControl is TextBox tb) tb.SelectAll();
            }
        }

        // --------------------------------------------------------------
        // Загрузка данных — только из кэша
        // --------------------------------------------------------------
        public void LoadData()
        {
            _watersData = AppDataCache.Instance.GetWaters(_subdivisionId).ToList();
            _penasData = AppDataCache.Instance.GetPenas(_subdivisionId).ToList();
            _sizodsData = AppDataCache.Instance.GetSizods(_subdivisionId).ToList();
            _kostymsData = AppDataCache.Instance.GetKostyms(_subdivisionId).ToList();

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
                int i = dgvWaters.Rows.Add();
                var row = dgvWaters.Rows[i];
                row.Cells["colWatersId"].Value = item.Id;
                row.Cells["colWatersName"].Value = item.Mname;
                row.Cells["colWatersTotal"].Value = item.Total;
                row.Cells["colWatersFault"].Value = item.Fault;
                row.Tag = item;
            }
            dgvWaters.ClearSelection();
        }

        private void RefreshPenasGrid()
        {
            dgvPenas.Rows.Clear();
            if (_penasData == null) return;

            foreach (var item in _penasData.OrderBy(p => p.Norder))
            {
                int i = dgvPenas.Rows.Add();
                var row = dgvPenas.Rows[i];
                row.Cells["colPenasId"].Value = item.Id;
                row.Cells["colPenasName"].Value = item.Mname;
                row.Cells["colPenasInwork"].Value = item.Inwork;
                row.Cells["colPenasInrezerv"].Value = item.Inrezerv;
                row.Tag = item;
            }
            dgvPenas.ClearSelection();
        }

        private void RefreshSizodsGrid()
        {
            dgvSizods.Rows.Clear();
            if (_sizodsData == null) return;

            foreach (var item in _sizodsData.OrderBy(s => s.Norder))
            {
                int i = dgvSizods.Rows.Add();
                var row = dgvSizods.Rows[i];
                row.Cells["colSizodsId"].Value = item.Id;
                row.Cells["colSizodsName"].Value = item.Mname;
                row.Cells["colSizodsRaschet"].Value = item.Raschet;
                row.Cells["colSizodsRezerv"].Value = item.Rezerv;
                row.Cells["colSizodsPostGdzs"].Value = item.PostGdzs;
                row.Cells["colSizodsBazaGdzs"].Value = item.BazaGdzs;
                row.Tag = item;
            }
            dgvSizods.ClearSelection();
        }

        private void RefreshKostymsGrid()
        {
            dgvKostyms.Rows.Clear();
            if (_kostymsData == null) return;

            foreach (var item in _kostymsData.OrderBy(k => k.Norder))
            {
                int i = dgvKostyms.Rows.Add();
                var row = dgvKostyms.Rows[i];
                row.Cells["colKostymsId"].Value = item.Id;
                row.Cells["colKostymsName"].Value = item.Mname;
                row.Cells["colKostymsCount"].Value = item.N;
                row.Tag = item;
            }
            dgvKostyms.ClearSelection();
        }

        // --------------------------------------------------------------
        // Сбор значений из гридов
        // --------------------------------------------------------------
        private List<Water> GetWatersFromGrid()
        {
            var result = new List<Water>();
            foreach (DataGridViewRow row in dgvWaters.Rows)
            {
                if (row.IsNewRow || row.Tag is not Water item) continue;
                item.Total = ParseInt(row.Cells["colWatersTotal"].Value);
                item.Fault = ParseInt(row.Cells["colWatersFault"].Value);
                result.Add(item);
            }
            return result;
        }

        private List<Pena> GetPenasFromGrid()
        {
            var result = new List<Pena>();
            foreach (DataGridViewRow row in dgvPenas.Rows)
            {
                if (row.IsNewRow || row.Tag is not Pena item) continue;
                item.Inwork = ParseInt(row.Cells["colPenasInwork"].Value);
                item.Inrezerv = ParseInt(row.Cells["colPenasInrezerv"].Value);
                result.Add(item);
            }
            return result;
        }

        private List<Sizod> GetSizodsFromGrid()
        {
            var result = new List<Sizod>();
            foreach (DataGridViewRow row in dgvSizods.Rows)
            {
                if (row.IsNewRow || row.Tag is not Sizod item) continue;
                item.Raschet = ParseInt(row.Cells["colSizodsRaschet"].Value);
                item.Rezerv = ParseInt(row.Cells["colSizodsRezerv"].Value);
                item.PostGdzs = ParseInt(row.Cells["colSizodsPostGdzs"].Value);
                item.BazaGdzs = ParseInt(row.Cells["colSizodsBazaGdzs"].Value);
                result.Add(item);
            }
            return result;
        }

        private List<Kostym> GetKostymsFromGrid()
        {
            var result = new List<Kostym>();
            foreach (DataGridViewRow row in dgvKostyms.Rows)
            {
                if (row.IsNewRow || row.Tag is not Kostym item) continue;
                item.N = ParseInt(row.Cells["colKostymsCount"].Value);
                result.Add(item);
            }
            return result;
        }

        private static int ParseInt(object v)
            => v == null ? 0 : (int.TryParse(v.ToString(), out int i) ? i : 0);

        // --------------------------------------------------------------
        // Редактирование
        // --------------------------------------------------------------
        private void DgvWaters_CellEndEdit(object sender, DataGridViewCellEventArgs e) => OnDataChanged();
        private void DgvPenas_CellEndEdit(object sender, DataGridViewCellEventArgs e) => OnDataChanged();
        private void DgvSizods_CellEndEdit(object sender, DataGridViewCellEventArgs e) => OnDataChanged();
        private void DgvKostyms_CellEndEdit(object sender, DataGridViewCellEventArgs e) => OnDataChanged();

        private void ChkEditMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEditMode.Checked)
            {
                using var passwordForm = new PasswordInputForm();
                if (passwordForm.ShowDialog() != DialogResult.OK) { chkEditMode.Checked = false; return; }
                if (passwordForm.Password != ADMIN_PASSWORD)
                {
                    chkEditMode.Checked = false;
                    MessageBox.Show("Неверный пароль.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
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

        // --------------------------------------------------------------
        // Сохранение
        // --------------------------------------------------------------
        private void BtnSave_Click(object sender, EventArgs e)
        {
            var waters = GetWatersFromGrid();
            var penas = GetPenasFromGrid();
            var sizods = GetSizodsFromGrid();
            var kostyms = GetKostymsFromGrid();

            bool success = true;

            using (var ctx = new stroevkaContext())
            {
                var wr = new WatersRepository(ctx);
                var pr = new PenasRepository(ctx);
                var sr = new SizodsRepository(ctx);
                var kr = new KostymsRepository(ctx);

                if (!wr.SaveWaters(waters)) success = false;
                if (!pr.SavePenas(penas)) success = false;
                if (!sr.SaveSizods(sizods)) success = false;
                if (!kr.SaveKostyms(kostyms)) success = false;
            }

            if (success)
            {
                // Обновляем кэш
                foreach (var w in waters) AppDataCache.Instance.UpdateWater(w);
                foreach (var p in penas) AppDataCache.Instance.UpdatePena(p);
                foreach (var s in sizods) AppDataCache.Instance.UpdateSizod(s);
                foreach (var k in kostyms) AppDataCache.Instance.UpdateKostym(k);

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

        private void OnDataChanged() => DataChanged?.Invoke(this, EventArgs.Empty);
        private void OnSaveRequested() => SaveRequested?.Invoke(this, EventArgs.Empty);
    }
}