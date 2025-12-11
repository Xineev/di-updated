using System.Drawing;

namespace TagCloudGenerator.Core.Models
{
    public class TextSettings
    {
        public string FontFamily { get; set; } = "Arial";
        public float MinFontSize { get; set; } = 12f;
        public float MaxFontSize { get; set; } = 72f;
        public Color TextColor { get; set; } = Color.Black;

        public TextSettings SetFontFamily(string? font)
        {
            if (!string.IsNullOrWhiteSpace(font))
                FontFamily = font;
            return this;
        }

        public TextSettings SetMinFontSize(float? size)
        {
            if (size.HasValue && size.Value >= 0)
                MinFontSize = size.Value;
            return this;
        }

        public TextSettings SetMaxFontSize(float? size)
        {
            if (size.HasValue && size.Value >= 0)
                MaxFontSize = size.Value;
            return this;
        }

        public TextSettings SetFontSizeRange(float? minSize, float? maxSize)
        {
            SetMinFontSize(minSize);
            SetMaxFontSize(maxSize);
            return this;
        }

        public TextSettings SetTextColor(Color? color)
        {
            if (color.HasValue)
                TextColor = color.Value;
            return this;
        }
    }
}
