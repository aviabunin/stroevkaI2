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
using StorageI.Services;


namespace stroevkaI
{
    public partial class Form1 : Form
    {
        #region Параметры программы

        PivotRowEditor _pivotEditor;
        public event EventHandler DataChanged;
        private bool _isRefreshing = false;

        private List<PivotRow> _allPivotRows;
        private readonly Dictionary<string, int> _psgNameToId = new();

        public stroevkaContext context = new stroevkaContext();
        private JsonDataService _jsonService;
        private AppStatusService _appStatus;
        private PivotTreeBuilder _treeBuilder;
        private System.Windows.Forms.Timer _autoSaveTimer;

        JsonDataService jsonService;// = new JsonDataService(@"\\server\shared\psg_data"); // сетевой путь
        private DataSyncManager _syncManager;

        private static string knownExcelFolder = Directory.GetCurrentDirectory() + @"\отчеты\";
        private string templatePath = knownExcelFolder + @"\шаблоны\";

        public static DateTime karaul1date = new DateTime(2018, 07, 31);
        public static int караул = ((DateTime.Now.AddHours(-8).Date - karaul1date).Days) % 4 + 1;
        private static int lastKaraul = -1;

        private Psg rootPsg = null;
        private PsgTotalRow rootPsg1 = null;

        public string rootPsgName = "";
        private PivotRow selectedItem1 = null;
        private List<Psg> allPsgs;
        private bool isLeftPanelVisible = false;

        private System.Windows.Forms.Timer karaulTimer;
        private System.Windows.Forms.Timer clockTimer;
        private bool isKaraulUpdated = false; // Флаг однократного обновления

        private List<Pch> cachedPchList;
        private List<Psg> cachedPsgList;
        private BackgroundWorker compareAllWorker;

        List<PivotRow> pivotSource;
        private ColumnVisibilityManager _columnManager;

        #endregion

        #region События формы

        private AppConfig _config = AppConfig.Load();
        public Form1()
        {
            InitializeComponent();

            PivotRowGrid.AutoGenerateColumns = false;

            karaulTextBox.Text = "       Караул № " + караул;

            PivotRowGrid.DataSource = new List<PivotRow>();


            _columnManager = new ColumnVisibilityManager(PivotRowGrid);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();
            void Mark(string stage) => Log.Mark(stage, sw.ElapsedMilliseconds, 0);

            Mark("Start");

            // Один раз загружаем все строки pivot_rows
            _allPivotRows = FastPivotLoader.LoadAll();
            Mark($"LoadAll ({_allPivotRows.Count})");

            // Заполняем cmbPsg — только Территориальный + 18 районных ПСГ
            LoadPsgListFromDb();
            Mark($"LoadPsgList ({cmbPsg.Items.Count})");

            // Ставим сохранённый ПСГ
            rootPsgName = Settings.Default.rootGarn;
            if (string.IsNullOrEmpty(rootPsgName) || !_psgNameToId.ContainsKey(rootPsgName))
                rootPsgName = "Территориальный";

            cmbPsg.SelectedIndexChanged -= CmbPsg_SelectedIndexChanged;
            int idx = cmbPsg.FindStringExact(rootPsgName);
            cmbPsg.SelectedIndex = idx >= 0 ? idx : 0;
            cmbPsg.SelectedIndexChanged += CmbPsg_SelectedIndexChanged;

            ShowView(rootPsgName);
            Mark($"ShowView({rootPsgName})");
        }

        private void LoadPsgListFromDb()
        {
            cmbPsg.Items.Clear();
            _psgNameToId.Clear();

            using var conn = new MySqlConnection(DbConfig.BuildConnectionString());
            conn.Open();

            // "Территориальный" — отдельно, всегда первым
            cmbPsg.Items.Add("Территориальный");
            _psgNameToId["Территориальный"] = 11;

            // 18 районных ПСГ — parent = 11, isitog = 1
            using var cmd = new MySqlCommand(@"
                SELECT id, name
                FROM psgstat
                WHERE used = 1 AND isitog = 1 AND parent = 11 and garntype='всего'
                ORDER BY norder", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);

                cmbPsg.Items.Add(name);
                _psgNameToId[name] = id;
            }
        }

