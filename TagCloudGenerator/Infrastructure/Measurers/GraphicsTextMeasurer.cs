using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Measurers
{
    public class GraphicsTextMeasurer : ITextMeasurer
    {
        private readonly string _fontFamily;

        public GraphicsTextMeasurer(string fontFamily = "Arial")
        {
            _fontFamily = fontFamily;
        }

        public Size Measure(string word, float fontSize, string fontFamily)
        {
            using var font = new Font(fontFamily, fontSize);
            using var bitmap = new Bitmap(1, 1);
            using var graphics = Graphics.FromImage(bitmap);

            var size = graphics.MeasureString(word, font);
            return new Size((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
        }
    }
}
