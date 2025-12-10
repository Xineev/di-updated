using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Filters
{
    public class BoringWordsFilter : IFilter
    {
        private readonly string[] _boringWords = ["in", "it", "a", "as", "for", "of", "on"];
        public IEnumerable<string> Filter(IEnumerable<string> words)
        {
            return words.Where(w => ShouldInclude(w));
        }

        public bool ShouldInclude(string word)
        {
            return !_boringWords.Contains(word);
        }
    }
}
