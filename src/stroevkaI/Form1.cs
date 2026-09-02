using StorageI.ModelsStroevkaMySql;
using System.Diagnostics;
using stroevkaI.Services.Reports;
using stroevkaI.Forms;
using stroevkaI.Properties;
using stroevkaI.Services.Tests;
using MySql.Data.MySqlClient;
using stroevkaI.Services;
using System.ComponentModel;
using System.Text;
using stroevkaI.Services;


namespace stroevkaI
{
    public partial class Form1 : Form
    {
        #region Параметры программы
        private static string knownExcelFolder = Directory.GetCurrentDirectory() + @"\отчеты\";
        private string templatePath = knownExcelFolder + @"\шаблоны\";

        private List<FirePsgStat> gridList;
        public static DateTime karaul1date = new DateTime(2018, 07, 31);
        public static int караул = ((DateTime.Now.AddHours(-8).Date - karaul1date).Days) % 4 + 1;
        private static int lastKaraul = -1;

        private FirePsgStat rootPsg = null;
        public string rootPsgName = "";
        private FirePsgStat selectedItem = null;
        private List<FirePsgStat> allPsgs;
        private bool isLeftPanelVisible = false;

        private System.Windows.Forms.Timer karaulTimer;
        private System.Windows.Forms.Timer clockTimer;
        private bool isKaraulUpdated = false; // Флаг однократного обновления

        private List<Pch> cachedPchList;
        private List<Psg> cachedPsgList;
        private BackgroundWorker compareAllWorker;

        List<PivotRow> pivotSource;

        #endregion

        #region События формы
        public Form1()
        {
            InitializeComponent();
            BuildTree();
            this.EquipmentDataGridView.AutoGenerateColumns = false;
            //this.btnTools.Sp.sp = true;

            karaulTextBox.Text = "       Караул № "+караул.ToString();

            // Скрываем левую панель при запуске
            //splitContainer1.Panel1Collapsed = true;
            isLeftPanelVisible = false;

            // Загружаем список ПСГ
            LoadPsgList();

            // Инициализируем rootPsgName из Settings
            rootPsgName = Settings.Default.rootGarn;
            if (string.IsNullOrEmpty(rootPsgName))
            {
                rootPsgName = "Территориальный";
                Settings.Default.rootGarn = rootPsgName;
                Settings.Default.Save();
            }

            // Устанавливаем выбранный ПСГ в комбобоксе
            if (!string.IsNullOrEmpty(rootPsgName))
            {
                int index = cmbPsg.FindStringExact(rootPsgName);
                if (index >= 0)
                {
                    cmbPsg.SelectedIndex = index;
                }
                else
                {
                    // Если не найден, выбираем территориальный
                    int territorialIndex = cmbPsg.FindStringExact("Территориальный");
                    if (territorialIndex >= 0)
                    {
                        cmbPsg.SelectedIndex = territorialIndex;
                        rootPsgName = "Территориальный";
                        Settings.Default.rootGarn = rootPsgName;
                        Settings.Default.Save();
                    }
                }
            }

            // Загружаем корневой гарнизон
            rootPsg = FireEquipsPivotRepository.GetPsgByName2(rootPsgName);

            EquipmentDataGridView.AutoGenerateColumns = false;

            InitGrid();
      
            InitPivotGrid(rootPsgName);
            // Подписываемся на события грида
            EquipmentDataGridView.CellValueChanged += EquipmentDataGridView_CellValueChanged;
            EquipmentDataGridView.CurrentCellDirtyStateChanged += EquipmentDataGridView_CurrentCellDirtyStateChanged;
            EquipmentDataGridView.CellPainting += EquipmentDataGridView_CellPainting_1;
            EquipmentDataGridView.DoubleClick += EquipmentDataGridView_DoubleClick;
            EquipmentDataGridView.CellFormatting += EquipmentDataGridView_CellFormatting;
            // Запускаем таймеры
            StartKaraulTimer();
            StartClockTimer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Принудительно обновляем караул при загрузке
                UpdateKaraul();
            cachedPchList = FireEquipsPivotRepository.getPchList();
            cachedPsgList = FireEquipsPivotRepository.getPsgList();
            statusStrip1.Items.Add(new ToolStripStatusLabel("Готово"));
        }

