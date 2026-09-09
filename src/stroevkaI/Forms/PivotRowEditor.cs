// stroevkaI/Forms/PivotRowEditor.cs
using System;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.Repositories;
using stroevkaI.Services;

namespace stroevkaI.Forms
{
    public partial class PivotRowEditor : Form
    {
        private int subdivisionId;
        private PivotRow currentRow;

        private SredstvaEditor sredstvaEditor;//sredstvaEditor, contactsEditor, personalsEditor,sostavEditor,combinedResourcesEditor,watersEditor,penasEditor,sizodsEditor,kostymsEditor
        private ContactsEditor contactsEditor;
        private PersonalsEditor personalsEditor;
        private SostavEditor sostavEditor;
        private CombinedResourcesEditor combinedResourcesEditor;

        private WatersEditor watersEditor;
        private PenasEditor penasEditor;//penasEditor,sizodsEditor,kostymsEditor
        private SizodsEditor sizodsEditor;
        private KostymsEditor kostymsEditor;

        private stroevkaContext _context;
        private SostavRepository _sostavRepository;

        public PivotRowEditor()
        {
            InitializeComponent();
            _context = new stroevkaContext();
            _sostavRepository = new SostavRepository(_context);
        }

        // Конструктор с PivotRow
        public PivotRowEditor(PivotRow row) : this()
        {
            currentRow = row;
            subdivisionId = currentRow.PchId; // теперь просто int
            InitializeEditors();
        }

        // Конструктор с subdivisionId (если нужно загрузить PivotRow по id)
        public PivotRowEditor(int subdivisionId) : this()
        {
            this.subdivisionId = subdivisionId;
            // Здесь можно получить PivotRow из источника, если нужно
            currentRow = GetPivotRowById(subdivisionId);
            InitializeEditors();
        }

        // Заглушка для получения PivotRow по id (реализуйте при необходимости)
        private PivotRow GetPivotRowById(int id)
        {
            // Например, можно запросить из списка или из БД
            // Если не нужно, оставьте возврат null или нового объекта
            return new PivotRow { PchId = id };
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
            InitializeResourcesTab();
        }

        private void InitializeResourcesTab()
        {
            if (currentRow == null) return;
            int pchId = currentRow.PchId;

            TabPage tabResources = null;
            foreach (TabPage tab in tabControl1.TabPages)
                if (tab.Text == "Ресурсы") { tabResources = tab; break; }

            if (tabResources == null)
            {
                tabResources = new TabPage("Ресурсы");
                tabControl1.TabPages.Add(tabResources);
            }

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

            watersEditor = new WatersEditor(pchId) { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };
            penasEditor = new PenasEditor(pchId) { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };
            sizodsEditor = new SizodsEditor(pchId) { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };
            kostymsEditor = new KostymsEditor(pchId) { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };

            tableLayout.Controls.Add(watersEditor, 0, 0);
            tableLayout.Controls.Add(penasEditor, 1, 0);
            tableLayout.Controls.Add(sizodsEditor, 0, 1);
            tableLayout.Controls.Add(kostymsEditor, 1, 1);

            tabResources.Controls.Clear();
            tabResources.Controls.Add(tableLayout);

            watersEditor.SaveRequested += (s, e) => this.Text = currentRow?.ПЧ + " (вода сохранена)";
            penasEditor.SaveRequested += (s, e) => this.Text = currentRow?.ПЧ + " (пена сохранена)";
            sizodsEditor.SaveRequested += (s, e) => this.Text = currentRow?.ПЧ + " (СИЗОД сохранён)";
            kostymsEditor.SaveRequested += (s, e) => this.Text = currentRow?.ПЧ + " (костюмы сохранены)";
        }

        private void InitializeContactsEditor()
        {
            if (currentRow == null) return;
            int pchId = currentRow.PchId;
            var baseDate = new DateTime(2018, 07, 31);
            int currentKaraul = ((DateTime.Now.AddHours(-8).Date - baseDate).Days) % 4 + 1;

            // Если ContactsEditor требует FirePsgStat, создаём фиктивный объект
            var fakePch = new FirePsgStat { PchId = pchId, Пч = currentRow?.ПЧ };
            contactsEditor = new ContactsEditor(fakePch, currentKaraul) { Dock = DockStyle.Fill };

            contactsEditor.DataChanged += (s, e) => this.Text = currentRow?.ПЧ + " (контакты изменены)";
            contactsEditor.SaveRequested += (s, e) => this.Text = currentRow?.ПЧ + " (сохранено)";

            foreach (TabPage tab in tabControl1.TabPages)
                if (tab.Text == "Контакты") { tab.Controls.Clear(); tab.Controls.Add(contactsEditor); break; }
        }

