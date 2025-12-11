using System.Drawing;

namespace TagCloudGenerator.Core.Models
{
    public class CloudItem
    {
        public string Word { get; }
        public Rectangle Rectangle { get; }
        public float FontSize { get; }
        public Color? Color { get; }
        public string FontFamily { get; }
        public FontStyle FontStyle { get; }
        public int Frequency { get; }
        public double Weight { get; }

        public CloudItem(
            string word,
            Rectangle rectangle,
            float fontSize,
            Color? color = null,
            string fontFamily = "Arial",
            FontStyle fontStyle = FontStyle.Regular,
            int frequency = 1,
            double weight = 1.0)
        {
            Word = word ?? throw new ArgumentNullException(nameof(word));
            Rectangle = rectangle;
            FontSize = fontSize;
            Color = color;
            FontFamily = fontFamily ?? throw new ArgumentNullException(nameof(fontFamily));
            FontStyle = fontStyle;
            Frequency = frequency;
            Weight = weight;
        }

        public CloudItem WithRectangle(Rectangle newRectangle)
        {
            return new CloudItem(Word, newRectangle, FontSize, Color, FontFamily, FontStyle, Frequency, Weight);
        }

        public CloudItem WithFontSize(float newFontSize)
        {
            return new CloudItem(Word, Rectangle, newFontSize, Color, FontFamily, FontStyle, Frequency, Weight);
        }

        public CloudItem WithColor(Color newColor)
        {
            return new CloudItem(Word, Rectangle, FontSize, newColor, FontFamily, FontStyle, Frequency, Weight);
        }
    }
}
