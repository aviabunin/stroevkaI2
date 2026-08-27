// stroevkaI/Forms/PersonalsEditor.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.ModelsStroevkaMySql.Repositories;

namespace stroevkaI.Forms
{
    public partial class PersonalsEditor : UserControl
    {
        private readonly ContactRepository _repository;
        private List<ContactRepository.PersonalWithPosts> _personals;
        private List<Post> _allPosts;
        private int _currentPchId;

        public PersonalsEditor(int pchId)
        {
            InitializeComponent();
            _repository = new ContactRepository();
            _currentPchId = pchId;

            LoadData();

            // Добавляем кнопку "Просмотр должностей" в панель
            AddViewPostsButton();
        }

        private void AddViewPostsButton()
        {
            var btnViewPosts = new Button();
            btnViewPosts.Text = "Просмотр должностей";
            btnViewPosts.Location = new System.Drawing.Point(520, 8);
            btnViewPosts.Size = new System.Drawing.Size(130, 28);
            btnViewPosts.TabIndex = 9;
            btnViewPosts.UseVisualStyleBackColor = true;
            btnViewPosts.Click += new System.EventHandler(this.BtnViewPosts_Click);
            this.panelButtons.Controls.Add(btnViewPosts);
        }

        private void LoadData()
        {
            _allPosts = _repository.LoadPosts();
            _personals = _repository.LoadPersonalsWithPosts(_currentPchId);

            LoadPostFilter();
            RefreshGrid();
        }

        private void LoadPostFilter()
        {
            cmbPostFilter.Items.Clear();
            cmbPostFilter.Items.Add("Все");
            foreach (var post in _allPosts)
            {
                cmbPostFilter.Items.Add(post.Name);
            }
            cmbPostFilter.SelectedIndex = 0;
        }

        private void RefreshGrid()
        {
            dgvPersonals.Rows.Clear();

            string filter = cmbPostFilter.SelectedItem?.ToString();
            var filtered = _personals;

            if (filter != "Все" && !string.IsNullOrEmpty(filter))
            {
                filtered = filtered.Where(p => p.AllowedPosts.Any(pp => pp.Name == filter)).ToList();
            }

            foreach (var item in filtered)
            {
                dgvPersonals.Rows.Add(
                    item.Personal.Id,
                    item.FullName,
                    item.Personal.Post ?? "",
                    item.AllowedPostsList,
                    item.Personal.TfMobil ?? "",
                    item.Personal.TfWork ?? "",
                    item.Personal.TfDom ?? ""
                );
            }

            lblCount.Text = $"Всего: {filtered.Count} сотрудников";
        }

        private void RefreshGridWithSearch(string searchText)
        {
            dgvPersonals.Rows.Clear();

            string filter = cmbPostFilter.SelectedItem?.ToString();
            var filtered = _personals;

            if (filter != "Все" && !string.IsNullOrEmpty(filter))
            {
                filtered = filtered.Where(p => p.AllowedPosts.Any(pp => pp.Name == filter)).ToList();
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(p =>
                    p.FullName.ToLower().Contains(searchText) ||
                    (p.Personal.TfMobil ?? "").Contains(searchText) ||
                    (p.Personal.Post ?? "").ToLower().Contains(searchText)
                ).ToList();
            }

            foreach (var item in filtered)
            {
                dgvPersonals.Rows.Add(
                    item.Personal.Id,
                    item.FullName,
                    item.Personal.Post ?? "",
                    item.AllowedPostsList,
                    item.Personal.TfMobil ?? "",
                    item.Personal.TfWork ?? "",
                    item.Personal.TfDom ?? ""
                );
            }

            lblCount.Text = string.IsNullOrEmpty(searchText)
                ? $"Всего: {filtered.Count} сотрудников"
                : $"Найдено: {filtered.Count} сотрудников";
        }

        // --- Обработчики событий ---

        private void BtnViewPosts_Click(object sender, EventArgs e)
        {
            if (dgvPersonals.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для просмотра разрешённых должностей",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvPersonals.SelectedRows[0];
            if (row.Cells["IdColumn"].Value == null) return;

            int personalId = Convert.ToInt32(row.Cells["IdColumn"].Value);
            var personal = _personals.FirstOrDefault(p => p.Personal.Id == personalId);

            if (personal == null) return;

            // Открываем форму для просмотра/редактирования должностей
            using (var form = new EditPersonalPostsForm(personal, _allPosts))
            {
                form.ShowDialog();
            }
        }

        private void DgvPersonals_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvPersonals.Rows[e.RowIndex];
            if (row.Cells["IdColumn"].Value == null) return;

            int personalId = Convert.ToInt32(row.Cells["IdColumn"].Value);
            var personal = _personals.FirstOrDefault(p => p.Personal.Id == personalId);

            if (personal == null) return;

            using (var form = new EditPersonalPostsForm(personal, _allPosts))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (_repository.SavePersonalPosts(personalId, form.SelectedPostIds))
                    {
                        LoadData();
                        MessageBox.Show("Должности сотрудника обновлены", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при сохранении", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CmbPostFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void BtnAddPersonal_Click(object sender, EventArgs e)
        {
            var newPersonal = new Personal
            {
                SubdivisionId = _currentPchId,
                Inwork = 1,
                F = "Новый_сотрудник",
                I = "",
                O = "",
                Post = "",
                TfMobil = "",
                TfWork = "",
                TfDom = "",
                Posyvnoy = "",
                Zvanie = ""
            };

            if (_repository.SavePersonal(newPersonal))
            {
                LoadData();
                MessageBox.Show("Сотрудник добавлен", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении сотрудника", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeletePersonal_Click(object sender, EventArgs e)
        {
            if (dgvPersonals.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvPersonals.SelectedRows[0];
            if (row.Cells["IdColumn"].Value == null) return;

            int personalId = Convert.ToInt32(row.Cells["IdColumn"].Value);
            var personal = _personals.FirstOrDefault(p => p.Personal.Id == personalId);

            if (personal == null) return;

            if (MessageBox.Show($"Удалить сотрудника '{personal.FullName}'?\nОн будет помечен как уволенный.",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_repository.DeletePersonal(personalId))
                {
                    LoadData();
                    MessageBox.Show("Сотрудник удалён", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show("Данные обновлены", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                RefreshGrid();
            }
            else
            {
                RefreshGridWithSearch(searchText);
            }
        }
    }
}