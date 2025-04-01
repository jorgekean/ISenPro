using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Service.Reports;
using System.Collections.Generic;
using System.Linq;

namespace Service.Reports.Transactions
{
    public class PPMPGenerateReport : BaseGenerateReport
    {
        // Merge field name to identify the row to clone
        private const string ROW_MERGEFIELD = "description";
        private Dictionary<string, string> _mergeValues;

        public override void GenerateReport(string templatePath, string outputPath, Dictionary<string, string> mergeValues)
        {
            // 1) Call base to copy the file and do standard merge field replacements
            base.GenerateReport(templatePath, outputPath, mergeValues);

            // 2) Now handle table merge fields in the output file
            FillPPMPTable(outputPath, ROW_MERGEFIELD, Items);

            _mergeValues = mergeValues;
        }

        public List<PPMPItem> Items { get; set; } = new List<PPMPItem>();

        private void FillPPMPTable(string filePath, string mergeFieldName, List<PPMPItem> items)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;

                // Find the table that has the merge field
                Table table = doc.MainDocumentPart.Document.Body
                    .Descendants<Table>()
                    .FirstOrDefault(t => t.InnerText.Contains(mergeFieldName));

                if (table != null)
                {
                    // Locate the row that contains the merge field
                    TableRow templateRow = table.Descendants<TableRow>()
                        .FirstOrDefault(r => r.InnerText.Contains(mergeFieldName));

                    if (templateRow != null)
                    {
                        foreach (var item in items)
                        {
                            // Clone the row
                            TableRow newRow = (TableRow)templateRow.CloneNode(true);

                            // First pass: Simple text replacement
                            foreach (var text in newRow.Descendants<Text>())
                            {
                                foreach (var prop in typeof(PPMPItem).GetProperties())
                                {
                                    string fieldPattern = $"«{prop.Name.ToLower()}»";
                                    if (text.Text.Contains(fieldPattern))
                                    {
                                        string value = prop.GetValue(item)?.ToString() ?? string.Empty;
                                        text.Text = text.Text.Replace(fieldPattern, value);
                                    }
                                }
                            }

                            //// Second pass: Clean up any remaining field artifacts
                            //foreach (var field in newRow.Descendants<FieldCode>().ToList())
                            //{
                            //    var parentRun = field.Parent as Run;
                            //    if (parentRun != null)
                            //    {
                            //        parentRun.Remove();
                            //    }
                            //}

                            // Append the new row
                            table.AppendChild(newRow);
                        }

                        // Remove the original template row
                        templateRow.Remove();
                    }
                }

                #region chart
                //// Add chart if we find a chart placeholder
                //var chartPlaceholder = body.Descendants<Text>()
                //    .FirstOrDefault(t => t.Text.Contains("«PIE_CHART»"));

                //if (chartPlaceholder != null)
                //{
                //    // Prepare chart data (example - group by category)
                //    //var chartData = items
                //    //    .GroupBy(i => i.Category) // Assuming PPMPItem has a Category property
                //    //    .ToDictionary(g => g.Key, g => g.Sum(i => i.TotalPrice));

                //    var chartData = new Dictionary<string, float>
                //    {
                //        { "PSDBMs", 1500000 },
                //        { "Supplementaries", 200000 },
                //        { "Projects", 300000 }

                //    };

                //    //byte[] chartImage = ChartGenerator.GeneratePieChart(chartData);

                //    // Insert the chart image
                //    WordChartInserter.InsertChartIntoWord(doc, chartData);
                //}
                #endregion

                doc.MainDocumentPart.Document.Save();
            }
        }

        //private string GetFieldValue(string fieldCode, PPMPItem item)
        //{
        //    if (fieldCode.Contains(" MERGEFIELD description "))
        //        return item.Description;
        //    if (fieldCode.Contains(" MERGEFIELD qtr1 "))
        //        return item.Qtr1.ToString();
        //    if (fieldCode.Contains(" MERGEFIELD qtr2 "))
        //        return item.Qtr2.ToString();
        //    if (fieldCode.Contains(" MERGEFIELD qtr3 "))
        //        return item.Qtr3.ToString();
        //    if (fieldCode.Contains(" MERGEFIELD qtr4 "))
        //        return item.Qtr4.ToString();
        //    if (fieldCode.Contains(" MERGEFIELD uom "))
        //        return item.UOM;
        //    if (fieldCode.Contains(" MERGEFIELD unitprice "))
        //        return item.UnitPrice.ToString("N2");
        //    if (fieldCode.Contains(" MERGEFIELD amount "))
        //        return item.Amount.ToString("N2");
        //    if (fieldCode.Contains(" MERGEFIELD remarks "))
        //        return item.Remarks;

        //    return null;
        //}
    }

    public class PPMPItem
    {
        public string Description { get; set; }
        public int Qtr1 { get; set; }
        public int Qtr2 { get; set; }
        public int Qtr3 { get; set; }
        public int Qtr4 { get; set; }
        public string UOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}