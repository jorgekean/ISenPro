using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using Service.Dto.Transaction;
using NPOI.SS.UserModel.Charts;
using NPOI.SS.Util;

namespace Service.Helpers
{
    public class ExcelHelper
    {


        public static async Task<byte[]> ExportToExcelAsync(Dictionary<string, DataTable> sheetsData)
        {
            // Create a new workbook
            IWorkbook workbook = new XSSFWorkbook();

            // Create a cell style for header
            ICellStyle headerStyle = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.IsBold = true;
            headerStyle.SetFont(font);

            foreach (var sheetData in sheetsData)
            {
                // Create a sheet
                ISheet sheet = workbook.CreateSheet(sheetData.Key);

                // Create header row
                IRow headerRow = sheet.CreateRow(0);

                // Add headers with bold style
                for (int i = 0; i < sheetData.Value.Columns.Count; i++)
                {
                    ICell cell = headerRow.CreateCell(i);
                    cell.SetCellValue(sheetData.Value.Columns[i].ColumnName);
                    cell.CellStyle = headerStyle;
                }

                // Add data rows
                for (int i = 0; i < sheetData.Value.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < sheetData.Value.Columns.Count; j++)
                    {
                        row.CreateCell(j).SetCellValue(sheetData.Value.Rows[i][j].ToString());
                    }
                }

                // Auto-size columns
                for (int i = 0; i < sheetData.Value.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            }

            // Convert workbook to byte array asynchronously
            using (var memoryStream = new MemoryStream())
            {
                // Write to memory stream synchronously (NPOI doesn't support async write)
                workbook.Write(memoryStream);

                // Return the buffer asynchronously
                return await Task.FromResult(memoryStream.ToArray());
            }
        }

        public static async Task<byte[]> GenerateExcelAsync(IWorkbook workbook)
        {
            // Convert workbook to byte array asynchronously
            using (var memoryStream = new MemoryStream())
            {
                // Write to memory stream synchronously (NPOI doesn't support async write)
                workbook.Write(memoryStream);

                // Return the buffer asynchronously
                return await Task.FromResult(memoryStream.ToArray());
            }
        }

        public static List<ISheet> CreateExcelSheets(IWorkbook workbook, Dictionary<string, DataTable> sheetsData)
        {
            var createdSheets = new List<ISheet>();

            // Create a cell style for header
            ICellStyle headerStyle = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.IsBold = true;
            headerStyle.SetFont(font);

            foreach (var sheetData in sheetsData)
            {
                // Create a sheet
                ISheet sheet = workbook.CreateSheet(sheetData.Key);
                createdSheets.Add(sheet);

                // Create header row
                // skip if no column names
                var skipHeader = sheetData.Value.Columns[0].ColumnName == "NOHEADER";

                if (!skipHeader)
                {
                    IRow headerRow = sheet.CreateRow(0);

                    // Add headers with bold style
                    for (int i = 0; i < sheetData.Value.Columns.Count; i++)
                    {
                        ICell cell = headerRow.CreateCell(i);
                        cell.SetCellValue(sheetData.Value.Columns[i].ColumnName);
                        cell.CellStyle = headerStyle;
                    }
                }

                // Add data rows
                for (int i = 0; i < sheetData.Value.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < sheetData.Value.Columns.Count; j++)
                    {
                        row.CreateCell(j).SetCellValue(sheetData.Value.Rows[i][j].ToString());
                    }
                }

                // Auto-size columns
                for (int i = 0; i < sheetData.Value.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            }

            return createdSheets;
        }