        private void InitializeSredstvaEditor()
        {
            if (currentRow == null)
                return;// Сделать исключение
            sredstvaEditor = new SredstvaEditor(currentRow.PchId);


            sredstvaEditor.Dock = DockStyle.Fill;
            sredstvaEditor.DataChanged += (s, e) => this.Text = currentRow?.ПЧ + " (изменено)";

            foreach (TabPage tab in tabControl1.TabPages)
                if (tab.Text == "Средства") { tab.Controls.Clear(); tab.Controls.Add(sredstvaEditor); break; }
        }

        private void InitializePersonalsEditor()
        {
            if (currentRow == null) return;
            personalsEditor = new PersonalsEditor(currentRow.PchId) { Dock = DockStyle.Fill };

            foreach (TabPage tab in tabControl1.TabPages)
                if (tab.Text == "Сотрудники") { tab.Controls.Clear(); tab.Controls.Add(personalsEditor); break; }
        }

        private void InitializeSostavEditor()
        {
            if (currentRow == null || _sostavRepository == null) return;
            sostavEditor = new SostavEditor(currentRow.PchId) { Dock = DockStyle.Fill };

            sostavEditor.DataChanged += (s, e) => this.Text = currentRow?.ПЧ + " (состав изменён)";
            sostavEditor.SaveRequested += (s, e) => this.Text = currentRow?.ПЧ + " (состав сохранён)";

            foreach (TabPage tab in tabControl1.TabPages)
                if (tab.Text == "Состав") { tab.Controls.Clear(); tab.Controls.Add(sostavEditor); break; }
        }

        private void InitializeWatersEditor()
        {
            if (currentRow == null) return;
            watersEditor = new WatersEditor(currentRow.PchId) { Dock = DockStyle.Fill };

            watersEditor.DataChanged += (s, e) => this.Text = currentRow?.ПЧ + " (вода изменена)";
            watersEditor.SaveRequested += (s, e) => this.Text = currentRow?.ПЧ + " (вода сохранена)";

            foreach (TabPage tab in tabControl1.TabPages)
                if (tab.Text == "Вода") { tab.Controls.Clear(); tab.Controls.Add(watersEditor); break; }
        }

        private void InitializePenasEditor()
        {
            if (currentRow == null) return;
            penasEditor = new PenasEditor(currentRow.PchId) { Dock = DockStyle.Fill };

            penasEditor.DataChanged += (s, e) => this.Text = currentRow?.ПЧ + " (пена изменена)";
            penasEditor.SaveRequested += (s, e) => this.Text = currentRow?.ПЧ + " (пена сохранена)";

            foreach (TabPage tab in tabControl1.TabPages)
                if (tab.Text == "Пена") { tab.Controls.Clear(); tab.Controls.Add(penasEditor); break; }
        }

        private void InitializeSizodsEditor()
        {
            if (currentRow == null) return;
            sizodsEditor = new SizodsEditor(currentRow.PchId) { Dock = DockStyle.Fill };

            sizodsEditor.DataChanged += (s, e) => this.Text = currentRow?.ПЧ + " (СИЗОД изменён)";
            sizodsEditor.SaveRequested += (s, e) => this.Text = currentRow?.ПЧ + " (СИЗОД сохранён)";

            foreach (TabPage tab in tabControl1.TabPages)
                if (tab.Text == "СИЗОД") { tab.Controls.Clear(); tab.Controls.Add(sizodsEditor); break; }
        }

        private void InitializeKostymsEditor()
        {
            if (currentRow == null) return;
            kostymsEditor = new KostymsEditor(currentRow.PchId) { Dock = DockStyle.Fill };

            kostymsEditor.DataChanged += (s, e) => this.Text = currentRow?.ПЧ + " (костюмы изменены)";
            kostymsEditor.SaveRequested += (s, e) => this.Text = currentRow?.ПЧ + " (костюмы сохранены)";

            foreach (TabPage tab in tabControl1.TabPages)
                if (tab.Text == "Костюмы") { tab.Controls.Clear(); tab.Controls.Add(kostymsEditor); break; }
        }

        private void PivotRowEditor_Load(object sender, EventArgs e)
        {
            if (sredstvaEditor == null)
                InitializeEditors();
        }

        public void RefreshEditors(PivotRow row)
        {
            currentRow = row;
            subdivisionId = row?.PchId ?? 0;
            sostavEditor?.LoadData();
            watersEditor?.LoadData();
            penasEditor?.LoadData();
            sizodsEditor?.LoadData();
            kostymsEditor?.LoadData();
        }

        public void RefreshEditors(int pchId)
        {
            subdivisionId = pchId;
            currentRow = new PivotRow { PchId = pchId };
            RefreshEditors(currentRow);
        }
    }
}