        private void LoadPsgList()
        {
            try
            {
                allPsgs = FireEquipsPivotRepository.LoadAllPsgs();
                if (allPsgs == null)
                {
                    allPsgs = new List<FirePsgStat>();
                }

                // Получаем список ПСГ (уникальные названия)
                var psgNames = allPsgs
                    .Where(p => !string.IsNullOrEmpty(p.Псг) && p.Isitog != 1)
                    .Select(p => p.Псг)
                    .Distinct()
                    .OrderBy(p => p)
                    .ToList();

                cmbPsg.Items.Clear();
                cmbPsg.Items.Add("Территориальный");
                foreach (var name in psgNames)
                {
                    cmbPsg.Items.Add(name);
                }

                // Если есть сохранённое значение, выбираем его
                if (!string.IsNullOrEmpty(Settings.Default.rootGarn))
                {
                    int index = cmbPsg.FindStringExact(Settings.Default.rootGarn);
                    if (index >= 0)
                    {
                        cmbPsg.SelectedIndex = index;
                    }
                }
                else if (cmbPsg.Items.Count > 0)
                {
                    cmbPsg.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки списка ПСГ: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Обработка событий от ComboBox
        private void CmbPsg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPsg.SelectedItem == null) return;

            rootPsgName = cmbPsg.SelectedItem.ToString();
            rootPsg = FireEquipsPivotRepository.GetPsgByName2(rootPsgName);

            // Сохраняем в Settings
            Settings.Default.rootGarn = rootPsgName;
            Settings.Default.Save();

            refreshGrid(rootPsgName);
            InitPivotGrid(rootPsgName);
            UpdateStatus($"Выбран гарнизон: {rootPsgName}");
        }
        #endregion

        #region Управление левой панелью
        private void BtnTools_Click(object sender, EventArgs e)
        {
            ToggleLeftPanel();
        }

        private void ToggleLeftPanel()
        {
            isLeftPanelVisible = !isLeftPanelVisible;
            splitContainer1.Panel1Collapsed = !isLeftPanelVisible;

            btnTools.Text = isLeftPanelVisible ? "Скрыть" : "Инструменты";

            if (isLeftPanelVisible)
            {
                splitContainer1.SplitterDistance = 200;
            }
        }
        #endregion

        #region Процедуры работы с гридом
        void InitGrid()
        {
            gridList = FireEquipsPivotRepository.LoadEquipsByPsg(rootPsgName);
            EquipmentDataGridView.DataSource = gridList;
        }
        void InitPivotGrid(string rootName)
        {
            var lst = PivotTreeBuilder.GetPsgChildes(rootName).OrderBy(c => c.Norder).ToList();
            if (lst == null)
                return;
            PivotRowGrid.DataSource = lst;
        }

        private void refreshGrid(string _psgname)
        {
            if (_psgname == null) return;

            gridList = FireEquipsPivotRepository.LoadEquipsByPsg(_psgname);
            EquipmentDataGridView.DataSource = gridList;
            InitPivotGrid(_psgname);
            HighlightDatafilledRows();
        }

        private void HighlightDatafilledRows()
        {
            foreach (DataGridViewRow row in EquipmentDataGridView.Rows)
            {
                if (row.Cells["Datafilled"]?.Value != null)
                {
                    try
                    {
                        object value = row.Cells["Datafilled"].Value;
                        bool isChecked = false;

                        if (value is bool)
                        {
                            isChecked = (bool)value;
                        }
                        else if (value is string)
                        {
                            string strValue = (string)value;
                            isChecked = strValue == "1" || strValue.Equals("true", StringComparison.OrdinalIgnoreCase);
                        }
                        else if (value is int)
                        {
                            isChecked = (int)value == 1;
                        }
                        else if (value is long)
                        {
                            isChecked = (long)value == 1;
                        }
                        else if (value is byte)
                        {
                            isChecked = (byte)value == 1;
                        }
                        else
                        {
                            isChecked = Convert.ToBoolean(value);
                        }

                        if (isChecked)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.White;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                    catch
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }
        #endregion

        #region Обработка событий от грида
        private void EquipmentDataGridView_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 1)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                e.Graphics.TranslateTransform(e.CellBounds.Left, e.CellBounds.Bottom);
                e.Graphics.RotateTransform(-90);

                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                Rectangle rect = new Rectangle(0, 0, e.CellBounds.Height, e.CellBounds.Width);
                e.Graphics.DrawString(
                    EquipmentDataGridView.Columns[e.ColumnIndex].HeaderText,
                    e.CellStyle.Font,
                    Brushes.Black,
                    rect,
                    format);

                e.Graphics.ResetTransform();
                e.Handled = true;
            }
        }

        private void EquipmentDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (EquipmentDataGridView.Columns[e.ColumnIndex].Name == "Datafilled")
            {
                if (e.Value != null)
                {
                    try
                    {
                        if (e.Value is string)
                        {
                            string strValue = (string)e.Value;
                            e.Value = strValue == "1" || strValue.Equals("true", StringComparison.OrdinalIgnoreCase);
                            e.FormattingApplied = true;
                        }
                        else if (e.Value is int)
                        {
                            e.Value = (int)e.Value == 1;
                            e.FormattingApplied = true;
                        }
                        else if (e.Value is long)
                        {
                            e.Value = (long)e.Value == 1;
                            e.FormattingApplied = true;
                        }
                        else if (e.Value is byte)
                        {
                            e.Value = (byte)e.Value == 1;
                            e.FormattingApplied = true;
                        }
                    }
                    catch
                    {
                        e.Value = false;
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        private void EquipmentDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var checkBoxCell = EquipmentDataGridView.Rows[e.RowIndex].Cells["Datafilled"] as DataGridViewCheckBoxCell;
            if (checkBoxCell != null && checkBoxCell.Value != null)
            {
                try
                {
                    object value = checkBoxCell.Value;
                    bool isChecked = false;

                    if (value is bool)
                    {
                        isChecked = (bool)value;
                    }
                    else if (value is string)
                    {
                        string strValue = (string)value;
                        isChecked = strValue == "1" || strValue.Equals("true", StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        isChecked = Convert.ToBoolean(value);
                    }

                    if (isChecked)
                    {
                        EquipmentDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                        EquipmentDataGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        EquipmentDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        EquipmentDataGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                catch
                {
                    EquipmentDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    EquipmentDataGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void EquipmentDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (EquipmentDataGridView.CurrentCell is DataGridViewCheckBoxCell)
            {
                EquipmentDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        string lastChoose = "";
        /// <summary>
        /// Почему заходит 2 раза?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EquipmentDataGridView_DoubleClick(object sender, EventArgs e)
        {
            
            if (EquipmentDataGridView.CurrentRow != null)
            {
                selectedItem = (FirePsgStat)EquipmentDataGridView.CurrentRow.DataBoundItem;
                if (selectedItem != null)
                {
                    if (selectedItem.Isitog == 1)//если строка итогов = и это районный ПСГ, то изменить выбор в combobox
                    {
                        var str = selectedItem.Псг;
                        if (cmbPsg.Items.Contains(str))
                            //lastChoose = cmbPsg.Text;
                            //if (str == cmbPsg.Text)
                            //    str = "Территориальный"; // если клик на уже выбранном ПСГ то возврат к Территориальному
                            cmbPsg.Text = str;
                        return;
                    }


                    using (var editorForm = new EditorsForm(selectedItem))
                    {
                        editorForm.ShowDialog();
                    }
                    refreshGrid(rootPsgName);
                }
            }
        }
        #endregion

        #region Обработка кнопок и инструментов
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (rootPsg != null && rootPsg.Псг.Contains("Территориал"))
                    cppsReport.myReport(EquipmentDataGridView);
                else
                    psgReport.printLocal(rootPsgName, EquipmentDataGridView);

                UpdateStatus("Печать выполнена");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при печати: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListBoxTools_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTools.SelectedItem == null) return;

            string selectedItem = listBoxTools.SelectedItem.ToString();

            if (selectedItem == "Сравнение с БД")
            {
                BtnCompare_Click(sender, e);
                listBoxTools.SelectedIndex = -1;
            }
            else if (selectedItem == "Сравнение всех")
                compareAllPsg();
            else if (selectedItem == "TreeBuilder") { 
                BuildTree();
            }
        }
        
        //Строим дерево узлов и дерево PivotRows
        private void BuildTree() {
            PivotTreeBuilder b = new PivotTreeBuilder();
            Models.ReportNode  root = b.BuildTree();
            pivotSource  = b.GeneratePivotRows(root);        
        }


        private void BtnCompare_Click(object sender, EventArgs e)
        {
            string rezStr = "";
            try
            {
                UpdateStatus("Выполняется сравнение с БД...");
                string psgName = cmbPsg.Text.Trim();
                rezStr = bdService.psgdataCompare(psgName, EquipmentDataGridView);

                MessageBox.Show(rezStr, "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                UpdateStatus("Сравнение завершено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(rezStr, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show($"Ошибка при сравнении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus(string message)
        {
            if (statusStrip1 != null && statusStrip1.Items.Count > 0)
            {
                statusStrip1.Items[0].Text = message;
            }
        }
        #endregion

        #region Таймеры
        private void StartKaraulTimer()
        {
            karaulTimer = new System.Windows.Forms.Timer();
            karaulTimer.Interval = 300000; // 5 минут
            karaulTimer.Tick += KaraulTimer_Tick;
            karaulTimer.Start();
        }

        private void KaraulTimer_Tick(object sender, EventArgs e)
        {
            // Проверяем, не изменился ли караул
            int newKaraul = CalculateKaraul();

            if (newKaraul != караул)
            {
                караул = newKaraul;
                OnKaraulChanged();
            }
        }
        private int CalculateKaraul()
        {
            // Формула расчёта караула с учётом смены в 8:00
            DateTime now = DateTime.Now;
            DateTime adjustedDate = now.AddHours(-8);
            return ((adjustedDate.Date - karaul1date.Date).Days % 4) + 1;
        }

        private void UpdateKaraul()
        {
            int newKaraul = CalculateKaraul();

            if (newKaraul != караул)
            {
                // Караул изменился
                караул = newKaraul;
                OnKaraulChanged();
            }
            else if (!isKaraulUpdated)
            {
                // При первом запуске обновляем
                OnKaraulChanged();
            }
        }

        private void OnKaraulChanged()
        {
            isKaraulUpdated = true;
            lastKaraul = караул;

            // Обновляем отображение
            UpdateKaraulDisplay();

            // Обновляем кэш начальников караулов в БД
            UpdateCacheNachkar();

            // Оповещаем все формы/контролы о смене караула
            OnKaraulChangedGlobal();

            UpdateStatus($"Смена караула: №{караул}");
        }

        private void UpdateKaraulDisplay()
        {
            karaulTextBox.Text = $"Караул №{караул}";

            // Изменяем цвет в зависимости от караула
            switch (караул)
            {
                case 1:
                    karaulTextBox.BackColor = System.Drawing.Color.LightGreen;
                    break;
                case 2:
                    karaulTextBox.BackColor = System.Drawing.Color.LightBlue;
                    break;
                case 3:
                    karaulTextBox.BackColor = System.Drawing.Color.LightYellow;
                    break;
                case 4:
                    karaulTextBox.BackColor = System.Drawing.Color.LightPink;
                    break;
                default:
                    karaulTextBox.BackColor = System.Drawing.Color.White;
                    break;
            }
        }

        private void UpdateCacheNachkar()
        {
            try
            {
                // Обновляем таблицу cache_nachkar в БД
                // Здесь вызываем метод для обновления кэша
                FireEquipsPivotRepository.UpdateCacheNachkar(караул);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка обновления cache_nachkar: {ex.Message}");
            }
        }

        private void OnKaraulChangedGlobal()
        {
            // Генерируем глобальное событие для всех форм и контролов
            var args = new KaraulChangedEventArgs { NewKaraul = караул };
            KaraulChanged?.Invoke(this, args);
        }

        // Глобальное событие для оповещения о смене караула
        public static event EventHandler<KaraulChangedEventArgs> KaraulChanged;


        private void StartClockTimer()
        {
            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 1000; // 1 секунда
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            // Обновляем время на форме (если есть поле для времени)
             timeTextBox.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        #endregion

        #region Работа с полем Караул
        // Класс для передачи данных о смене караула
        public class KaraulChangedEventArgs : EventArgs
        {
            public int NewKaraul { get; set; }
            public int OldKaraul { get; set; }
        }
        private void karaulTextBox_Click(object sender, EventArgs e)
        {
            UpdateKaraul();
        }
        #endregion

        #region Генератор представлений строёвки
        string connectionString = @"server=localhost;port=3306;user=root;password=Djkjlz1; database=stroevka; Character Set = utf8; Convert Zero Datetime=True; Allow Zero Datetime=True";
        private void generator_Click(object sender, EventArgs e)
        {
            try
            {
                // Генерируем SQL
                string sql = ViewGenerator.GenerateAllViews();

                // ОТЛАДКА: сохраняем SQL в файл для просмотра
                System.IO.File.WriteAllText(@"C:\temp\generated_sql.txt", sql);
                //MessageBox.Show($"SQL сохранен в C:\\temp\\generated_sql.txt\nДлина: {sql.Length} символов", "Отладка");

                // Выполняем SQL по частям
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                // Разбиваем на отдельные команды
                var commands = sql.Split(new[] { "CREATE OR REPLACE" }, StringSplitOptions.RemoveEmptyEntries);

                int commandIndex = 0;
                foreach (var cmd in commands)
                {
                    commandIndex++;
                    var fullCmd = "CREATE OR REPLACE " + cmd.Trim();

                    // Пропускаем пустые команды
                    if (string.IsNullOrWhiteSpace(fullCmd) || fullCmd == "CREATE OR REPLACE")
                        continue;

                    try
                    {
                        //string sss = fullCmd.Substring(0,3000);
                        using var command = new MySqlCommand(fullCmd, connection);
                        command.ExecuteNonQuery();
                        Console.WriteLine($"? Команда {commandIndex} выполнена");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка в команде {commandIndex}:\n{ex.Message}\n\nSQL:\n{fullCmd.Substring(0, 120)}...",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }

                MessageBox.Show("Все представления успешно созданы!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void pchsRefresh_Click(object sender, EventArgs e)
        {
            RowIdService serv = new RowIdService();
            //serv.UpdateAllPchRowIds();
            serv.UpdateAllRowIds();
//            serv.UpdateAllPsgRowIds();
            MessageBox.Show("pch/psg rowId обновлены");
        }
        

        private void toolStripRight_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void compareAllPsg() {

            if (compareAllWorker != null && compareAllWorker.IsBusy)
                return;

            //btnCompareAll.Enabled = false;
            compareAllWorker = new BackgroundWorker();
            compareAllWorker.WorkerReportsProgress = true;
            compareAllWorker.DoWork += CompareAllWorker_DoWork;
            compareAllWorker.ProgressChanged += CompareAllWorker_ProgressChanged;
            compareAllWorker.RunWorkerCompleted += CompareAllWorker_RunWorkerCompleted;
            compareAllWorker.RunWorkerAsync();

        }
        private void CompareAllWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var psgNames = cmbPsg.Items.Cast<string>().ToList();
            int total = psgNames.Count;
            int current = 0;
            var allResults = new Dictionary<string, List<GridComparisonResult>>();

            foreach (string psgName in psgNames)
            {
                current++;
                worker.ReportProgress(current * 100 / total, $"Сравнение {psgName}...");

                // Переключение ПСГ в UI-потоке
                this.Invoke((MethodInvoker)delegate
                {
                    int idx = cmbPsg.FindStringExact(psgName);
                    if (idx >= 0)
                        cmbPsg.SelectedIndex = idx;
                });

                // Ожидание обновления грида (3 секунды)
                System.Threading.Thread.Sleep(3000);

                var results = CompareSinglePsg(psgName);
                allResults[psgName] = results;
            }

            e.Result = allResults;
        }

        private void CompareAllWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (statusStrip1.Items.Count > 0)
                statusStrip1.Items[0].Text = e.UserState?.ToString() ?? "";
        }

        private void CompareAllWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
          //  btnCompareAll.Enabled = true;
            if (e.Error != null)
            {
                MessageBox.Show($"Ошибка: {e.Error.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var allResults = e.Result as Dictionary<string, List<GridComparisonResult>>;
            if (allResults == null) return;

            // Подсчёт статистики
            int totalErrors = 0;
            var sb = new StringBuilder();
            foreach (var kvp in allResults)
            {
                int errors = kvp.Value.Sum(r => r.Differences.Count);
                totalErrors += errors;
                sb.AppendLine($"{kvp.Key}: {errors} расхождений");
            }
            sb.Insert(0, $"Всего расхождений: {totalErrors}\n\n");
            MessageBox.Show(sb.ToString(), "Результаты сравнения всех ПСГ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private List<GridComparisonResult> CompareSinglePsg(string psgName)
        {
            // Формируем путь к Excel-файлу
            string dateStr = DateTime.Now.AddDays(-1).ToString("dd-MM-yy");
            string excelFilePath = @"D:\stroevka_reports\" + psgName + "_" + dateStr + ".xlsx";
            if (!File.Exists(excelFilePath))
            {
                // Если файла за сегодня нет, берём последний доступный
                var dir = new DirectoryInfo(@"D:\stroevka_reports\");
                var files = dir.GetFiles(psgName + "_*.xls");
                if (files.Length == 0)
                    return new List<GridComparisonResult>(); // или выбросить исключение
                excelFilePath = files.OrderByDescending(f => f.LastWriteTime).First().FullName;
            }

            var psg = FireEquipsPivotRepository.GetPsgByName(psgName);
            if (psg == null)
                return new List<GridComparisonResult>();

            // Читаем Excel
            var reader = new ExcelReaderService(cachedPchList, cachedPsgList);
            var excelData = reader.ReadExcelFile(excelFilePath, psg, EquipmentDataGridView.Rows.Count);

            // Сравниваем с гридом
            var comparer = new GridComparer(EquipmentDataGridView);
            var results = comparer.CompareAll(excelData);

            return results;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void EquipmentDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}