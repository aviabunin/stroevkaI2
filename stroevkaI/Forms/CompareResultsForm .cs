using stroevkaI.Services.Tests;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace stroevkaI.Forms
{
    public partial class CompareResultsForm : Form
    {
        public CompareResultsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Отображает результаты сравнения
        /// </summary>
        public void ShowResults(List<GridComparisonResult> results)
        {
            if (results == null || results.Count == 0)
            {
                dgvResults.Rows.Clear();
                dgvDetails.Rows.Clear();
                lblSummary.Text = "Нет данных для сравнения";
                return;
            }

            dgvResults.Rows.Clear();
            dgvDetails.Rows.Clear();

            // Счетчики
            int totalRows = results.Count;
            int matchCount = results.Count(r => r.Status == "Совпадает");
            int diffCount = results.Count(r => r.Status == "Различия");
            int notFoundCount = results.Count(r => r.Status == "Нет в гриде");
            int totalDifferences = results.Sum(r => r.Differences?.Count ?? 0);

            lblSummary.Text = $"Всего: {totalRows}  |  Совпадает: {matchCount}  |  Различия: {diffCount}  |  Нет в гриде: {notFoundCount}  |  Всего различий: {totalDifferences}";

            foreach (var result in results)
            {
                int rowIndex = dgvResults.Rows.Add(
                    result.RowId ?? "",
                    result.Наименование ?? "",
                    result.Status ?? "Неизвестно",
                    result.Differences?.Count ?? 0
                );

                DataGridViewRow row = dgvResults.Rows[rowIndex];
                row.Tag = result.Differences;
            }

            // Если есть различия – выбираем первую строку с различиями
            if (diffCount > 0)
            {
                foreach (DataGridViewRow row in dgvResults.Rows)
                {
                    string status = GetCellValue(row, "Status");
                    if (status == "Различия")
                    {
                        row.Selected = true;
                        dgvResults.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
        }

        // Обработчик CellFormatting для постоянной подкраски строк в dgvResults
        private void DgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvResults.Rows.Count)
                return;

            DataGridViewRow row = dgvResults.Rows[e.RowIndex];
            if (row.IsNewRow)
                return;

            string status = GetCellValue(row, "Status");
            switch (status)
            {
                case "Совпадает":
                    row.DefaultCellStyle.BackColor = Color.FromArgb(200, 230, 200);
                    row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                    break;
                case "Различия":
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200);
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    break;
                case "Нет в гриде":
                    row.DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                    break;
                default:
                    // Сброс для неизвестного статуса
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    break;
            }
        }

        private string GetCellValue(DataGridViewRow row, string columnName)
        {
            if (row == null || string.IsNullOrEmpty(columnName))
                return string.Empty;

            if (!dgvResults.Columns.Contains(columnName))
                return string.Empty;

            var cell = row.Cells[columnName];
            if (cell == null || cell.Value == null)
                return string.Empty;

            return cell.Value.ToString();
        }

        private string GetCellValue(DataGridViewRow row, int columnIndex)
        {
            if (row == null || columnIndex < 0 || columnIndex >= dgvResults.Columns.Count)
                return string.Empty;

            var cell = row.Cells[columnIndex];
            if (cell == null || cell.Value == null)
                return string.Empty;

            return cell.Value.ToString();
        }

        private void DgvResults_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvResults.SelectedRows.Count > 0)
            {
                var selectedRow = dgvResults.SelectedRows[0];
                var differences = selectedRow.Tag as List<FieldDifference>;

                dgvDetails.Rows.Clear();

                if (differences != null && differences.Count > 0)
                {
                    foreach (var diff in differences)
                    {
                        int rowIndex = dgvDetails.Rows.Add(
                            diff.ColumnNumber,
                            diff.FieldName ?? "",
                            diff.ExcelValue ?? "",
                            diff.GridValue ?? ""
                        );

                        DataGridViewRow row = dgvDetails.Rows[rowIndex];

                        // Подкраска: если значение в гриде пустое – серый, иначе жёлтый
                        bool isMissingInGrid = string.IsNullOrEmpty(diff.GridValue?.Trim());
                        if (isMissingInGrid)
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                            row.DefaultCellStyle.ForeColor = Color.DimGray;
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200);
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
                else
                {
                    dgvDetails.Rows.Add("", "Нет различий в данной строке", "", "");
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx";
                    saveDialog.DefaultExt = "csv";
                    saveDialog.FileName = $"Сравнение_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                    saveDialog.Title = "Сохранить отчет сравнения";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportToCsv(saveDialog.FileName);
                        MessageBox.Show($"Отчет успешно экспортирован в {saveDialog.FileName}",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv(string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("RowId;Наименование;Статус;Кол-во различий");

                foreach (DataGridViewRow row in dgvResults.Rows)
                {
                    if (row.IsNewRow) continue;

                    string rowId = GetCellValue(row, "RowId");
                    string name = GetCellValue(row, "Name");
                    string status = GetCellValue(row, "Status");
                    string diffCount = GetCellValue(row, "DiffCount");

                    writer.WriteLine($"{rowId};{name};{status};{diffCount}");
                }

                writer.WriteLine();
                writer.WriteLine("=== ДЕТАЛИ РАЗЛИЧИЙ ===");
                writer.WriteLine();

                writer.WriteLine("RowId;Колонка;Поле;Значение в Excel;Значение в гриде");

                foreach (DataGridViewRow row in dgvResults.Rows)
                {
                    if (row.IsNewRow) continue;

                    string rowId = GetCellValue(row, "RowId");
                    var differences = row.Tag as List<FieldDifference>;

                    if (differences != null && differences.Count > 0)
                    {
                        foreach (var diff in differences)
                        {
                            writer.WriteLine($"{rowId};{diff.ColumnNumber};{diff.FieldName};{diff.ExcelValue};{diff.GridValue}");
                        }
                    }
                }

                writer.WriteLine();
                writer.WriteLine("=== СТАТИСТИКА ===");
                writer.WriteLine($"Дата экспорта: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine($"Всего строк: {dgvResults.Rows.Count}");

                int matchCount = 0, diffCountStat = 0, notFoundCount = 0;
                foreach (DataGridViewRow row in dgvResults.Rows)
                {
                    if (row.IsNewRow) continue;
                    string status = GetCellValue(row, "Status");
                    switch (status)
                    {
                        case "Совпадает": matchCount++; break;
                        case "Различия": diffCountStat++; break;
                        case "Нет в гриде": notFoundCount++; break;
                    }
                }

                writer.WriteLine($"Совпадает: {matchCount}");
                writer.WriteLine($"Различия: {diffCountStat}");
                writer.WriteLine($"Нет в гриде: {notFoundCount}");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}