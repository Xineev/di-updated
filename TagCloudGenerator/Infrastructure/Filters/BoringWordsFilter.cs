using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Filters
{
    public class BoringWordsFilter : IFilter
    {
        private readonly string[] _boringWords = ["in", "it", "a", "as", "for", "of", "on"];
        public List<string> Filter(List<string> words)
        {
            if (words == null || words.Count() == 0) return new List<string>();
            return words.Where(w => ShouldInclude(w)).ToList();
        }

        public bool ShouldInclude(string word)
        {
            return !_boringWords.Contains(word);
        }
    }
}
