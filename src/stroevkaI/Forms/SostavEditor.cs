// stroevkaI/Forms/SostavEditor.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;

namespace stroevkaI.Forms
{
    public partial class SostavEditor : UserControl,IDataEditor
    {
        public event EventHandler DataChanged;
        public event EventHandler SaveRequested;

        private readonly SostavRepository _repository;
        private  int _subdivisionId;
        private readonly string _subdivisionName;
        private List<Sostav> _currentData;
        private bool _isConstEditEnabled = false;
        private const string ADMIN_PASSWORD = "111111";

        // Список вычисляемых полей (запрещены для редактирования всегда)
        private readonly HashSet<string> _calculatedFields = new HashSet<string>
        {
            "Всего",     // В группе "2 Боевой расчет"
            "ЛС в БР",
            "Налицо",
            // "Всего" в группе "4 Отсутствует" тоже вычисляется
            // "Всего" в группе "1 Общие" тоже вычисляется
        };

        // Поле "Всего" в группе "1 Общие" - редактируется только при включенном чекбоксе
        private const string TOTAL_FIELD_NAME = "Всего";
        private const string TOTAL_GROUP = "1 Общие";

        // События для связи с формой


        // Конструктор для дизайнера
        public SostavEditor()
        {
            InitializeComponent();
            SetupDataGridView();
            _repository = new SostavRepository(new stroevkaContext());
        }

        // Основной конструктор
        public SostavEditor(int subdivisionId)
            : this()
        {
            _subdivisionId = subdivisionId;

            var pch = FireEquipsPivotRepository.getPchById(_subdivisionId);
            if (pch != null)
            {
                _subdivisionName = pch.Name;
            }
            else
            {
                _subdivisionName = $"ПЧ {_subdivisionId}";
            }

            LoadData();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SetupDataGridView()
        {
            dgvSostav.AllowUserToAddRows = false;
            dgvSostav.AllowUserToDeleteRows = false;
            dgvSostav.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSostav.MultiSelect = false;
            dgvSostav.RowHeadersVisible = false;
            dgvSostav.BackgroundColor = Color.White;
            dgvSostav.BorderStyle = BorderStyle.Fixed3D;
            dgvSostav.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgvSostav.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvSostav.CellEnter += DgvSostav_CellEnter;

            colName.HeaderText = "Параметр";
            colName.Width = 300;
            colName.ReadOnly = true;
            colName.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F);

            colCount.HeaderText = "Количество";
            colCount.Width = 120;
            colCount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colCount.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F);

            dgvSostav.RowTemplate.Height = 25;
        }

        private void DgvSostav_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (e.ColumnIndex != colCount.Index) return;

            var row = dgvSostav.Rows[e.RowIndex];
            if (row.ReadOnly) return;
            if (row.Cells[e.ColumnIndex].ReadOnly) return;   // ← ключевое

