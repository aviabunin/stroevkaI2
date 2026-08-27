// stroevkaI/Forms/EditorsForm.cs
using System;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;

namespace stroevkaI.Forms
{
    public partial class EditorsForm : Form
    {
        private int subdivisionId;
        private FirePsgStat currentPch;
        private SredstvaEditor sredstvaEditor;
        private ContactsEditor contactsEditor;
        private PersonalsEditor personalsEditor;
        private SostavEditor sostavEditor;
        private CombinedResourcesEditor combinedResourcesEditor;


        // Новые редакторы
        private WatersEditor watersEditor;
        private PenasEditor penasEditor;
        private SizodsEditor sizodsEditor;
        private KostymsEditor kostymsEditor;

        private stroevkaContext _context;
        private SostavRepository _sostavRepository;

        public EditorsForm()
        {
            InitializeComponent();
            _context = new stroevkaContext();
            _sostavRepository = new SostavRepository(_context);
        }

        // Конструктор с FirePsgStat
        public EditorsForm(FirePsgStat _pch) : this()
        {
            currentPch = _pch;
            if (currentPch != null && currentPch.PchId.HasValue)
            {
                subdivisionId = (int)currentPch.PchId.Value;
            }

            InitializeEditors();
        }

        // Конструктор с subdivisionId
        public EditorsForm(int _subdivisionId) : this()
        {
            subdivisionId = _subdivisionId;
            currentPch = FireEquipsPivotRepository.getPchById(subdivisionId);

            InitializeEditors();
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

        private void InitializeEditors()
        {
            InitializeContactsEditor();
            InitializeSredstvaEditor();
            InitializePersonalsEditor();
            InitializeSostavEditor();
            InitializeWatersEditor();
            InitializePenasEditor();
            InitializeSizodsEditor();
            InitializeKostymsEditor();
            InitializeResourcesTab(); // Новая вкладка со всеми редакторами


        }

        // Новый метод - создаём вкладку с таблицей 2×2
        private void InitializeResourcesTab()
        {
            if (currentPch == null) return;

            int pchId = currentPch.PchId.HasValue ? (int)currentPch.PchId.Value : 0;

            // Создаём вкладку "Ресурсы"
            TabPage tabResources = null;
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "Ресурсы")
                {
                    tabResources = tab;
                    break;
                }
            }

            if (tabResources == null)
            {
                tabResources = new TabPage("Ресурсы");
                tabControl1.TabPages.Add(tabResources);
            }

            // Создаём TableLayoutPanel 2×2
            var tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = SystemColors.Control
            };
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // Создаём каждого редактора
            watersEditor = new WatersEditor(pchId);
            watersEditor.Dock = DockStyle.Fill;
            watersEditor.BorderStyle = BorderStyle.FixedSingle;

            penasEditor = new PenasEditor(pchId);
            penasEditor.Dock = DockStyle.Fill;
            penasEditor.BorderStyle = BorderStyle.FixedSingle;

            sizodsEditor = new SizodsEditor(pchId);
            sizodsEditor.Dock = DockStyle.Fill;
            sizodsEditor.BorderStyle = BorderStyle.FixedSingle;

            kostymsEditor = new KostymsEditor(pchId);
            kostymsEditor.Dock = DockStyle.Fill;
            kostymsEditor.BorderStyle = BorderStyle.FixedSingle;

            // Добавляем в таблицу
            tableLayout.Controls.Add(watersEditor, 0, 0);
            tableLayout.Controls.Add(penasEditor, 1, 0);
            tableLayout.Controls.Add(sizodsEditor, 0, 1);
            tableLayout.Controls.Add(kostymsEditor, 1, 1);

            // Очищаем вкладку и добавляем таблицу
            tabResources.Controls.Clear();
            tabResources.Controls.Add(tableLayout);

