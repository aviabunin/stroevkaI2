using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;

namespace stroevkaI.Controls
{
    public partial class PsgListViewControl : UserControl
    {
        // События
        public event EventHandler<PsgSelectedEventArgs> PsgSelected;
        public event EventHandler<PchSelectedEventArgs> PchSelected;
        public event EventHandler<PanelSizeRequestEventArgs> PanelSizeRequest;

        private bool showFullNames = false;
        private List<FirePsgStat> allPsgs;
        private List<FirePsgStat> psgList; // Список ПСГ для отображения
        private string currentRootPsg = "";
        private string selectedPsg = "";
        private bool isInPsgMode = false;
        private const long TERRITORIAL_PCH_ID = 11; // PchId территориального ПСГ

        // Класс для хранения данных элемента ListBox
        private class ListBoxItem
        {
            public FirePsgStat PsgData { get; set; }
            public string DisplayText { get; set; }
            public string PsgName { get; set; }
            public int? PchId { get; set; }
            public bool IsPsg { get; set; }
            public bool IsPch { get; set; }
            public bool IsRoot { get; set; }
            public int Level { get; set; }
            public bool IsItog { get; set; }
        }

        // Конструктор без параметров (для дизайнера)
        public PsgListViewControl()
        {
            InitializeComponent();
        }

        // Основной конструктор с параметром rootGarn
        public PsgListViewControl(string rootGarn) : this()
        {
            LoadData();

            // Загружаем текущий гарнизон
            if (string.IsNullOrEmpty(rootGarn))
            {
                rootGarn = "Территориальный";
            }

            currentRootPsg = rootGarn;
            selectedPsg = rootGarn;

            // Сохраняем в Settings
            Properties.Settings.Default.rootGarn = rootGarn;
            Properties.Settings.Default.Save();

            BuildList();
        }

        private void LoadData()
        {
            try
            {
                allPsgs = FireEquipsPivotRepository.LoadAllPsgs();
                if (allPsgs == null)
                {
                    allPsgs = new List<FirePsgStat>();
                }

                // Формируем список ПСГ для отображения
                BuildPsgList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                allPsgs = new List<FirePsgStat>();
                psgList = new List<FirePsgStat>();
            }
        }

        private void BuildPsgList()
        {
            psgList = new List<FirePsgStat>();

            // Находим территориальный ПСГ (PchId = 11)
            var territorialPsg = allPsgs.FirstOrDefault(p => p.PchId == TERRITORIAL_PCH_ID );
            if (territorialPsg != null)
            {
                psgList.Add(territorialPsg);
            }

            // Находим районные ПСГ (Parent = 11 и Isitog != 1)
            var districtPsgs = allPsgs
                .Where(p => p.Parent == TERRITORIAL_PCH_ID )
                .OrderBy(p => p.Псг)
                .ToList();

            psgList.AddRange(districtPsgs);
        }

        private List<FirePsgStat> GetPchsForPsg(string psgName)
        {
            // Находим ПСГ по имени
            var psg = allPsgs.FirstOrDefault(p => p.Псг == psgName && p.Isitog != 1);
            if (psg == null) return new List<FirePsgStat>();

            // Если это территориальный ПСГ - возвращаем все районные ПСГ
            if (psg.PchId == TERRITORIAL_PCH_ID)
            {
                return allPsgs
                    .Where(p => p.Parent == TERRITORIAL_PCH_ID && p.Isitog != 1)
                    .OrderBy(p => p.Псг)
                    .ToList();
            }

            // Для районного ПСГ - возвращаем его самого и подчинённые ПЧ
            var result = new List<FirePsgStat>();

            // Добавляем сам ПСГ (не итоговый)
            var psgItem = allPsgs.FirstOrDefault(p => p.Псг == psgName && p.PchId == null && p.Isitog != 1);
            if (psgItem != null)
            {
                result.Add(psgItem);
            }

            // Добавляем ПЧ (Parent = PchId ПСГ и Isitog != 1)
            var pchs = allPsgs
                .Where(p => p.Parent == psg.PchId && p.PchId.HasValue && p.Isitog != 1)
                .OrderBy(p => p.Пч)
                .ToList();

            result.AddRange(pchs);

            return result;
        }

        private string GetShortName(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;

            var shortNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Территориальный", "Тер" },
                { "Петрозаводский", "Птр" },
                { "Прионежский", "При" },
                { "Беломорский", "Бел" },
                { "Костомукшский", "Кст" },
                { "Калевальский", "Кал" },
                { "Кондопожский", "Кнд" },
                { "Лахденпохский", "Лах" },
                { "Лоухский", "Лоу" },
                { "Медвежьегорский", "Мед" },
                { "Муезерский", "Муе" },
                { "Олонецкий", "Оло" },
                { "Питкярантский", "Пит" },
                { "Пряжинский", "Пря" },
                { "Пудожский", "Пуд" },
                { "Сегежский", "Сег" },
                { "Сортавальский", "Сор" },
                { "Суоярвский", "Суо" },
                { "Кемский", "Кем" }
            };

