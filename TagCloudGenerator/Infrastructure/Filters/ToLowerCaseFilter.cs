using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Filters
{
    public class ToLowerCaseFilter : IFilter
    {
        public List<string> Filter(List<string> words)
        {
            if (words == null || words.Count() == 0) return new List<string>();

            return words.Select(w => w.ToLower()).ToList();
        }
    }
}