            // Подписываемся на события сохранения
            watersEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (вода сохранена)";
            };
            penasEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (пена сохранена)";
            };
            sizodsEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (СИЗОД сохранён)";
            };
            kostymsEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (костюмы сохранены)";
            };
        }

        private void InitializeCombinedResourcesEditor()
        {
            if (currentPch == null) return;

            int pchId = currentPch.PchId.HasValue ? (int)currentPch.PchId.Value : 0;

            combinedResourcesEditor = new CombinedResourcesEditor(pchId);
            combinedResourcesEditor.Dock = DockStyle.Fill;

            combinedResourcesEditor.DataChanged += (s, e) => {
                this.Text = currentPch?.Пч + " (ресурсы изменены)";
            };

            combinedResourcesEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (ресурсы сохранены)";
            };

            // Добавляем на вкладку "Ресурсы"
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "Ресурсы")
                {
                    tab.Controls.Clear();
                    tab.Controls.Add(combinedResourcesEditor);
                    break;
                }
            }
        }
        private void InitializeContactsEditor()
        {
            if (currentPch == null) return;

            var baseDate = new DateTime(2018, 07, 31);
            int currentKaraul = ((DateTime.Now.AddHours(-8).Date - baseDate).Days) % 4 + 1;

            contactsEditor = new ContactsEditor(currentPch, currentKaraul);
            contactsEditor.Dock = DockStyle.Fill;

            contactsEditor.DataChanged += (s, e) => {
                this.Text = currentPch?.Пч + " (контакты изменены)";
            };

            contactsEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (сохранено)";
            };

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "Контакты")
                {
                    tab.Controls.Clear();
                    tab.Controls.Add(contactsEditor);
                    break;
                }
            }
        }

        private void InitializeSredstvaEditor()
        {
            if (currentPch != null)
            {
                sredstvaEditor = new SredstvaEditor(currentPch);
            }
            else if (subdivisionId > 0)
            {
                sredstvaEditor = new SredstvaEditor(subdivisionId);
            }
            else
            {
                sredstvaEditor = new SredstvaEditor();
            }

            sredstvaEditor.Dock = DockStyle.Fill;

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "Средства")
                {
                    tab.Controls.Clear();
                    tab.Controls.Add(sredstvaEditor);
                    break;
                }
            }

            sredstvaEditor.DataChanged += (s, e) => {
                this.Text = currentPch?.Пч + " (изменено)";
            };
        }

        private void InitializePersonalsEditor()
        {
            if (currentPch == null) return;

            personalsEditor = new PersonalsEditor(
                currentPch.PchId.HasValue ? (int)currentPch.PchId.Value : 0
            );
            personalsEditor.Dock = DockStyle.Fill;

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "Сотрудники")
                {
                    tab.Controls.Clear();
                    tab.Controls.Add(personalsEditor);
                    break;
                }
            }
        }

        private void InitializeSostavEditor()
        {
            if (currentPch == null) return;
            if (_sostavRepository == null) return;

            int pchId = currentPch.PchId.HasValue ? (int)currentPch.PchId.Value : 0;

            sostavEditor = new SostavEditor(pchId);
            sostavEditor.Dock = DockStyle.Fill;

            sostavEditor.DataChanged += (s, e) => {
                this.Text = currentPch?.Пч + " (состав изменён)";
            };

            sostavEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (состав сохранён)";
            };

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "Состав")
                {
                    tab.Controls.Clear();
                    tab.Controls.Add(sostavEditor);
                    break;
                }
            }
        }

        private void InitializeWatersEditor()
        {
            if (currentPch == null) return;

            int pchId = currentPch.PchId.HasValue ? (int)currentPch.PchId.Value : 0;

            // Создаём редактор для воды
            watersEditor = new WatersEditor(pchId);
            watersEditor.Dock = DockStyle.Fill;

            watersEditor.DataChanged += (s, e) => {
                this.Text = currentPch?.Пч + " (вода изменена)";
            };

            watersEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (вода сохранена)";
            };

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "Вода")
                {
                    tab.Controls.Clear();
                    tab.Controls.Add(watersEditor);
                    break;
                }
            }
        }

        private void InitializePenasEditor()
        {
            if (currentPch == null) return;

            int pchId = currentPch.PchId.HasValue ? (int)currentPch.PchId.Value : 0;

            penasEditor = new PenasEditor(pchId);
            penasEditor.Dock = DockStyle.Fill;

            penasEditor.DataChanged += (s, e) => {
                this.Text = currentPch?.Пч + " (пена изменена)";
            };

            penasEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (пена сохранена)";
            };

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "Пена")
                {
                    tab.Controls.Clear();
                    tab.Controls.Add(penasEditor);
                    break;
                }
            }
        }

        private void InitializeSizodsEditor()
        {
            if (currentPch == null) return;

            int pchId = currentPch.PchId.HasValue ? (int)currentPch.PchId.Value : 0;

            sizodsEditor = new SizodsEditor(pchId);
            sizodsEditor.Dock = DockStyle.Fill;

            sizodsEditor.DataChanged += (s, e) => {
                this.Text = currentPch?.Пч + " (СИЗОД изменён)";
            };

            sizodsEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (СИЗОД сохранён)";
            };

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "СИЗОД")
                {
                    tab.Controls.Clear();
                    tab.Controls.Add(sizodsEditor);
                    break;
                }
            }
        }

        private void InitializeKostymsEditor()
        {
            if (currentPch == null) return;

            int pchId = currentPch.PchId.HasValue ? (int)currentPch.PchId.Value : 0;

            kostymsEditor = new KostymsEditor(pchId);
            kostymsEditor.Dock = DockStyle.Fill;

            kostymsEditor.DataChanged += (s, e) => {
                this.Text = currentPch?.Пч + " (костюмы изменены)";
            };

            kostymsEditor.SaveRequested += (s, e) => {
                this.Text = currentPch?.Пч + " (костюмы сохранены)";
            };

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "Костюмы")
                {
                    tab.Controls.Clear();
                    tab.Controls.Add(kostymsEditor);
                    break;
                }
            }
        }

        private void EditorsForm_Load(object sender, EventArgs e)
        {
            if (sredstvaEditor == null)
            {
                InitializeEditors();
            }
        }

        // Обновляем RefreshEditors
        public void RefreshEditors(FirePsgStat pch)
        {
            currentPch = pch;
            if (pch != null && pch.PchId.HasValue)
            {
                subdivisionId = (int)pch.PchId.Value;
            }

            sostavEditor?.LoadData();
            watersEditor?.LoadData();
            penasEditor?.LoadData();
            sizodsEditor?.LoadData();
            kostymsEditor?.LoadData();
        }

        public void RefreshEditors(int pchId)
        {
            subdivisionId = pchId;
            var pch = FireEquipsPivotRepository.getPchById(pchId);
            if (pch != null)
            {
                RefreshEditors(pch);
            }
        }
    }
}