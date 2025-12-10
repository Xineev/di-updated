using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IFontSizeCalculator
    {
        float Calculate(int wordFrequency, int minFrequency, int maxFrequency, float minFontSize, float maxFontSize);
    }
}
