using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IAnalyzer
    {
        IEnumerable<CloudItem> Analyze(IEnumerable<string> words);
    }
}