            if (shortNames.ContainsKey(name))
                return shortNames[name];

            if (name.Length <= 3) return name;

            if (name.EndsWith("ский") || name.EndsWith("ской") || name.EndsWith("цкий"))
                return name.Substring(0, Math.Min(3, name.Length));

            return name.Substring(0, Math.Min(3, name.Length));
        }

        private string GetDisplayName(string name, bool full)
        {
            if (string.IsNullOrEmpty(name)) return name;
            if (full) return name;
            return GetShortName(name);
        }

        public void BuildList()
        {
            listBox.BeginUpdate();
            listBox.Items.Clear();
            ClearInfoPanel();

            if (psgList == null || !psgList.Any())
            {
                listBox.Items.Add("Нет данных");
                listBox.EndUpdate();
                return;
            }

            // Определяем режим отображения
            if (isInPsgMode)
            {
                BuildPsgMode();
            }
            else
            {
                BuildHierarchyList();
            }

            // Выбираем текущий элемент
            SelectCurrentItem();

            listBox.EndUpdate();

            // Уведомляем об изменении ширины
            OnPanelSizeChanged();
        }

        private void BuildHierarchyList()
        {
            // Показываем все ПСГ из списка
            foreach (var psg in psgList)
            {
                bool isRoot = psg.PchId == TERRITORIAL_PCH_ID;
                int level = isRoot ? 0 : 1;

                // Для территориального показываем без отступа, для районных - с отступом
                AddListBoxItem(psg, level);
            }
        }

        private void BuildPsgMode()
        {
            // Получаем данные для выбранного ПСГ
            var items = GetPchsForPsg(selectedPsg);

            if (items == null || !items.Any())
            {
                // Если ПСГ не найден, возвращаемся в иерархический режим
                isInPsgMode = false;
                BuildHierarchyList();
                return;
            }

            // Добавляем ПСГ и его ПЧ
            int level = 0;
            foreach (var item in items)
            {
                bool isPsg = !item.PchId.HasValue || item.Псг == item.Пч;
                AddListBoxItem(item, level, isPsg);
                if (isPsg)
                {
                    level = 1; // ПЧ будут с отступом
                }
            }

            // Показываем кнопку "Назад"
            backButton.Visible = true;
            backButton.Text = "◄ Назад к списку ПСГ";
        }

        private void AddListBoxItem(FirePsgStat psgData, int level, bool isPsg = true)
        {
            if (psgData == null) return;

            string displayText = isPsg ? psgData.Псг : psgData.Пч;
            string indent = new string(' ', level * 2);

            // Для коротких имён используем сокращения
            string displayName;
            if (!showFullNames && isPsg)
            {
                displayName = GetShortName(displayText);
            }
            else
            {
                displayName = displayText;
            }

            string fullText = indent + displayName;

            var item = new ListBoxItem
            {
                PsgData = psgData,
                DisplayText = displayText,
                PsgName = psgData.Псг,
                PchId = psgData.PchId.HasValue ? (int)psgData.PchId.Value : (int?)null,
                IsPsg = isPsg,
                IsPch = !isPsg && psgData.PchId.HasValue,
                IsRoot = psgData.PchId == TERRITORIAL_PCH_ID,
                Level = level,
                IsItog = psgData.Isitog == 1
            };

            int index = listBox.Items.Add(item);

            // Настройка отображения с префиксами
            string prefix = "";
            if (isPsg && psgData.PchId == TERRITORIAL_PCH_ID)
            {
                prefix = "● ";
            }
            else if (isPsg)
            {
                prefix = "► ";
            }
            else if (!isPsg && psgData.PchId.HasValue)
            {
                prefix = "  - ";
            }

            listBox.Items[index] = prefix + fullText;
        }

        private void ClearInfoPanel()
        {
            infoLabel.Text = "Выберите гарнизон";
            infoLabel.ForeColor = Color.DarkGray;
        }

