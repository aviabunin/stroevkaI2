using System;
using System.Drawing;
using System.Windows.Forms;
using stroevkaI.Services;

namespace stroevkaI.Forms
{
    /// <summary>
    /// Панель лога изменений: ПЧ | Параметр | Старое | Новое | Время.
    /// Автоматически обновляется при вызове ChangeLogService.Add/Clear.
    /// </summary>
    public partial class ChangeLogPanel : UserControl
    {
        public ChangeLogPanel()
        {
            InitializeComponent();
            SetupGrid();

            // Подписываемся на изменения в сервисе
            ChangeLogService.Instance.Changed += OnLogChanged;

            // Если панель будет удалена — отпишемся
            this.Disposed += (s, e) =>
            {
                ChangeLogService.Instance.Changed -= OnLogChanged;
            };

            Reload();
        }

        // -----------------------------------------------------------------
        // Настройка грида — по общему стилю (как в редакторах)
        // -----------------------------------------------------------------
        private void SetupGrid()
        {
            dgvLog.Columns.Clear();
            dgvLog.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPch",
                HeaderText = "ПЧ",
                Width = 80,
                ReadOnly = true
            });
            dgvLog.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colField",
                HeaderText = "Параметр",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });
            dgvLog.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colOld",
                HeaderText = "Старое",
                Width = 60,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvLog.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNew",
                HeaderText = "Новое",
                Width = 60,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvLog.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTime",
                HeaderText = "Время",
                Width = 60,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvLog.ReadOnly = true;
            dgvLog.AllowUserToAddRows = false;
            dgvLog.AllowUserToDeleteRows = false;
            dgvLog.RowHeadersVisible = false;
            dgvLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLog.MultiSelect = false;
            dgvLog.BorderStyle = BorderStyle.Fixed3D;
            dgvLog.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLog.RowTemplate.Height = 20;
            dgvLog.ColumnHeadersHeight = 24;

            dgvLog.EnableHeadersVisualStyles = false;
            dgvLog.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            dgvLog.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvLog.ColumnHeadersDefaultCellStyle.Font =
                new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            dgvLog.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dgvLog.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvLog.BackgroundColor = Color.White;
            dgvLog.DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 216, 230);
            dgvLog.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLog.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            dgvLog.RowHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dgvLog.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
        }

        // -----------------------------------------------------------------
        // Реакция на изменения в сервисе
        // -----------------------------------------------------------------
        private void OnLogChanged(object sender, EventArgs e)
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(Reload));
                return;
            }
            Reload();
        }

        private void Reload()
        {
            dgvLog.Rows.Clear();

            foreach (var entry in ChangeLogService.Instance.Entries)
            {
                int idx = dgvLog.Rows.Add();
                var row = dgvLog.Rows[idx];
                row.Cells["colPch"].Value = entry.PchName;
                row.Cells["colField"].Value = entry.FieldName;
                row.Cells["colOld"].Value = entry.OldValue;
                row.Cells["colNew"].Value = entry.NewValue;
                row.Cells["colTime"].Value = entry.ChangedAt.ToString("HH:mm:ss");
                row.Tag = entry;
            }

            // Автопрокрутка вниз — оператор всегда видит последнее изменение
            if (dgvLog.Rows.Count > 0)
            {
                int lastIndex = dgvLog.Rows.Count - 1;
                dgvLog.FirstDisplayedScrollingRowIndex = lastIndex;
                dgvLog.ClearSelection();
                dgvLog.Rows[lastIndex].Selected = true;
            }
        }

        // Публичный метод для принудительной очистки (если нужно)
        public void ClearLog()
        {
            ChangeLogService.Instance.Clear();
        }
    }
}
