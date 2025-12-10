using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ICloudItem
    {
        string Word { get; }
        Rectangle Rectangle { get; }
        float FontSize { get; }
        Color? Color { get; }
        string FontFamily { get; }
        FontStyle FontStyle { get; }
        int Frequency { get; }
        double Weight { get; }
    }
}