            dgvSostav.BeginEdit(true);
            if (dgvSostav.EditingControl is TextBox tb) tb.SelectAll();
        }
        public void LoadData()
        {
            if (_repository == null)
            {
                lblTitle.Text = "Личный состав - репозитарий не инициализирован";
                return;
            }

            _currentData = _repository.LoadSostav(_subdivisionId);

            // ПЕРЕСЧИТЫВАЕМ ВСЕ ВЫЧИСЛЯЕМЫЕ ПОЛЯ
            RecalculateAllCalculatedFields();

            // ОБНОВЛЯЕМ ГРИД
            RefreshGrid();
            UpdateTitle();

            // ============================================================
            // ПРОВЕРКА ПОСЛЕ ПОЛНОГО ОБНОВЛЕНИЯ ГРИДА
            // ============================================================
            // Используем Application.Idle для выполнения проверки после полной загрузки формы
            EventHandler idleHandler = null;
            idleHandler = (s, e) =>
            {
                Application.Idle -= idleHandler;
                CheckAndWarnAboutDataInconsistency();
            };
            Application.Idle += idleHandler;
        }

        public void LoadData(int pchId)
        {
            _subdivisionId = pchId;
            LoadData();
        }

        /// <summary>
        /// Проверяет данные на соответствие формуле при загрузке
        /// </summary>
        //private void CheckAndWarnAboutDataInconsistency()
        //{
        //    if (_currentData == null) return;

        //    var totalGeneral = _currentData.FirstOrDefault(s => s.Name == "Всего" && s.SostavVid == "1 Общие");
        //    var nalico = _currentData.FirstOrDefault(s => s.Name == "Налицо" && s.SostavVid == "1 Общие");
        //    var totalAbsent = _currentData.FirstOrDefault(s => s.Name == "Всего" && s.SostavVid == "4 Отсутствует");

        //    if (totalGeneral != null && nalico != null && totalAbsent != null)
        //    {
        //        int expectedValue = (nalico.Count ?? 0) + (totalAbsent.Count ?? 0);
        //        int actualValue = totalGeneral.Count ?? 0;

        //        if (actualValue != expectedValue)
        //        {
        //            // Показываем предупреждение при загрузке
        //            MessageBox.Show(
        //                $"ВНИМАНИЕ: ОБНАРУЖЕНА ОШИБКА В ДАННЫХ!\n\n" +
        //                $"Параметр 'Всего' (группа '1 Общие') должен быть равен сумме:\n" +
        //                $"  'Налицо' (1 Общие) + 'Всего' (4 Отсутствует)\n\n" +
        //                $"Текущие значения:\n" +
        //                $"  Налицо: {nalico.Count ?? 0}\n" +
        //                $"  Всего (4 Отсутствует): {totalAbsent.Count ?? 0}\n" +
        //                $"  Ожидаемое значение 'Всего' (1 Общие): {expectedValue}\n" +
        //                $"  Фактическое значение 'Всего' (1 Общие): {actualValue}\n" +
        //                $"  Разница: {actualValue - expectedValue}\n\n" +
        //                $"Для исправления:\n" +
        //                $"  1. Измените значения 'Налицо' или 'Всего' (4 Отсутствует)\n" +
        //                $"  2. ИЛИ включите 'Редактирование постоянных' и исправьте 'Всего' (1 Общие) вручную",
        //                "Обнаружена ошибка в данных",
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Warning
        //            );
        //        }
        //        else
        //        {
        //            // Данные корректны - показываем сообщение об успехе (опционально)
        //            // MessageBox.Show("Данные корректны.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //}

        private void CheckAndWarnAboutDataInconsistency()
        {
            if (_currentData == null) return;

            // Ищем поле "По списку" в группе "1 Общие" (это аналог "Всего" в 1 Общие)
            var totalGeneral = _currentData.FirstOrDefault(s => s.Name == "По списку" && s.SostavVid == "1 Общие");
            // Ищем "Налицо" в группе "1 Общие"
            var nalico = _currentData.FirstOrDefault(s => s.Name == "Налицо" && s.SostavVid == "1 Общие");
            // Ищем "Всего" в группе "4 Отсутствует"
            var totalAbsent = _currentData.FirstOrDefault(s => s.Name == "Всего" && s.SostavVid == "4 Отсутствует");

            // Отладочный вывод
            System.Diagnostics.Debug.WriteLine($"=== CheckAndWarnAboutDataInconsistency ===");
            System.Diagnostics.Debug.WriteLine($"totalGeneral (По списку): {totalGeneral?.Name} = {totalGeneral?.Count}");
            System.Diagnostics.Debug.WriteLine($"nalico: {nalico?.Name} = {nalico?.Count}");
            System.Diagnostics.Debug.WriteLine($"totalAbsent: {totalAbsent?.Name} = {totalAbsent?.Count}");

            if (totalGeneral != null && nalico != null && totalAbsent != null)
            {
                int expectedValue = (nalico.Count ?? 0) + (totalAbsent.Count ?? 0);
                int actualValue = totalGeneral.Count ?? 0;

                System.Diagnostics.Debug.WriteLine($"expectedValue: {expectedValue}, actualValue: {actualValue}");

                if (actualValue != expectedValue)
                {
                    // Проверяем, что форма уже создана и видима
                    if (this.IsHandleCreated && this.Visible)
                    {
                        // Показываем предупреждение при загрузке
                        MessageBox.Show(
                            this,
                            $"ВНИМАНИЕ: ОБНАРУЖЕНА ОШИБКА В ДАННЫХ!\n\n" +
                            $"Параметр 'По списку' (группа '1 Общие') должен быть равен сумме:\n" +
                            $"  'Налицо' (1 Общие) + 'Всего' (4 Отсутствует)\n\n" +
                            $"Текущие значения:\n" +
                            $"  Налицо: {nalico.Count ?? 0}\n" +
                            $"  Всего (4 Отсутствует): {totalAbsent.Count ?? 0}\n" +
                            $"  Ожидаемое значение 'По списку': {expectedValue}\n" +
                            $"  Фактическое значение 'По списку': {actualValue}\n" +
                            $"  Разница: {actualValue - expectedValue}\n\n" +
                            $"Для исправления:\n" +
                            $"  1. Измените значения 'Налицо' или 'Всего' (4 Отсутствует)\n" +
                            $"  2. ИЛИ включите 'Редактирование постоянных' и исправьте 'По списку' вручную",
                            "Обнаружена ошибка в данных",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Данные корректны!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Не найдены необходимые строки для проверки!");
                System.Diagnostics.Debug.WriteLine($"totalGeneral: {(totalGeneral != null ? "найден" : "НЕ НАЙДЕН")}");
                System.Diagnostics.Debug.WriteLine($"nalico: {(nalico != null ? "найден" : "НЕ НАЙДЕН")}");
                System.Diagnostics.Debug.WriteLine($"totalAbsent: {(totalAbsent != null ? "найден" : "НЕ НАЙДЕН")}");
            }
        }

        private void RefreshGrid()
        {
            #region Сохраняем текущую выбранную строку и удаляем все строки
            int selectedRowIndex = -1;
            if (dgvSostav.CurrentRow != null && dgvSostav.CurrentRow.Tag is Sostav)
            {
                selectedRowIndex = dgvSostav.CurrentRow.Index;
            }

            dgvSostav.Rows.Clear();

            if (_currentData == null || !_currentData.Any())
            {
                lblTitle.Text = "Личный состав - нет данных";
                return;
            }
            #endregion
            var groups = _currentData
                .GroupBy(s => s.SostavVid ?? "Без группы")
                .OrderBy(g => g.Key);

            int currentRowIndex = 0;
            foreach (var group in groups)
            {
                #region Групповая строка
                int groupRowIndex = dgvSostav.Rows.Add();
                var groupRow = dgvSostav.Rows[groupRowIndex];
                groupRow.Cells["colName"].Value = group.Key;
                groupRow.Cells["colCount"].Value = "";
                groupRow.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                groupRow.DefaultCellStyle.Font = new Font(dgvSostav.Font, FontStyle.Bold);
                groupRow.DefaultCellStyle.ForeColor = Color.DarkBlue;
                groupRow.Tag = "GROUP";
                groupRow.ReadOnly = true;
                groupRow.Height = 28;
                #endregion
                foreach (var item in group.OrderBy(s => s.Norder))
                {
                    int rowIndex = dgvSostav.Rows.Add();
                    var row = dgvSostav.Rows[rowIndex];
                    row.Cells["colId"].Value = item.Id;
                    row.Cells["colName"].Value = item.Name;

                    // Проверяем, является ли строка(поле) вычисляемой
                    bool isCalculated = IsCalculatedField(item.Name, item.SostavVid);

                    if (isCalculated)
                    {
                        // ВЫЧИСЛЯЕМОЕ ПОЛЕ - значение берётся из item.Count (уже пересчитано)
                        row.Cells["colCount"].Value = item.Count;
                        row.DefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
                        row.DefaultCellStyle.Font = new Font(dgvSostav.Font, FontStyle.Bold);
                        row.Tag = item;
                        row.ReadOnly = true; // Всегда только для чтения
                    }
                    else
                    {
                        // Обычное поле - можно редактировать
                        row.Cells["colCount"].Value = item.Count;
                        row.Tag = item;

                        bool isTotalField = (item.Name == TOTAL_FIELD_NAME && item.SostavVid == TOTAL_GROUP);
                        if (isTotalField)
                        {
                            row.ReadOnly = !_isConstEditEnabled;
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 224);
                            row.DefaultCellStyle.Font = new Font(dgvSostav.Font, FontStyle.Italic);
                        }
                        else
                        {
                            row.ReadOnly = false;
                        }
                    }
                    #region Определяем, редактируется ли эта строка
                    bool editable =
                        !IsAlwaysReadOnlyField(item.Name, item.SostavVid) &&
                        (!IsPasswordOnlyField(item.Name, item.SostavVid) || _isConstEditEnabled);

                    row.ReadOnly = !editable;
                    row.Cells["colCount"].ReadOnly = !editable;   // ← ключевое

                    // Цветовая индикация
                    if (IsAlwaysReadOnlyField(item.Name, item.SostavVid))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
                        row.DefaultCellStyle.Font = new Font(dgvSostav.Font, FontStyle.Bold);
                    }
                    else if (IsPasswordOnlyField(item.Name, item.SostavVid))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 224);
                        row.DefaultCellStyle.Font = new Font(dgvSostav.Font, FontStyle.Italic);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.Font = dgvSostav.Font;
                    }

                    row.Tag = item;
                    #endregion


                    #region Восстанавливаем выбор
                    row.Height = 25;
                    if (selectedRowIndex >= 0 && currentRowIndex == selectedRowIndex)
                    {
                        dgvSostav.CurrentCell = row.Cells["colCount"];
                    }
                    currentRowIndex++;//TODO разобраться зачем это?
                    #endregion
                }
            }

            dgvSostav.AutoResizeColumns();
        }

        /// <summary>
        /// Проверяет, является ли поле вычисляемым
        /// </summary>
        private bool IsCalculatedField(string name, string sostavVid = null)
        {
            // Всегда вычисляемые
            if (IsAlwaysReadOnlyField(name, sostavVid)) return true;

            // По списку (1 Общие) — вычисляемым НЕ является:
            // либо доступно только по паролю, либо недоступно вообще.
            // Но в режиме ручного редактирования данные из грида в него пишутся.

            return false;
        }

        private bool IsAlwaysReadOnlyField(string name, string sostavVid)
        {
            if (name == "Налицо" && sostavVid == "1 Общие") return true;
            if (name == "Всего" && sostavVid == "2 Боевой расчет") return true;
            if (name == "ЛС в БР" && sostavVid == "2 Боевой расчет") return true;
            if (name == "Всего" && sostavVid == "4 Отсутствует") return true;
            return false;
        }

        private bool IsPasswordOnlyField(string name, string sostavVid)
        {
            return name == "По списку" && sostavVid == "1 Общие";
        }

        /// <summary>
        /// ВЫЧИСЛЯЕТ значение для поля на основе других полей
        /// </summary>
        private int CalculateField(string name, string sostavVid)
        {
            if (_currentData == null) return 0;

            // 1. ЛС в БР (2 Боевой расчет) = ПНК + КО + Пожарные + Водители
            if (name == "ЛС в БР" && sostavVid == "2 Боевой расчет")
            {
                var names = new List<string> { "ПНК", "КО", "Пожарные", "Водители" };
                return _currentData
                    .Where(s => names.Contains(s.Name) && s.SostavVid == "2 Боевой расчет")
                    .Sum(s => s.Count ?? 0);
            }

            // 2. Всего (2 Боевой расчет) = ЛС в БР + НК
            if (name == "Всего" && sostavVid == "2 Боевой расчет")
            {
                int lsInBr = _currentData
                    .Where(s => s.Name == "ЛС в БР" && s.SostavVid == "2 Боевой расчет")
                    .Sum(s => s.Count ?? 0);
                int nk = _currentData
                    .Where(s => s.Name == "НК" && s.SostavVid == "2 Боевой расчет")
                    .Sum(s => s.Count ?? 0);
                return lsInBr + nk;
            }

            // 3. Налицо (1 Общие) = Всего (2 Боевой расчет) + Диспетчер (2 Боевой расчет)
            if (name == "Налицо" && sostavVid == "1 Общие")
            {
                int totalCombat = _currentData
                    .Where(s => s.Name == "Всего" && s.SostavVid == "2 Боевой расчет")
                    .Sum(s => s.Count ?? 0);
                int dispatcher = _currentData
                    .Where(s => s.Name == "Диспетчер" && s.SostavVid == "2 Боевой расчет")
                    .Sum(s => s.Count ?? 0);
                return totalCombat + dispatcher;
            }

            // 4. Всего (4 Отсутствует) = По списку (1 Общие) − Налицо (1 Общие)
            if (name == "Всего" && sostavVid == "4 Отсутствует")
            {
                int poSpisku = _currentData
                    .Where(s => s.Name == "По списку" && s.SostavVid == "1 Общие")
                    .Sum(s => s.Count ?? 0);
                int nalico = _currentData
                    .Where(s => s.Name == "Налицо" && s.SostavVid == "1 Общие")
                    .Sum(s => s.Count ?? 0);
                return poSpisku - nalico;
            }

            // 5. По списку (1 Общие) НЕ вычисляется — вводится вручную
            //    (через форму установки параметров или по паролю)
            return 0;
        }
        /// <summary>
        /// ПЕРЕСЧИТЫВАЕТ ВСЕ вычисляемые поля в данных
        /// </summary>
        private void RecalculateAllCalculatedFields()
        {
            if (_currentData == null) return;

            // 1. ЛС в БР (2 БР) = ПНК + КО + Пожарные + Водители
            var lsInBr = _currentData.FirstOrDefault(s =>
                s.Name == "ЛС в БР" && s.SostavVid == "2 Боевой расчет");
            if (lsInBr != null)
                lsInBr.Count = CalculateField("ЛС в БР", "2 Боевой расчет");

            // 2. Всего (2 БР) = ЛС в БР + НК
            var totalCombat = _currentData.FirstOrDefault(s =>
                s.Name == "Всего" && s.SostavVid == "2 Боевой расчет");
            if (totalCombat != null)
                totalCombat.Count = CalculateField("Всего", "2 Боевой расчет");

            // 3. Налицо (1 Общие) = Всего (2 БР) + Диспетчер (2 БР)
            var nalico = _currentData.FirstOrDefault(s =>
                s.Name == "Налицо" && s.SostavVid == "1 Общие");
            if (nalico != null)
                nalico.Count = CalculateField("Налицо", "1 Общие");

            // 4. По списку (1 Общие) — НЕ пересчитываем, вводится вручную

            // 5. Всего (4 Отсутствует) = По списку (1) − Налицо (1)
            var totalAbsent = _currentData.FirstOrDefault(s =>
                s.Name == "Всего" && s.SostavVid == "4 Отсутствует");
            if (totalAbsent != null)
                totalAbsent.Count = CalculateField("Всего", "4 Отсутствует");
        }
        /// <summary>
        /// Обновляет вычисляемые поля в гриде без полной перестройки
        /// </summary>
        private void UpdateCalculatedFieldsInGrid()
        {
            foreach (DataGridViewRow row in dgvSostav.Rows)
            {
                if (row.Tag is not Sostav item) continue;
                if (IsCalculatedField(item.Name, item.SostavVid))
                {
                    row.Cells["colCount"].Value = item.Count;
                }
            }
        }

        private void UpdateTitle()
        {
            int count = _currentData?.Count ?? 0;
            lblTitle.Text = $"Личный состав - {count} записей";
        }

        private List<Sostav> GetDataFromGrid()
        {
            var result = new List<Sostav>();

            foreach (DataGridViewRow row in dgvSostav.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Tag is not Sostav item) continue;

                // Читаем значение из грида, если поле НЕ является "вычисляемым".
                // По списку (1) — редактируемое, поэтому попадает сюда.
                if (!IsCalculatedField(item.Name, item.SostavVid))
                {
                    int newCount = ParseInt(row.Cells["colCount"].Value);
                    item.Count = newCount;
                }

                result.Add(item);
            }
            return result;
        }

        private static int ParseInt(object v)
            => v == null ? 0 : (int.TryParse(v.ToString(), out int i) ? i : 0);

        private void OnDataChanged()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveRequested()
        {
            SaveRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Пересчитываем всё перед сохранением
            RecalculateAllCalculatedFields();

            var dataToSave = GetDataFromGrid();

            // Проверка: Всего (4) должно быть >= 0
            var totalAbsent = dataToSave.FirstOrDefault(s =>
                s.Name == "Всего" && s.SostavVid == "4 Отсутствует");
            if (totalAbsent != null && (totalAbsent.Count ?? 0) < 0)
            {
                MessageBox.Show(this,
                    $"Ошибка: 'Всего (4 Отсутствует)' не может быть отрицательным.\n" +
                    $"По списку (1) < Налицо (1).",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_repository.SaveSostav(dataToSave))
            {
                MessageBox.Show("Данные сохранены.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                OnSaveRequested();
            }
            else
            {
                MessageBox.Show("Ошибка сохранения.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
                _isConstEditEnabled = true;
                btnSaveConst.Enabled = true;
            }
            else
            {
                _isConstEditEnabled = false;
                btnSaveConst.Enabled = false;
                RecalculateAllCalculatedFields();
            }

            RefreshGrid();   // ← перерисовка учтёт флаг и заново расставит ReadOnly
        }
        private void DgvSostav_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvSostav.Rows[e.RowIndex];
            if (row.Tag is string && (string)row.Tag == "GROUP")
            {
                bool isGroupVisible = !row.Visible;
                row.Visible = isGroupVisible;

                for (int i = e.RowIndex + 1; i < dgvSostav.Rows.Count; i++)
                {
                    var nextRow = dgvSostav.Rows[i];
                    if (nextRow.Tag is string && (string)nextRow.Tag == "GROUP")
                        break;
                    nextRow.Visible = isGroupVisible;
                }
            }
        }

        private void DgvSostav_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == colName.Index)
            {
                dgvSostav.Sort(dgvSostav.Columns[colName.Index],
                    System.ComponentModel.ListSortDirection.Ascending);
            }
            else if (e.ColumnIndex == colCount.Index)
            {
                dgvSostav.Sort(dgvSostav.Columns[colCount.Index],
                    System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        // Обработчик события CellEndEdit - ПЕРЕСЧЁТ БЕЗ ПОЛНОЙ ПЕРЕСТРОЙКИ ГРИДА
        private void DgvSostav_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvSostav.Rows[e.RowIndex];
            if (row.Tag is not Sostav item) return;

            // Обновляем данные из грида
            if (!IsCalculatedField(item.Name, item.SostavVid))
            {
                item.Count = ParseInt(row.Cells["colCount"].Value);
            }

            // Пересчитываем всю цепочку зависимостей
            RecalculateAllCalculatedFields();

            // Обновляем только вычисляемые строки в гриде (не всю таблицу)
            UpdateCalculatedFieldsInGrid();

            OnDataChanged();
        }

    }
}