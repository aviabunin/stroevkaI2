using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace stroevkaI.Controls
{
    public partial class StroevkaHelpDoc : UserControl
    {
        private static string HelpFilePath =>
            Path.Combine(AppContext.BaseDirectory, "HelpDoc.rtf");

        private bool _isEditMode = false;

        public StroevkaHelpDoc()
        {
            InitializeComponent();
            LoadDocument();
        }

        private void LoadDocument()
        {
            try
            {
                if (File.Exists(HelpFilePath))
                {
                    rtbHelp.LoadFile(HelpFilePath, RichTextBoxStreamType.RichText);
                }
                else
                {
                    rtbHelp.Text =
                        "Здесь будет описание изменений в программе.\r\n" +
                        "\r\n" +
                        "Нажмите «Редактировать», чтобы начать ввод.\r\n" +
                        "\r\n" +
                        "Можно также подготовить текст в Word и сохранить как .rtf,\r\n" +
                        "затем положить файл HelpDoc.rtf рядом с exe.";
                }
            }
            catch (Exception ex)
            {
                rtbHelp.Text = $"Не удалось загрузить справку: {ex.Message}";
            }
        }

        private void SaveDocument()
        {
            try
            {
                rtbHelp.SaveFile(HelpFilePath, RichTextBoxStreamType.RichText);
                MessageBox.Show("Сохранено в " + HelpFilePath, "Готово",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetEditMode(bool enabled)
        {
            _isEditMode = enabled;
            rtbHelp.ReadOnly = !enabled;

            btnEdit.Text = enabled ? "Выйти из режима" : "Редактировать";
            btnSave.Enabled = enabled;

            if (!enabled)
                SaveDocument();   // автосохранение при выходе
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            SetEditMode(!_isEditMode);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                var res = MessageBox.Show(
                    "Есть несохранённые изменения. Перезагрузить?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes) return;
            }
            LoadDocument();
        }
    }
}
