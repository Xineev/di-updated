using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IFilter
    {
        IEnumerable<string> Filter(IEnumerable<string> words);
        bool ShouldInclude(string word);
    }
}
