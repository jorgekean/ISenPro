using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Service.Reports
{
    public class BaseGenerateReport
    {
        // Common merge field names used in all reports
        public const string MERGEFIELD_BUDGETYEAR = "BUDGETYEAR";
        public const string MERGEFIELD_DEPARTMENTNAME = "DEPARTMENTNAME";

        public virtual void GenerateReport(string templatePath, string outputPath, Dictionary<string, string> mergeValues)
        {
            // 1) Copy template to output
            File.Copy(templatePath, outputPath, true);

            // 2) Replace the merge fields in the output file
            ReplaceMergeFields(outputPath, mergeValues);
        }

        protected virtual void ReplaceMergeFields(string filePath, Dictionary<string, string> mergeValues)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;

                // First pass: Simple text replacement for direct merge field patterns
                foreach (var text in body.Descendants<Text>())
                {
                    foreach (var kvp in mergeValues)
                    {
                        string fieldPattern = $"«{kvp.Key}»";
                        if (text.Text.Contains(fieldPattern))
                        {
                            text.Text = text.Text.Replace(fieldPattern, kvp.Value);
                        }
                    }
                }

                //// Second pass: Clean up any remaining field artifacts
                //foreach (var field in body.Descendants<FieldCode>().ToList())
                //{
                //    foreach (var kvp in mergeValues)
                //    {
                //        if (field.Text.Contains($" MERGEFIELD {kvp.Key} "))
                //        {
                //            // Remove the entire field structure
                //            var parent = field.Parent;
                //            if (parent != null)
                //            {
                //                parent.Remove();
                //            }
                //            break;
                //        }
                //    }
                //}

                doc.MainDocumentPart.Document.Save();
            }
        }

        protected List<Run> GetCompleteFieldRuns(Run fieldCodeRun)
        {
            var fieldRuns = new List<Run>();
            var currentElement = fieldCodeRun.Parent;

            // Walk through siblings to find the complete field structure
            foreach (var child in currentElement.Elements())
            {
                if (child is Run run)
                {
                    // Check if this run is part of the field structure
                    if (run.Descendants<FieldCode>().Any() ||
                        run.Descendants<FieldChar>().Any())
                    {
                        fieldRuns.Add(run);
                    }
                }
            }

            return fieldRuns;
        }

        protected Run GetFieldDisplayRun(List<Run> fieldRuns)
        {
            bool foundSeparator = false;

            foreach (var run in fieldRuns)
            {
                if (!foundSeparator)
                {
                    // Look for the separator that marks the beginning of the display text
                    if (run.Descendants<FieldChar>().Any(fc => fc.FieldCharType.Value == FieldCharValues.Separate))
                    {
                        foundSeparator = true;
                    }
                    continue;
                }

                // The first run after the separator is typically the display run
                return run;
            }

            return null;
        }

        protected void ReplaceFieldWithText(List<Run> fieldRuns, Run newRun)
        {
            if (fieldRuns.Count == 0) return;

            var parent = fieldRuns[0].Parent;

            // Insert the new run before the first field run
            parent.InsertBefore(newRun, fieldRuns[0]);

            // Remove all field runs
            foreach (var run in fieldRuns)
            {
                parent.RemoveChild(run);
            }
        }

        protected Text FindFieldDisplayText(Run run)
        {
            // First check if this run contains the display text (common case)
            var textElement = run.Elements<Text>().FirstOrDefault();
            if (textElement != null)
            {
                return textElement;
            }

            // If not, look for the separate field char and following text
            bool foundSeparator = false;
            foreach (var element in run.Elements())
            {
                if (element is FieldChar fc && fc.FieldCharType.Value == FieldCharValues.Separate)
                {
                    foundSeparator = true;
                    continue;
                }

                if (foundSeparator && element is Text)
                {
                    return (Text)element;
                }
            }

            return null;
         }
    }
}