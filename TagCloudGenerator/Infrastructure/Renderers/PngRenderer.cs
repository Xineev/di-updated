using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;
using System.IO;

namespace TagCloudGenerator.Infrastructure.Renderers
{
    public class PngRenderer : IRenderer
    {
        public void Render(IEnumerable<CloudItem> items, RenderSettings settings)
        {
            var itemsList = items.ToList();
            if (!itemsList.Any())
            {
                Console.WriteLine("No elements to render");
                return;
            }

            using var bitmap = new Bitmap(settings.CanvasSize.Width, settings.CanvasSize.Height);
            using var graphics = Graphics.FromImage(bitmap);

            ConfigureGraphics(graphics);
            graphics.Clear(settings.BackgroundColor);

            var (offsetX, offsetY) = CalculateOffset(itemsList, settings);

            foreach (var item in itemsList)
            {
                DrawCloudItem(graphics, item, offsetX, offsetY, settings);
            }

            bitmap.Save(settings.OutputPath, ImageFormat.Png);
        }

        private (int offsetX, int offsetY) CalculateOffset(
            List<CloudItem> items,
            RenderSettings settings)
        {
            if (!settings.CenterCloud)
                return (settings.Padding, settings.Padding);

            var minX = items.Min(i => i.Rectangle.X);
            var minY = items.Min(i => i.Rectangle.Y);
            var maxX = items.Max(i => i.Rectangle.Right);
            var maxY = items.Max(i => i.Rectangle.Bottom);

            var cloudWidth = maxX - minX;
            var cloudHeight = maxY - minY;

            var offsetX = (settings.CanvasSize.Width - cloudWidth) / 2 - minX;
            var offsetY = (settings.CanvasSize.Height - cloudHeight) / 2 - minY;

            return (offsetX, offsetY);
        }

        private void DrawCloudItem(
            Graphics graphics,
            CloudItem item,
            int offsetX,
            int offsetY,
            RenderSettings settings)
        {
            var drawRect = new Rectangle(
                item.Rectangle.X + offsetX,
                item.Rectangle.Y + offsetY,
                item.Rectangle.Width,
                item.Rectangle.Height);

            var color = item.Color ?? settings.TextColor;

            using var font = new Font(
                item.FontFamily,
                item.FontSize,
                item.FontStyle,
                GraphicsUnit.Pixel);

            using var brush = new SolidBrush(color);
            using var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            graphics.DrawString(item.Word, font, brush, drawRect, stringFormat);

            if (settings.ShowRectangles)
            {
                using var pen = new Pen(color, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                graphics.DrawRectangle(pen, drawRect);
            }
        }

        private void ConfigureGraphics(Graphics graphics)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        }
    }
}
