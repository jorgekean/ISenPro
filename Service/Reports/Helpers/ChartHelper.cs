using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;

public class ChartGenerator
{
    public static byte[] GeneratePieChart(Dictionary<string, float> data, int width = 500, int height = 500)
    {
        using (SKBitmap bitmap = new SKBitmap(width, height))
        using (SKCanvas canvas = new SKCanvas(bitmap))
        {
            canvas.Clear(SKColors.White);

            float total = 0;
            foreach (var value in data.Values)
                total += value;

            float startAngle = 0;
            Random rand = new Random();

            foreach (var entry in data)
            {
                float sweepAngle = (entry.Value / total) * 360;
                SKColor color = new SKColor((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));

                using (SKPaint paint = new SKPaint { Color = color, IsAntialias = true })
                {
                    canvas.DrawArc(new SKRect(50, 50, width - 50, height - 50),
                                   startAngle, sweepAngle, true, paint);
                }
                startAngle += sweepAngle;
            }

            using (SKImage image = SKImage.FromBitmap(bitmap))
            using (SKData dataStream = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                return dataStream.ToArray(); // Returns image as a byte array
            }
        }
    }
}