        private void ShowView(string psgName)
        {
            if (!_psgNameToId.TryGetValue(psgName, out var psgId))
            {
                PivotRowGrid.DataSource = new List<PivotRow>();
                return;
            }

            List<PivotRow> rows;

            if (psgId == 11)
                rows = PivotRowDisplayBuilder.BuildTerritorialView(_allPivotRows);
            else
                rows = PivotRowDisplayBuilder.BuildPsgView(_allPivotRows, psgId);

            PivotRowGrid.DataSource = rows;
            HighlightDatafilledRows();
            UpdateStatus($"{psgName}: {rows.Count} строк");
        }


        private int GetPsgIdByName(string name)
        {
            // быстрый ADO.NET-запрос
            using var conn = new MySqlConnection(DbConfig.BuildConnectionString());
            conn.Open();
            using var cmd = new MySqlCommand(
                "SELECT id FROM psgstat WHERE name = @n AND used = 1 LIMIT 1", conn);
            cmd.Parameters.AddWithValue("@n", name.Trim());
            var obj = cmd.ExecuteScalar();
            return obj == null ? -1 : Convert.ToInt32(obj);
        }

  

        private async void CmbPsg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPsg.SelectedItem == null) return;
            rootPsgName = cmbPsg.SelectedItem.ToString();
            Settings.Default.rootGarn = rootPsgName;
            Settings.Default.Save();
            ShowView(rootPsgName);
        }
        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    var sw = Stopwatch.StartNew();
        //    Mark("Start");

        //    // Временно всегда Территориальный для отладки
        //    rootPsgName = "Территориальный";
        //    Settings.Default.rootGarn = rootPsgName;
        //    Settings.Default.Save();

        //    //for (int i = 0; i < 3; i++)
        //    //{
        //    //    var t = Stopwatch.StartNew();
        //    //    FastPivotLoader.LoadTerritorialFast();
        //    //    Log.Write($"[Fast] call{i + 1}: {t.ElapsedMilliseconds} ms");
        //    //}
        //    //return;

        //    var allRows = FastPivotLoader.LoadTerritorialFast();
        //    Mark($"FastPivotLoader ({allRows.Count})");

        //    var displayRows = PivotRowDisplayBuilder.BuildTerritorialView(allRows);
        //    Mark($"BuildTerritorialView ({displayRows.Count})");

        //    PivotRowGrid.DataSource = displayRows;
        //    Mark("Grid bound");
        //    //var sw = Stopwatch.StartNew();
        //    void Mark(string stage)
        //    {
        //        Log.Mark(stage, sw.ElapsedMilliseconds, 0);
        //        UpdateStatus($"{stage} ({sw.ElapsedMilliseconds} ms)");
        //    }

        //    //Mark("Start");

        //    //var allRows = FastPivotLoader.LoadTerritorialFast();
        //    //Mark($"FastPivotLoader ({allRows.Count} rows)");

        //    //var displayRows = PivotRowDisplayBuilder.BuildTerritorialView(allRows);
        //    //Mark($"BuildTerritorialView ({displayRows.Count} rows)");

        //    //PivotRowGrid.DataSource = displayRows;
        //    //Mark("Grid bound");
        //}

        private async Task BuildTreeAsync(string psgName)
        {
            var rows = await _treeBuilder.GeneratePivotRowsAsync(psgName, forceReload: true);
            PivotRowGrid.DataSource = rows;

            HighlightDatafilledRows();
        }

        // Загрузка списка ПСГ — без побочных эффектов
        private void LoadPsgListУдалить()
        {
            //try
            //{
                var psgNames = allPsgs.Select(p => p.Garnizon).ToList();
                cmbPsg.Items.Clear();
                foreach (var name in psgNames)
                    cmbPsg.Items.Add(name);

                // Временно снимаем обработчик, чтобы не дёргать CmbPsg_SelectedIndexChanged
                cmbPsg.SelectedIndexChanged -= CmbPsg_SelectedIndexChanged;
                int idx = cmbPsg.FindStringExact(rootPsgName);
                cmbPsg.SelectedIndex = idx >= 0 ? idx : 0;
                cmbPsg.SelectedIndexChanged += CmbPsg_SelectedIndexChanged;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка загрузки списка ПСГ: {ex.Message}", "Ошибка",
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        #endregion

        #region Обработка событий от ComboBox
        //private async void CmbPsg_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cmbPsg.SelectedItem == null) return;

        //    rootPsgName = cmbPsg.SelectedItem.ToString();
        //    Settings.Default.rootGarn = rootPsgName;
        //    Settings.Default.Save();

        //    rootPsg = FireEquipsPivotRepository.GetPsgByName2(rootPsgName);
        //    rootPsg1 = FireEquipsPivotRepository.PsgByName(rootPsgName);

        //    UpdateStatus($"Загрузка «{rootPsgName}»...");

        //    try
        //    {
        //        await BuildTreeAsync(rootPsgName);
        //        UpdateStatus($"Выбран гарнизон: {rootPsgName}");
        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateStatus($"Ошибка: {ex.Message}");
        //    }
        //}
        #endregion

        #region Управление левой панелью
        private void BtnTools_Click(object sender, EventArgs e)
        {
            ToggleLeftPanel();
        }

        private void ToggleLeftPanel()
        {
            //isLeftPanelVisible = !isLeftPanelVisible;
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;

            btnTools.Text = isLeftPanelVisible ? "Скрыть" : "Инструменты";

            //////if (isLeftPanelVisible)
            //////{
            //////    splitContainer1.SplitterDistance = 200;
            //////}
        }
        #endregion

        #region Процедуры работы с гридом

        void InitPivotGrid(string rootName)
        {
            var lst = PivotTreeBuilder.GetPsgChildes(rootName).OrderBy(c => c.Norder).ToList();
            if (lst == null)
                return;
            PivotRowGrid.DataSource = lst;
        }

        // refreshGrid — обновляет оба грида
        private async void refreshGrid(string psgName)
        {
            if (string.IsNullOrEmpty(psgName)) return;



            PivotTreeBuilder.InvalidateCache(psgName);
            //await InitPivotGridAsync(psgName);

            HighlightDatafilledRows();
        }
        // InitPivotGrid теперь не нужен — заменён на BuildTreeAsync.
        // Если используется в других местах — оставьте как обёртку:
        private async Task InitPivotGridAsync(string rootName)
        {
            var rows = await _treeBuilder.GeneratePivotRowsAsync(rootName);
            PivotRowGrid.DataSource = rows;
        }

        private void HighlightDatafilledRows()
        {
            foreach (DataGridViewRow row in PivotRowGrid.Rows)
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
 


        #endregion

        #region Обработка кнопок и инструментов
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (rootPsg != null && rootPsg.Garnizon.Contains("Территориал"))
                    cppsReport.myReport(PivotRowGrid);
                else
                    psgReport.printLocal(rootPsgName, PivotRowGrid);

                UpdateStatus("Печать выполнена");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при печати: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        async private void ListBoxTools_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTools.SelectedItem == null) return;

            string selectedItem = listBoxTools.SelectedItem.ToString();

            if (selectedItem == "Обновить")
            {
                await RecalculatePivotRowsAsync();
            }
            return;
            #region Остальные варианты
            //            JSON save
            //JSON load
            //Контроль данных
            //TreeBuilder
            //Сравнение с БД
            //Сравнение всех
            if (selectedItem == "Сравнение с БД")
            {
                BtnCompare_Click(sender, e);
                listBoxTools.SelectedIndex = -1;
            }
            else if (selectedItem == "JSON save")
                SaveCurrentPchData();
            else if (selectedItem == "JSON load")
            {
                LoadCurrentPchData();
            }
            else if (selectedItem == "Сравнение всех")
                compareAllPsg();
            else if (selectedItem == "TreeBuilder") { 
                 await BuildTreeAsync();
            }
            #endregion
        }
        private async Task RecalculatePivotRowsAsync()
        {
            var confirm = MessageBox.Show(
                "Пересчитать pivot_rows для всех ПСГ?\n" +
                "Это может занять несколько минут.",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            //_refreshTimer?.Stop();
            Cursor = Cursors.WaitCursor;

            try
            {
                var sw = Stopwatch.StartNew();
                int totalSaved = 0;
                var errors = new List<string>();

                string psgName = "Территориальный";

                AppStatusService statusApp = new AppStatusService("","",new List<string>() { @"D:\"});
                string basePath = @"D:\";
                JsonDataService service = new JsonDataService(basePath);
                _treeBuilder = new PivotTreeBuilder(context,statusApp, service);
                var rows = await _treeBuilder.GeneratePivotRowsAsync(psgName, forceReload: true);
                var rezult =  PivotRowsRepository.SavePivotRows(rows);

                sw.Stop();
                Cursor = Cursors.Default;

                string msg = $"Пересчёт завершён за {sw.Elapsed.TotalSeconds:F1} сек.\n" +
                             $"Сохранено строк: {rezult.Updated}";

                //if (rezult.errors.Count > 0)
                //{
                //    msg += $"\n\nОшибок: {errors.Count}\n" +
                //           string.Join("\n", errors.Take(5));
                //}

                MessageBox.Show(msg, "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Перечитываем и обновляем грид
                _allPivotRows = FastPivotLoader.LoadAll();
                ShowView(rootPsgName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка пересчёта: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                //_refreshTimer?.Start();
            }
        }
        private async void SaveCurrentPchData() {

            currentPchId = (int)rootPsg1.PsgId;
            var data = CollectCurrentData();
            _syncManager = new DataSyncManager(context, jsonService, currentPchId);
            await _syncManager.SaveDataAsync(data);
            MessageBox.Show("Данные сохранены.");
        }
        private async void LoadCurrentPchData()
        {
         //   var data = CollectCurrentData();
         //   await _syncManager.SaveDataAsync(data);
            MessageBox.Show("Данные сохранены.");
        }

        //Строим дерево узлов и дерево PivotRows
        async private Task BuildTreeAsync() {
            //PivotTreeBuilder b = new PivotTreeBuilder();
            //Models.ReportNode  root = await b.BuildTree();
            //pivotSource  = await b.GeneratePivotRows(root);        
        }
        int currentPchId = 1;
        private PchData CollectCurrentData()
        {
            // Собрать данные из всех редакторов
            currentPchId = (int)rootPsg1.PsgId;

            var data = new PchData { PchId = currentPchId };//

            if(currentPchId!=11)
                data.SredstvaList = FireEquipsPivotRepository.LoadSredstva(currentPchId);// .GetData();
            else { 
                data.SredstvaList = FireEquipsPivotRepository.LoadAllSredstva();// .GetData();
                data.SostavList = FireEquipsPivotRepository.LoadAllSostav();// .GetData();
                data.ContactsList = FireEquipsPivotRepository.LoadAllcontacts();// .GetData();
                data.WatersList = FireEquipsPivotRepository.LoadAllWaters();// .GetData();
                data.PenasList = FireEquipsPivotRepository.LoadAllpenas();// .GetData();
                data.KostymsList = FireEquipsPivotRepository.LoadAllKostyms();// .GetData();
                data.SizodsList = FireEquipsPivotRepository.LoadAllSizods();// .GetData();

            }
            return data;
        }

        private void BtnCompare_Click(object sender, EventArgs e)
        {
            string rezStr = "";
            try
            {
                UpdateStatus("Выполняется сравнение с БД...");
                string psgName = cmbPsg.Text.Trim();
                rezStr = bdService.psgdataCompare(psgName, PivotRowGrid);

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
                //System.Threading.Thread.Sleep(3000);

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
            var excelData = reader.ReadExcelFile(excelFilePath, psg, PivotRowGrid.Rows.Count);

            // Сравниваем с гридом
            var comparer = new GridComparer(PivotRowGrid);
            var results = comparer.CompareAll(excelData);

            return results;
        }



        private string BuildTooltipText(List<DetailItem> details, string columnName)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Составляющие для колонки «{columnName}»:");
            sb.AppendLine("---------------------------");
            foreach (var d in details.OrderByDescending(d => d.Value))
            {
                sb.AppendLine($"{d.Name}: {d.Value:F0}");
            }
            sb.AppendLine("---------------------------");
            sb.Append($"ИТОГО: {details.Sum(d => d.Value):F0}");
            return sb.ToString();
        }

        private void PivotRowGrid_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var grid = sender as DataGridView;
            var row = grid.Rows[e.RowIndex];

            // Проверяем, что строка привязана к объекту PivotRow
            if (row.DataBoundItem is PivotRow pivotRow)
            {
                // Получаем имя свойства, связанного с этой колонкой
                var column = grid.Columns[e.ColumnIndex];
                string columnName = column.DataPropertyName;
                if (string.IsNullOrEmpty(columnName))
                    columnName = column.Name; // запасной вариант

                // Пытаемся получить детали для этой колонки
                if (pivotRow.CellDetails.TryGetValue(columnName, out var details) && details.Any())
                {
                    // Формируем текст подсказки
                    e.ToolTipText = BuildTooltipText(details, columnName);
                }
                else
                {
                    // Если деталей нет — показываем стандартную подсказку (или ничего)
                    e.ToolTipText = null; // не показывать дополнительную подсказку
                }
            }
        }

        private void PivotRowGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
                    PivotRowGrid.Columns[e.ColumnIndex].HeaderText,
                    e.CellStyle.Font,
                    Brushes.Black,
                    rect,
                    format);

                e.Graphics.ResetTransform();
                e.Handled = true;
            }
        }

        private void PivotRowGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var checkBoxCell = PivotRowGrid.Rows[e.RowIndex].Cells["Datafilled"] as DataGridViewCheckBoxCell;
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
                        PivotRowGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                        PivotRowGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        PivotRowGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        PivotRowGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                catch
                {
                    PivotRowGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    PivotRowGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void PivotRowGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (PivotRowGrid.CurrentCell is DataGridViewCheckBoxCell)
            {
                PivotRowGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void PivotRowGrid_DoubleClick(object sender, EventArgs e)
        {
            if (PivotRowGrid.CurrentRow?.DataBoundItem is not PivotRow row) return;

            selectedItem1 = (PivotRow)PivotRowGrid.CurrentRow.DataBoundItem;
            #region Если строка не итоговая, то открываем(обновляем) редактор ПЧ 
            if (row.Isitog != 1)//если строка итогов = и это районный ПСГ, то изменить выбор в combobox
            {
                OpenPivotEditor((int)selectedItem1.Id);
                return;
            }
            #endregion
            #region По двойному клику на ПСГ - выбрать его вместо ТПСГ
            var str = row.Псг;
            if (!string.IsNullOrEmpty(str) && cmbPsg.Items.Contains(str))
                cmbPsg.Text = str;
            return;                   
            #endregion                              
            
        }

        private void ЛСtoolStrip_Click(object sender, EventArgs e)
        {
            _columnManager.ApplyGroup("ЛичныйСостав");
        }

        private void othertoolStrip_Click(object sender, EventArgs e)
        {
            _columnManager.ApplyGroup("ДополнительныйСписок");            
        }

        private void BrtoolStripButton_Click(object sender, EventArgs e)
        {
            _columnManager.ApplyGroup("БоевойРасчёт");
        }

        private void choosColumnsStripButton_Click(object sender, EventArgs e)
        {
            ShowColumnSelector();
        }
        // Метод для вызова диалога выбора колонок
        private void ShowColumnSelector()
        {
            var currentVisible = _columnManager.GetVisibleColumns();
            using (var dlg = new ColumnSelectorDialog(ColumnGroups.ВсеКолонки, currentVisible))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _columnManager.ApplyVisibility(dlg.SelectedColumns);
                }
            }
        }

        #region обновление данных
        private void RefreshData(bool showStatus = true)
        {
            if (_isRefreshing) return;
            _isRefreshing = true;
            var sw = Stopwatch.StartNew();

            try
            {
                // Запомним выбранный ПСГ и текущий скролл
                string currentPsg = rootPsgName;
                int? firstVisibleRow = PivotRowGrid.Rows.Count > 0
                    ? PivotRowGrid.FirstDisplayedScrollingRowIndex
                    : (int?)null;

                // Перечитываем все строки pivot_rows
                _allPivotRows = FastPivotLoader.LoadAll();
                var loadMs = sw.ElapsedMilliseconds;

                // Обновляем отображение (без пересоздания cmbPsg — просто применяем)
                ShowView(currentPsg);
                var totalMs = sw.ElapsedMilliseconds;

                // Восстанавливаем скролл
                if (firstVisibleRow.HasValue && firstVisibleRow.Value < PivotRowGrid.Rows.Count)
                    PivotRowGrid.FirstDisplayedScrollingRowIndex = firstVisibleRow.Value;

                if (showStatus)
                    UpdateStatus($"Обновлено за {totalMs} ms (загрузка {loadMs} ms, строк {_allPivotRows.Count})");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Ошибка обновления: {ex.Message}");
                Log.Write($"[Refresh] error: {ex}");
            }
            finally
            {
                _isRefreshing = false;
            }
        }
        #endregion
        private void OpenPivotEditor(int pchId)
        {
            if (_pivotEditor == null || _pivotEditor.IsDisposed)
            {
                _pivotEditor = new PivotRowEditor(pchId);
                _pivotEditor.FormClosed += PivotEditor_FormClosed;
                _pivotEditor.DataChanged += PivotEditor_DataChanged;
                _pivotEditor.Show(this);           // <— не ShowDialog, не using
            }
            else
            {
                _pivotEditor.LoadPch(pchId);       // просто перезагрузить данные
                if (!_pivotEditor.Visible) _pivotEditor.Show(this);
                _pivotEditor.BringToFront();
            }
            //if (editorForm == null)
            //{
            //    editorForm = new PivotRowEditor(subdiv_Id);
            //    editorForm.DataChanged += OnDataChanged;
            //    editorForm.Show();
            //    editorForm.BringToFront();
            //}
            //else {
           // _pivotEditor.setSubdivId(subdiv_Id);// просто обновляем данные в редакторе
            //}
        }
        private void PivotEditor_DataChanged(object sender, EventArgs e) {
            // тут ваш пересчёт: InvalidateAllCache ? RecalculatePivotRowsAsync ? ShowView
            PivotTreeBuilder.InvalidateAllCache();
            _ = RecalculatePivotRowsAsync();     // осторожно с await в event — см. ниже
        }
        
        private void PivotEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            _pivotEditor.DataChanged -= PivotEditor_DataChanged;
            base.OnFormClosed(e);
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           // Пока неясно нужно ли - это более простой путь вроде как
            //base.OnFormClosed(e);
        }

        private void PivotRowGrid_SelectionChanged(object sender, EventArgs e)
        {

            if (_pivotEditor == null || _pivotEditor.IsDisposed) return;
            if (PivotRowGrid.CurrentRow?.DataBoundItem is not PivotRow row) return;
            if (row.Isitog == 1) return;              // не переключаемся на итоги

            _pivotEditor.LoadPch((int)row.Id);
        }
    }
}
