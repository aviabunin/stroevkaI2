// stroevkaI/Forms/ContactsEditor.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.ModelsStroevkaMySql.Repositories;

namespace stroevkaI.Forms
{
    public partial class ContactsEditor : UserControl
    {
        private readonly ContactRepository _repository;
        private List<Contact> _currentContacts;

        // --- Кэшированный список всех сотрудников с их должностями ---
        private List<ContactRepository.PersonalWithPosts> _allPersonalsWithPosts;
        private List<ContactRepository.PersonalWithPosts> _filteredPersonals;

        private Contact _selectedContact;
        private int _selectedContactId;  // Вместо _selectedContact
        private int _currentKaraul;
        private int _currentPchId;
        private int _currentSubdivisionId;
        private string _currentSubdivisionName;
        private string _currentGarnizonName;
        private bool _showRightPanel = false;

        public event EventHandler DataChanged;
        public event EventHandler SaveRequested;
        public event EventHandler CancelRequested;

        public ContactsEditor(FirePsgStat pch, int karaul)
        {
            InitializeComponent();
            _repository = new ContactRepository();

            _currentPchId = pch?.PchId.HasValue == true ? (int)pch.PchId.Value : 0;
            _currentSubdivisionId = _currentPchId;
            _currentSubdivisionName = pch?.Пч ?? "";
            _currentGarnizonName = pch?.Псг ?? "";
            _currentKaraul = karaul;

            SetupDataGridViews();
            LoadPostsFilter();

            // --- Загружаем всех сотрудников один раз при создании ---
            LoadAllPersonals();

            LoadContacts();
            ApplyPersonalsFilter(null);

            SetEditMode(false);
            SetRightPanelVisible(false);
        }

        private void SetupDataGridViews()
        {
            // --- Верхний грид - Контакты (только 3 колонки) ---
            dgvContacts.Columns.Clear();
            dgvContacts.Columns.Add("Id", "ID");
            dgvContacts.Columns.Add("Post", "Должность");
            dgvContacts.Columns.Add("Fio", "ФИО");
            dgvContacts.Columns.Add("TfMobil", "Мобильный");

            dgvContacts.Columns["Id"].Visible = false;
            dgvContacts.Columns["Post"].Width = 200;
            dgvContacts.Columns["Fio"].Width = 180;
            dgvContacts.Columns["TfMobil"].Width = 150;

            dgvContacts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // --- Правый грид - Сотрудники ---
            dgvPersonals.Columns.Clear();
            dgvPersonals.Columns.Add("PersonalId", "ID");
            dgvPersonals.Columns.Add("FullName", "ФИО");
            dgvPersonals.Columns.Add("AllowedPosts", "Должности");
            dgvPersonals.Columns.Add("TfMobil", "Мобильный");

            dgvPersonals.Columns["PersonalId"].Width = 40;
            dgvPersonals.Columns["FullName"].Width = 150;
            dgvPersonals.Columns["AllowedPosts"].Width = 200;
            dgvPersonals.Columns["TfMobil"].Width = 100;

            dgvPersonals.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }

        private void LoadPostsFilter()
        {
            try
            {
                var posts = _repository.LoadPosts();
                cmbPostFilter.Items.Clear();
                cmbPostFilter.Items.Add("Все");
                foreach (var post in posts)
                {
                    cmbPostFilter.Items.Add(post.Name);
                }
                cmbPostFilter.SelectedIndex = 0;
            }
            catch
            {
                cmbPostFilter.Items.Clear();
                cmbPostFilter.Items.Add("Все");
                cmbPostFilter.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Загрузка всех сотрудников ПЧ с их должностями (кэширование)
        /// </summary>
        private void LoadAllPersonals()
        {
            try
            {
                _allPersonalsWithPosts = _repository.LoadPersonalsWithPosts(_currentSubdivisionId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки сотрудников: {ex.Message}");
                _allPersonalsWithPosts = new List<ContactRepository.PersonalWithPosts>();
            }
        }

        /// <summary>
        /// Применение фильтра к кэшированному списку сотрудников
        /// </summary>
        private void ApplyPersonalsFilter(string postFilter)
        {
            if (_allPersonalsWithPosts == null)
            {
                _filteredPersonals = new List<ContactRepository.PersonalWithPosts>();
                RefreshPersonalsGrid();
                return;
            }

            if (string.IsNullOrEmpty(postFilter) || postFilter == "Все")
            {
                _filteredPersonals = _allPersonalsWithPosts;
            }
            else
            {
                // Фильтруем по должности
                _filteredPersonals = _allPersonalsWithPosts
                    .Where(p => p.AllowedPosts.Any(pp => pp.Name == postFilter))
                    .ToList();
            }

            RefreshPersonalsGrid();
        }

        private void LoadContacts()
        {
            try
            {
                _currentContacts = _repository.LoadContacts(_currentSubdivisionId, _currentKaraul);
                RefreshContactsGrid();
                lblTitle.Text = $"Контакты караула {_currentKaraul} ({_currentContacts.Count} записей)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки контактов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshContactsGrid()
        {
            // Сохраняем ID выбранного контакта (если есть)
            int? selectedId = null;
            if (dgvContacts.SelectedRows.Count > 0)
            {
                var selectedRow = dgvContacts.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value != null)
                {
                    selectedId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                }
            }

            dgvContacts.Rows.Clear();

            if (_currentContacts == null || _currentContacts.Count == 0)
                return;

            foreach (var contact in _currentContacts.OrderBy(c => c.Norder))
            {
                int rowIndex = dgvContacts.Rows.Add(
                    contact.Id,
                    contact.Post ?? "",
                    contact.Fio ?? "",
                    contact.TfMobil ?? ""
                );

                // Если это ранее выбранный контакт - выделяем его
                if (selectedId.HasValue && contact.Id == selectedId.Value)
                {
                    dgvContacts.Rows[rowIndex].Selected = true;
                }
            }

            // Если выделение было потеряно - очищаем
            if (dgvContacts.SelectedRows.Count == 0)
            {
                _selectedContact = null;
            }
        }

        private void RefreshPersonalsGrid()
        {
            dgvPersonals.Rows.Clear();

            if (_filteredPersonals == null)
                return;

            foreach (var personal in _filteredPersonals)
            {
                dgvPersonals.Rows.Add(
                    personal.Personal.Id,
                    personal.FullName,
                    personal.AllowedPostsList,
                    personal.Personal.TfMobil ?? ""
                );
            }
        }

        private void SetEditMode(bool enabled)
        {
            dgvContacts.ReadOnly = !enabled;
            btnSaveContact.Enabled = enabled;
            btnCancelContact.Enabled = enabled;
            btnAddContact.Enabled = !enabled;
            btnDeleteContact.Enabled = !enabled;
            btnCreateAllKarauls.Enabled = !enabled;

            dgvContacts.AllowUserToAddRows = enabled;
            dgvContacts.AllowUserToDeleteRows = enabled;
        }

        private void SetRightPanelVisible(bool visible)
        {
            splitContainer.Panel2Collapsed = !visible;
            btnCloseRightPanel.Visible = visible;
            _showRightPanel = visible;
        }

        // --- Обработчики ---

        private void DgvContacts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvContacts.SelectedRows.Count > 0)
            {
                var row = dgvContacts.SelectedRows[0];
                if (row.Cells["Id"].Value != null)
                {
                    int id = Convert.ToInt32(row.Cells["Id"].Value);
                    _selectedContact = _currentContacts.FirstOrDefault(c => c.Id == id);
                }
            }
        }

        private void DgvContacts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvContacts.Rows[e.RowIndex];
            if (row.Cells["Id"].Value == null) return;

            _selectedContactId = Convert.ToInt32(row.Cells["Id"].Value);
            var contact = _currentContacts.FirstOrDefault(c => c.Id == _selectedContactId);

            if (contact != null)
            {
                SetRightPanelVisible(true);
                ApplyPersonalsFilter(contact.Post);
                dgvContacts.ClearSelection();
                row.Selected = true;
            }
        }
        private void BtnCloseRightPanel_Click(object sender, EventArgs e)
        {
            SetRightPanelVisible(false);
        }

        private void DgvPersonals_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // 1. Проверка: есть ли выбранная строка в правом гриде
                if (e.RowIndex < 0) return;

                var row = dgvPersonals.Rows[e.RowIndex];
                if (row.Cells["PersonalId"].Value == null) return;

                // 2. Получаем ID выбранного сотрудника
                int personalId = Convert.ToInt32(row.Cells["PersonalId"].Value);
                var personal = _filteredPersonals.FirstOrDefault(p => p.Personal.Id == personalId);

                if (personal == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Сотрудник с ID {personalId} не найден в кэше");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Выбран сотрудник: {personal.FullName} (ID: {personalId})");

                // 3. Проверка: выбран ли контакт в левом гриде
                if (dgvContacts.SelectedRows.Count == 0)
                {
                    MessageBox.Show(
                        "Не выбран контакт для замены.\n" +
                        "Пожалуйста, выберите контакт в левом списке.",
                        "Предупреждение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                // 4. Получаем выбранный контакт из левого грида
                var contactRow = dgvContacts.SelectedRows[0];
                if (contactRow.Cells["Id"].Value == null) return;

                int contactId = Convert.ToInt32(contactRow.Cells["Id"].Value);
                var contact = _currentContacts.FirstOrDefault(c => c.Id == contactId);

                if (contact == null)
                {
                    MessageBox.Show(
                        $"Контакт с ID {contactId} не найден в базе данных.",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Выбран контакт: {contact.Post} (ID: {contactId})");

                // 5. Проверка: есть ли у сотрудника разрешённая должность, совпадающая с контактом
                bool hasAllowedPost = personal.AllowedPosts.Any(p => p.Name == contact.Post);

                System.Diagnostics.Debug.WriteLine($"Проверка должности: {contact.Post}");
                System.Diagnostics.Debug.WriteLine($"Разрешённые должности: {personal.AllowedPostsList}");

                if (!hasAllowedPost)
                {
                    MessageBox.Show(
                        $"Сотрудник '{personal.FullName}' не имеет разрешённой должности '{contact.Post}'.\n\n" +
                        "Настройте разрешённые должности на вкладке 'Сотрудники'.\n\n" +
                        $"Разрешённые должности: {personal.AllowedPostsList}",
                        "Предупреждение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                // 6. Обновляем контакт данными сотрудника
                contact.Fio = personal.FullName;
                contact.Posyvnoy = personal.Personal.Posyvnoy ?? "";
                contact.TfDom = personal.Personal.TfDom ?? "";
                contact.TfMobil = personal.Personal.TfMobil ?? "";
                contact.TfWork = personal.Personal.TfWork ?? "";
                contact.Excel = personal.Personal.Zvanie ?? "";
                contact.EditTime = DateTime.Now;

                System.Diagnostics.Debug.WriteLine($"Обновлён контакт: {contact.Post} -> {contact.Fio}");

                // 7. Сохраняем изменения в БД
                if (_repository.SaveContact(contact))
                {
                    // 8. Обновляем грид контактов
                    RefreshContactsGrid();

                    // 9. Вызываем событие об изменении данных
                    DataChanged?.Invoke(this, EventArgs.Empty);

                    // 10. Показываем сообщение об успехе
                    MessageBox.Show(
                        $"Сотрудник {personal.FullName} назначен на должность {contact.Post}",
                        "Информация",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // 11. Закрываем правую панель
                    SetRightPanelVisible(false);
                }
                else
                {
                    // 12. Ошибка при сохранении
                    MessageBox.Show(
                        "Ошибка при обновлении контакта.\n" +
                        "Проверьте подключение к базе данных и попробуйте снова.",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка в DgvPersonals_CellDoubleClick: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");

                MessageBox.Show(
                    $"Произошла ошибка:\n{ex.Message}",
                    "Критическая ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private void BtnAddContact_Click(object sender, EventArgs e)
        {
            SetEditMode(true);

            int rowIndex = dgvContacts.Rows.Add();
            var row = dgvContacts.Rows[rowIndex];
            row.Cells["Karaul"].Value = _currentKaraul;
            row.Cells["Id"].Value = 0;

            if (_selectedContact != null)
                row.Cells["Post"].Value = _selectedContact.Post;

            dgvContacts.CurrentCell = dgvContacts.Rows[rowIndex].Cells["Post"];
            dgvContacts.ReadOnly = false;
            dgvContacts.Rows[rowIndex].Selected = true;
            dgvContacts.BeginEdit(true);
        }

        private void BtnDeleteContact_Click(object sender, EventArgs e)
        {
            if (dgvContacts.SelectedRows.Count == 0) return;

            var row = dgvContacts.SelectedRows[0];
            if (row.Cells["Id"].Value == null) return;

            int id = Convert.ToInt32(row.Cells["Id"].Value);

            if (id == 0)
            {
                dgvContacts.Rows.Remove(row);
                return;
            }

            if (MessageBox.Show($"Удалить контакт '{row.Cells["Post"].Value}'?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_repository.DeleteContact(id))
                {
                    _currentContacts.RemoveAll(c => c.Id == id);
                    dgvContacts.Rows.Remove(row);
                    lblTitle.Text = $"Контакты караула {_currentKaraul} ({_currentContacts.Count} записей)";
                    DataChanged?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCreateAllKarauls_Click(object sender, EventArgs e)
        {
            if (_selectedContact == null)
            {
                MessageBox.Show("Выберите контакт для создания во всех караулах",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(_selectedContact.Post))
            {
                MessageBox.Show("У выбранного контакта не указана должность",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show(
                $"Создать контакт '{_selectedContact.Post}' для всех караулов?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                bool success = _repository.CreateContactsForAllKarauls(
                    _currentPchId,
                    _selectedContact.PostId ?? 0,
                    _selectedContact.Post,
                    _currentSubdivisionId,
                    _currentSubdivisionName,
                    _currentGarnizonName
                );

                if (success)
                {
                    MessageBox.Show("Контакты созданы для всех караулов", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadContacts();
                    DataChanged?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Ошибка при создании контактов", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            // Перезагружаем все данные
            LoadAllPersonals();
            LoadContacts();

            string filter = cmbPostFilter.SelectedItem?.ToString();
            if (filter == "Все") filter = null;
            ApplyPersonalsFilter(filter);

            MessageBox.Show("Данные обновлены", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSaveContact_Click(object sender, EventArgs e)
        {
            try
            {
                var contactsToSave = GetContactsFromGrid();

                if (contactsToSave.Count == 0)
                {
                    MessageBox.Show("Нет данных для сохранения", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var invalidContacts = contactsToSave.Where(c => string.IsNullOrEmpty(c.Post)).ToList();
                if (invalidContacts.Any())
                {
                    MessageBox.Show(
                        $"Обнаружены контакты без указания должности ({invalidContacts.Count} шт.).\n" +
                        "Заполните должность перед сохранением.",
                        "Ошибка валидации",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                if (_repository.SaveContacts(contactsToSave))
                {
                    SetEditMode(false);
                    LoadContacts();
                    DataChanged?.Invoke(this, EventArgs.Empty);
                    SaveRequested?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("Данные сохранены", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelContact_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
            LoadContacts();
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ChkEditMode_CheckedChanged(object sender, EventArgs e)
        {
            SetEditMode(chkEditMode.Checked);
        }

        private void CmbPostFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = cmbPostFilter.SelectedItem?.ToString();
            if (filter == "Все") filter = null;

            // Применяем фильтр к кэшированному списку
            ApplyPersonalsFilter(filter);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                RefreshPersonalsGrid();
                return;
            }

            // Поиск по уже отфильтрованному списку
            var searchResults = _filteredPersonals
                .Where(p => p.FullName.ToLower().Contains(searchText) ||
                           (p.Personal.TfMobil != null && p.Personal.TfMobil.Contains(searchText)))
                .ToList();

            dgvPersonals.Rows.Clear();
            foreach (var personal in searchResults)
            {
                dgvPersonals.Rows.Add(
                    personal.Personal.Id,
                    personal.FullName,
                    personal.AllowedPostsList,
                    personal.Personal.TfMobil ?? ""
                );
            }
        }

        private List<Contact> GetContactsFromGrid()
        {
            var result = new List<Contact>();

            foreach (DataGridViewRow row in dgvContacts.Rows)
            {
                if (row.IsNewRow) continue;

                var contact = new Contact
                {
                    Id = row.Cells["Id"].Value != null ? Convert.ToInt32(row.Cells["Id"].Value) : 0,
                    Post = row.Cells["Post"].Value?.ToString(),
                    Fio = row.Cells["Fio"].Value?.ToString(),
                    TfMobil = row.Cells["TfMobil"].Value?.ToString(),
                    TfWork = row.Cells["TfWork"].Value?.ToString(),
                    TfDom = row.Cells["TfDom"].Value?.ToString(),
                    Posyvnoy = row.Cells["Posyvnoy"].Value?.ToString(),
                    Karaul = _currentKaraul,
                    SubdivisionId = _currentSubdivisionId,
                    EditTime = DateTime.Now
                };

                result.Add(contact);
            }

            return result;
        }

        public void SetTitle(string title)
        {
            if (lblTitle != null)
                lblTitle.Text = title;
        }

        public void UpdateKaraul(int karaul)
        {
            _currentKaraul = karaul;
            LoadContacts();
            SetRightPanelVisible(false);
        }

        /// <summary>
        /// Обновление кэша сотрудников (после изменений на вкладке "Сотрудники")
        /// </summary>
        public void RefreshPersonalsCache()
        {
            LoadAllPersonals();

            string filter = cmbPostFilter.SelectedItem?.ToString();
            if (filter == "Все") filter = null;
            ApplyPersonalsFilter(filter);
        }
    }
}