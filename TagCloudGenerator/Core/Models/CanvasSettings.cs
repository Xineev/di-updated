using System.Drawing;

namespace TagCloudGenerator.Core.Models
{
    public class CanvasSettings
    {
        public Color BackgroundColor { get; set; } = Color.White;
        public Size CanvasSize { get; set; } = new Size(1000, 1000);
        public bool CenterCloud { get; set; } = true;
        public bool ShowRectangles { get; set; } = true;
        public int Padding { get; set; } = 50;

        public CanvasSettings SetWidth(int? width)
        {
            if (width.HasValue && width > 0)
                CanvasSize = new Size(width.Value, CanvasSize.Height);
            return this;
        }

        public CanvasSettings SetHeight(int? height)
        {
            if (height.HasValue && height > 0)
                CanvasSize = new Size(CanvasSize.Width, height.Value);
            return this;
        }

        public CanvasSettings SetSize(int? width, int? height)
        {
            if (width.HasValue && height.HasValue && width > 0 && height > 0)
                CanvasSize = new Size(width.Value, height.Value);
            else if (width.HasValue && width > 0)
                CanvasSize = new Size(width.Value, CanvasSize.Height);
            else if (height.HasValue && height > 0)
                CanvasSize = new Size(CanvasSize.Width, height.Value);

            return this;
        }

        public CanvasSettings SetBackgroundColor(Color? color)
        {
            if (color.HasValue)
                BackgroundColor = color.Value;
            return this;
        }

        public CanvasSettings WithCenterCloud(bool value = true)
        {
            CenterCloud = value;
            return this;
        }

        public CanvasSettings WithShowRectangles(bool value = true)
        {
            ShowRectangles = value;
            return this;
        }

        public CanvasSettings SetPadding(int? padding)
        {
            if (padding.HasValue && padding >= 0)
                Padding = (int)padding;
            return this;
        }
    }
}
