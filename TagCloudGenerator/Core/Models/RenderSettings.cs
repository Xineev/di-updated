using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Core.Models
{
    public class RenderSettings : IRenderSettings
    {
        public string OutputPath { get; set; }
        public string FontFamily { get; set; } = "Arial";
        public float MinFontSize { get; set; } = 12f;
        public float MaxFontSize { get; set; } = 72f;
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public Size CanvasSize { get; set; }
        public bool CenterCloud { get; set; }
        public bool ShowRectangles { get; set; }
        public int Padding { get; set; } = 50;
    }
}