        public static ISheet CreateSideBySideSheet(IWorkbook workbook, PPMPDto data, string sheetName)
        {
            // Create sheet
            ISheet sheet = workbook.CreateSheet(sheetName);

            // Create styles
            ICellStyle headerStyle = workbook.CreateCellStyle();
            IFont headerFont = workbook.CreateFont();
            headerFont.IsBold = true;
            headerFont.FontHeight = 24 * 20; // Set font size
            headerStyle.SetFont(headerFont);

            // Create styles
            ICellStyle labelStyle = workbook.CreateCellStyle();
            IFont boldFont = workbook.CreateFont();
            boldFont.IsBold = true;
            boldFont.FontHeight = 16 * 20; // Set font size
            labelStyle.SetFont(boldFont);

            ICellStyle valueStyle = workbook.CreateCellStyle();
            IFont regularFont = workbook.CreateFont();
            regularFont.FontHeight = 16 * 20; // Set font size
            valueStyle.SetFont(regularFont);

            // Set column widths
            sheet.SetColumnWidth(0, 32 * 256); // First label column width
            sheet.SetColumnWidth(1, 36 * 256); // First value column width
            sheet.SetColumnWidth(2, 10 * 256); // Spacer column
            sheet.SetColumnWidth(3, 32 * 256); // Second label column width
            sheet.SetColumnWidth(4, 40 * 256); // Second value column width

            // First group of fields (left side)
            var leftFields = new Dictionary<string, string>
            {                
                {data.Ppmpno, ""},
                {"RequestingOffice", data.RequestingOffice.Name},
            };

            // Second group of fields (right side)
            var rightFields = new Dictionary<string, string>
            {
                {"PreparedBy", data.CreatedByStr},
                {"CreatedDate", data.CreatedDateStr}
            };

            // Create rows
            int maxRows = Math.Max(leftFields.Count, rightFields.Count);
            //int maxRows = leftFields.Count;
            for (int i = 0; i < maxRows; i++)
            {
                IRow row = sheet.CreateRow(i);
                int cellIndex = 0;

                // Left side (first label-value pair)
                if (i < leftFields.Count)
                {
                    var leftItem = leftFields.ElementAt(i);
                    // Label
                    ICell leftLabelCell = row.CreateCell(cellIndex++);
                    leftLabelCell.SetCellValue(leftItem.Key);
                    leftLabelCell.CellStyle = cellIndex == 1 && i == 0 ? headerStyle : labelStyle;
                    // Value
                    ICell leftValueCell = row.CreateCell(cellIndex++);
                    leftValueCell.SetCellValue(leftItem.Value);
                    leftValueCell.CellStyle = valueStyle;
                }
                else
                {
                    cellIndex += 2; // Skip left side if no more items
                }

                // Spacer column
                cellIndex++;

                // Right side (second label-value pair)
                if (i < rightFields.Count)
                {
                    var rightItem = rightFields.ElementAt(i);
                    // Label
                    ICell rightLabelCell = row.CreateCell(cellIndex++);
                    rightLabelCell.SetCellValue(rightItem.Key);
                    rightLabelCell.CellStyle = labelStyle;
                    // Value
                    ICell rightValueCell = row.CreateCell(cellIndex);
                    rightValueCell.SetCellValue(rightItem.Value);
                    rightValueCell.CellStyle = valueStyle;
                }
            }

            // =============================================
            // ADD PIE CHART BELOW THE DATA
            // =============================================

            // Create data for the chart (2 rows below last data row)
            int chartDataStartRow = maxRows + 2;

            // Create chart data headers
            //IRow chartHeaderRow = sheet.CreateRow(chartDataStartRow);
            //chartHeaderRow.CreateCell(0).SetCellValue("Amount Type");
            //chartHeaderRow.CreateCell(1).SetCellValue("Value");

            var currencyFormat = workbook.CreateDataFormat().GetFormat("₱#,##0.00");

            // Create chart data rows
            IRow catalogueRow = sheet.CreateRow(chartDataStartRow + 1);
            var catalogueCellLabel = catalogueRow.CreateCell(0).SetCellValue("Catalogue");
            var catalogueCellValue = catalogueRow.CreateCell(1).SetCellValue((double)data.CatalogueAmount.GetValueOrDefault());
            catalogueCellLabel.CellStyle = labelStyle;
            catalogueCellValue.CellStyle = valueStyle;
            catalogueCellValue.CellStyle.DataFormat = currencyFormat;

            IRow supplementaryRow = sheet.CreateRow(chartDataStartRow + 2);
            var suppCellLabel = supplementaryRow.CreateCell(0).SetCellValue("Supplementary");
            var suppCellVal = supplementaryRow.CreateCell(1).SetCellValue((double)data.SupplementaryAmount.GetValueOrDefault());
            suppCellLabel.CellStyle = labelStyle;
            suppCellVal.CellStyle = valueStyle;
            suppCellVal.CellStyle.DataFormat = currencyFormat;


            IRow projectRow = sheet.CreateRow(chartDataStartRow + 3);
            var projCellLabel = projectRow.CreateCell(0).SetCellValue("Project");
            var projCellVal = projectRow.CreateCell(1).SetCellValue((double)data.ProjectAmount.GetValueOrDefault());
            projCellLabel.CellStyle = labelStyle;
            projCellVal.CellStyle = valueStyle;
            projCellVal.CellStyle.DataFormat = currencyFormat;

            IRow totalAmountRow = sheet.CreateRow(chartDataStartRow + 4);
            var totalAmountCellLabel = totalAmountRow.CreateCell(0).SetCellValue("Total Amount");
            var totalAmountCellVal = totalAmountRow.CreateCell(1).SetCellValue((double)data.TotalAmount.GetValueOrDefault());
            totalAmountCellLabel.CellStyle = labelStyle;
            totalAmountCellVal.CellStyle = valueStyle;
            totalAmountCellVal.CellStyle.DataFormat = currencyFormat;

            // Create the drawing patriarch
            var drawing = sheet.CreateDrawingPatriarch();

            // Create chart anchor (positioning - below the data)
            var anchor = drawing.CreateAnchor(
                0, 0, 0, 0,
                0, chartDataStartRow + 7,  // Start 7 rows below data
                2, chartDataStartRow + 20 // End 15 rows below start
            );

            // Create chart and plot it
            var chart = drawing.CreateChart(anchor);

            var chartData = chart.ChartDataFactory.CreatePieChartData<string, double>();

            // Define data ranges
            var categoryAxis = DataSources.FromStringCellRange(
                sheet,
                new CellRangeAddress(chartDataStartRow + 1, chartDataStartRow + 3, 0, 0)
            );

            var valueAxis = DataSources.FromNumericCellRange(
                sheet,
                new CellRangeAddress(chartDataStartRow + 1, chartDataStartRow + 3, 1, 1)
            );

            // Add series to chart
            var series = chartData.AddSeries(categoryAxis, valueAxis);
            chart.Plot(chartData);

            // Customize chart appearance
            //chart.SetTitle("Amount Distribution");
            var legend = chart.GetOrCreateLegend();
            legend.Position = LegendPosition.Left;

            return sheet;
        }
    }
}
