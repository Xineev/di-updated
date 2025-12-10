using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Calculators
{
    public class LinearFontSizeCalculator : IFontSizeCalculator
    {
        public float Calculate(int wordFrequency, int minFrequency, int maxFrequency, float minFontSize, float maxFontSize)
        {
            if (minFrequency == maxFrequency)
                return (minFontSize + maxFontSize) / 2;

            float frequencyRatio = (float)(wordFrequency - minFrequency) / (maxFrequency - minFrequency);
            return minFontSize + frequencyRatio * (maxFontSize - minFontSize);
        }
    }
}
