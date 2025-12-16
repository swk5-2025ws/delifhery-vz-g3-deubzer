using DeliFHery.API.Interfaces;
using SkiaSharp;
using System.Reflection.Metadata;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp.Rendering;

namespace DeliFHery.API.Services
{
    public class LabelGenerator : ILabelGenerator
    {

        public Task<string> GenerateLabelAsync(string trackingNumber, string recipientName, string recipientStreet, string recipientPostalCode, string recipientCity, CancellationToken ct)
        {
            var writer = new BarcodeWriter<SKBitmap>
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Width = 900,
                    Height = 220,
                    Margin = 10,
                    PureBarcode = true
                },
                Renderer = new SKBitmapRenderer()
            };

            using var barcodeBmp = writer.Write(trackingNumber);

            const int width = 1000;
            const int height = 700;


            using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul));
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            using (var borderPaint = new SKPaint { Color = SKColors.Black, IsStroke = true, StrokeWidth = 4 })
            {
                canvas.DrawRect(new SKRect(8, 8, width - 8, height - 8), borderPaint);
            }

            using var titlePaint = new SKPaint { Color = SKColors.Black, TextSize = 28, IsAntialias = true, Typeface = SKTypeface.FromFamilyName("Arial") };
            using var textPaint = new SKPaint { Color = SKColors.Black, TextSize = 24, IsAntialias = true, Typeface = SKTypeface.FromFamilyName("Arial") };
            using var smallPaint = new SKPaint { Color = SKColors.Black, TextSize = 26, IsAntialias = true, Typeface = SKTypeface.FromFamilyName("Arial") };

            int x = 40;
            int y = 70;

            canvas.DrawText("DeliFHery Label", x, y, titlePaint);

            y += 60;
            canvas.DrawText($"TRACKING: {trackingNumber}", x, y, textPaint);

            y += 60;
            canvas.DrawText("TO:", x, y, textPaint);

            y += 50;
            canvas.DrawText(recipientName, x, y, textPaint);

            y += 45;
            canvas.DrawText(recipientStreet, x, y, textPaint);

            y += 45;
            canvas.DrawText($"{recipientPostalCode} {recipientCity}", x, y, textPaint);

            int barcodeX = (width - barcodeBmp.Width) / 2;
            int barcodeY = height - barcodeBmp.Height - 90;

            canvas.DrawBitmap(barcodeBmp, barcodeX, barcodeY);

            var textWidth = smallPaint.MeasureText(trackingNumber);
            canvas.DrawText(trackingNumber, (width - textWidth) / 2, height - 35, smallPaint);

            using var img = surface.Snapshot();
            using var pngData = img.Encode(SKEncodedImageFormat.Png, 100);

            var base64 = Convert.ToBase64String(pngData.ToArray());
            return Task.FromResult(base64);
        }
    }
}
