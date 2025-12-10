using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IRenderSettings
    {
        string OutputPath { get; set; }
        string FontFamily { get; set; }
        float MinFontSize { get; set; }
        float MaxFontSize { get; set; }
        Color BackgroundColor { get; set; }
        Color TextColor { get; set; }
        Size CanvasSize { get; set; }
        bool CenterCloud { get; set; }
        bool ShowRectangles { get; set; }
        int Padding { get; set; }
    }
}