        private void UpdateInfoPanel(ListBoxItem item)
        {
            if (item == null || item.PsgData == null)
            {
                ClearInfoPanel();
                return;
            }

            var psg = item.PsgData;

            if (item.IsPsg && psg.PchId == TERRITORIAL_PCH_ID)
            {
                int totalPch = allPsgs.Count(p => p.Parent == TERRITORIAL_PCH_ID && p.PchId.HasValue && p.Isitog != 1);
                infoLabel.Text = $"🏛️ {psg.Псг} - Главный гарнизон, ПЧ: {totalPch}";
                infoLabel.ForeColor = Color.DarkRed;
            }
            else if (item.IsPsg)
            {
                int pchCount = allPsgs.Count(p => p.Псг == psg.Псг && p.PchId.HasValue && p.Isitog != 1);
                infoLabel.Text = $"🏛️ {psg.Псг} - ПЧ: {pchCount}";
                infoLabel.ForeColor = Color.DarkBlue;
            }
            else if (item.IsPch)
            {
                infoLabel.Text = $"🚒 {psg.Пч} - ПЧ, гарнизон: {psg.Псг}";
                infoLabel.ForeColor = Color.Black;
            }
            else
            {
                ClearInfoPanel();
            }
        }

        private void SelectCurrentItem()
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                var item = listBox.Items[i] as ListBoxItem;
                if (item != null && item.PsgName == selectedPsg && item.IsPsg)
                {
                    listBox.SelectedIndex = i;
                    return;
                }
            }
        }

        private void OnPanelSizeChanged()
        {
            int panelWidth = 280;

            if (!showFullNames)
            {
                panelWidth = 120;
            }

            PanelSizeRequest?.Invoke(this, new PanelSizeRequestEventArgs(panelWidth));
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex < 0) return;

            var item = listBox.Items[listBox.SelectedIndex] as ListBoxItem;
            if (item == null) return;

            UpdateInfoPanel(item);

            if (item.IsPsg)
            {
                selectedPsg = item.PsgName;
                currentRootPsg = item.PsgName;

                // Сохраняем в Settings
                Properties.Settings.Default.rootGarn = item.PsgName;
                Properties.Settings.Default.Save();

                PsgSelected?.Invoke(this, new PsgSelectedEventArgs(item.PsgName));
            }
            else if (item.IsPch && item.PchId.HasValue)
            {
                PchSelected?.Invoke(this, new PchSelectedEventArgs(item.PchId.Value, item.DisplayText));
            }
        }

        private void ListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBox.IndexFromPoint(e.Location);
            if (index < 0) return;

            var item = listBox.Items[index] as ListBoxItem;
            if (item == null) return;

            // Если кликнули по районному ПСГ и не в режиме ПСГ
            if (item.IsPsg && item.PsgName != "Территориальный" && !isInPsgMode)
            {
                isInPsgMode = true;
                selectedPsg = item.PsgName;
                BuildList();
            }
            // Если кликнули по ПЧ
            else if (item.IsPch && item.PchId.HasValue)
            {
                PchSelected?.Invoke(this, new PchSelectedEventArgs(item.PchId.Value, item.DisplayText));
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            isInPsgMode = false;
            backButton.Visible = false;
            selectedPsg = "Территориальный";
            BuildList();
        }

        private void SearchBox_Enter(object sender, EventArgs e)
        {
            if (searchBox.Text == "Поиск...")
                searchBox.Text = "";
        }

        private void SearchBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchBox.Text))
                searchBox.Text = "Поиск...";
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchBox.Text.ToLower();
            if (string.IsNullOrEmpty(searchText) || searchText == "поиск...")
            {
                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    listBox.SelectedIndex = -1;
                }
                return;
            }

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                var item = listBox.Items[i] as ListBoxItem;
                if (item != null && item.DisplayText.ToLower().Contains(searchText))
                {
                    listBox.SelectedIndex = i;
                    return;
                }
            }
        }

        private void ShowFullNamesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            showFullNames = showFullNamesCheckBox.Checked;
            BuildList();
            OnPanelSizeChanged();
        }

        // Публичные методы
        public void SetSelectedPsg(string psgName)
        {
            if (string.IsNullOrEmpty(psgName))
            {
                psgName = "Территориальный";
            }

            currentRootPsg = psgName;
            selectedPsg = psgName;

            // Сохраняем в Settings
            Properties.Settings.Default.rootGarn = psgName;
            Properties.Settings.Default.Save();

            isInPsgMode = false;
            backButton.Visible = false;
            BuildList();
        }

        public void RefreshTree()
        {
            LoadData();
            isInPsgMode = false;
            backButton.Visible = false;

            // Загружаем из Settings
            currentRootPsg = Properties.Settings.Default.rootGarn;
            if (string.IsNullOrEmpty(currentRootPsg))
            {
                currentRootPsg = "Территориальный";
                Properties.Settings.Default.rootGarn = currentRootPsg;
                Properties.Settings.Default.Save();
            }
            selectedPsg = currentRootPsg;

            BuildList();
        }

        public string GetSelectedPsg()
        {
            return currentRootPsg;
        }
    }

    public class PanelSizeRequestEventArgs : EventArgs
    {
        public int PanelWidth { get; private set; }

        public PanelSizeRequestEventArgs(int panelWidth)
        {
            PanelWidth = panelWidth;
        }
    }
}