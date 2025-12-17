using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Filters
{
    public class BoringWordsFilter : IFilter
    {
        private string[] boringWords = ["in", "it", "a", "as", "for", "of", "on"];

        public BoringWordsFilter() { }

        public BoringWordsFilter(string[] words) 
        {
            boringWords = words;
        }

        public BoringWordsFilter(List<string> wordsList)
        {
            boringWords = wordsList.ToArray();
        }

        public List<string> Filter(List<string> words)
        {
            if (words == null || words.Count == 0) return new List<string>();

            return words.Where(w => !boringWords.Contains(w)).ToList();
        }

        public bool ShouldInclude(string word)
        {
            return !boringWords.Contains(word);
        }
    }
}
