// stroevkaI/Forms/EditPersonalPostsForm.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StorageI.ModelsStroevkaMySql;
using StorageI.ModelsStroevkaMySql.Repositories;

namespace stroevkaI.Forms
{
    public partial class EditPersonalPostsForm : Form
    {
        private readonly ContactRepository.PersonalWithPosts _personal;
        private readonly List<Post> _allPosts;
        private List<int> _selectedPostIds;
        private bool _readOnly;

        public List<int> SelectedPostIds => _selectedPostIds;

        public EditPersonalPostsForm(ContactRepository.PersonalWithPosts personal, List<Post> allPosts, bool readOnly = false)
        {
            InitializeComponent();
            _personal = personal;
            _allPosts = allPosts;
            _selectedPostIds = personal.AllowedPosts.Select(p => p.Id).ToList();
            _readOnly = readOnly;

            LoadData();
        }

        private void LoadData()
        {
            this.Text = _readOnly
                ? $"Просмотр должностей - {_personal.FullName}"
                : $"Редактирование должностей - {_personal.FullName}";

            lblPersonal.Text = $"Сотрудник: {_personal.FullName}";
            lblSubdivision.Text = $"Подразделение: {_personal.Personal.Subdivision}";

            clbPosts.Items.Clear();
            foreach (var post in _allPosts.OrderBy(p => p.Norder))
            {
                int index = clbPosts.Items.Add($"{post.Norder}. {post.Name}");
                clbPosts.SetItemChecked(index, _selectedPostIds.Contains(post.Id));
            }

            // Если режим только для просмотра - блокируем изменения
            if (_readOnly)
            {
                clbPosts.Enabled = false;
                btnSave.Visible = false;
                btnSave.Enabled = false;
                btnCancel.Text = "Закрыть";
                this.Text = $"Просмотр должностей - {_personal.FullName}";
            }

            UpdateCount();
        }

        private void UpdateCount()
        {
            int count = 0;
            for (int i = 0; i < clbPosts.Items.Count; i++)
            {
                if (clbPosts.GetItemChecked(i))
                    count++;
            }
            lblCount.Text = $"Выбрано: {count} должностей";
        }

        private void ClbPosts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_readOnly) return;

            // Используем BeginInvoke только если дескриптор уже создан
            if (clbPosts.IsHandleCreated)
            {
                this.BeginInvoke((Action)(() => UpdateCount()));
            }
            else
            {
                // Если дескриптор ещё не создан, обновляем напрямую
                UpdateCount();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_readOnly)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            _selectedPostIds.Clear();
            for (int i = 0; i < clbPosts.Items.Count; i++)
            {
                if (clbPosts.GetItemChecked(i))
                {
                    var post = _allPosts[i];
                    _selectedPostIds.Add(post.Id);
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}