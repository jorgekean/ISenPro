using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Drawing.Pictures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using NonVisualGraphicFrameDrawingProperties = DocumentFormat.OpenXml.Drawing.Wordprocessing.NonVisualGraphicFrameDrawingProperties;
using Picture = DocumentFormat.OpenXml.Wordprocessing.Picture;
using NonVisualPictureProperties = DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureProperties;
using ShapeProperties = DocumentFormat.OpenXml.Drawing.ShapeProperties;
using BlipFill = DocumentFormat.OpenXml.Drawing.BlipFill;
using NonVisualDrawingProperties = DocumentFormat.OpenXml.Drawing.Pictures.NonVisualDrawingProperties;
using NonVisualPictureDrawingProperties = DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureDrawingProperties;
using SkiaSharp;

public class WordChartInserter
{
    public static void InsertChartIntoWord(WordprocessingDocument doc, Dictionary<string, float> chartData)
    {
        byte[] imageBytes = ChartGenerator.GeneratePieChart(chartData);

        //save the image to a document folder location
        string imageFilePath = "C:\\Users\\Jorge\\Documents\\Freelance\\PPMS\\Report Output\\PieChart.png";
        File.WriteAllBytes(imageFilePath, imageBytes);


        // Find and replace the merge field
        var mainPart = doc.MainDocumentPart;
        var body = mainPart.Document.Body;

        foreach (var text in body.Descendants<Text>())
        {
            string fieldPattern = "«PIE_CHART»";
            if (text.Text.Contains(fieldPattern))
            {
                // Find the parent run and remove the text
                Run parentRun = text.Parent as Run;
                if (parentRun != null)
                {
                    parentRun.RemoveAllChildren<Text>();

                    // Insert the image
                    AddImageToRun(doc, parentRun);
                }
            }
        }

        mainPart.Document.Save();
    }

    private static void AddImageToRun(WordprocessingDocument doc, Run run)
    {
        // Generate Pie Chart Image
        byte[] imageBytes = GeneratePieChart();

        // Add the image to the Word document
        var mainPart = doc.MainDocumentPart;
        var imagePart = mainPart.AddImagePart(ImagePartType.Png);

        using (var stream = new MemoryStream(imageBytes))
        {
            imagePart.FeedData(stream);
        }

        // Get image ID
        string imageId = mainPart.GetIdOfPart(imagePart);

        // Create the drawing element
        Drawing drawing = new Drawing(
            new Inline(
                new Extent() { Cx = 990000L, Cy = 792000L }, // Set size
                new EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
                new DocProperties() { Id = 1U, Name = "PieChart" },
                new NonVisualGraphicFrameDrawingProperties(new GraphicFrameLocks() { NoChangeAspect = true }),
                new Graphic(
                    new GraphicData(
                        new Picture(
                            new NonVisualPictureProperties(
                                new NonVisualDrawingProperties() { Id = 0U, Name = "PieChart.png" },
                                new NonVisualPictureDrawingProperties()),
                            new BlipFill(
                                new Blip() { Embed = imageId },
                                new Stretch(new FillRectangle())),
                            new ShapeProperties(
                                new Transform2D(
                                    new Offset() { X = 0L, Y = 0L },
                                    new Extents() { Cx = 990000L, Cy = 792000L }),
                                new PresetGeometry(new AdjustValueList()) { Preset = ShapeTypeValues.Rectangle })))
                    { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }))
            { DistanceFromTop = 0U, DistanceFromBottom = 0U, DistanceFromLeft = 0U, DistanceFromRight = 0U });

        // Append the drawing to the run
        run.AppendChild(drawing);
    }

    private static byte[] GeneratePieChart()
    {
        int width = 300, height = 200;
        using (var surface = SKSurface.Create(new SKImageInfo(width, height)))
        {
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            float[] values = { 30, 50, 20 };
            SKColor[] colors = { SKColors.Red, SKColors.Blue, SKColors.Green };
            float total = values.Sum();
            float startAngle = 0;

            for (int i = 0; i < values.Length; i++)
            {
                float sweepAngle = (values[i] / total) * 360;
                using (SKPaint paint = new SKPaint { Color = colors[i], IsAntialias = true })
                {
                    canvas.DrawArc(new SKRect(50, 50, 250, 250), startAngle, sweepAngle, true, paint);
                }
                startAngle += sweepAngle;
            }

            using (SKImage img = surface.Snapshot())
            using (SKData data = img.Encode(SKEncodedImageFormat.Png, 100))
            {
                return data.ToArray();
            }
        }
    }
}
