using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ITagCloudGenerator
    {
        public void Generate(string inputFile, string outputFile, RenderSettings settings);
    }
}
