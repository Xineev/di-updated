using System.Drawing;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Infrastructure.Analyzers
{
    public class WordsFrequencyAnalyzer : IAnalyzer
    {
        public List<CloudItem> Analyze(List<string> words)
        {
            if (words == null || words.Count == 0)
                return new List<CloudItem>();

            var wordGroups = words
                .GroupBy(word => word)
                .Select(group => new { Word = group.Key, Frequency = group.Count() })
                .OrderByDescending(x => x.Frequency);

            if (wordGroups.Count() == 0)
                return new List<CloudItem>();

            var maxFreq = wordGroups.Max(g => g.Frequency);

            return wordGroups.Select(group => new CloudItem(
                word: group.Word,
                rectangle: Rectangle.Empty,
                fontSize: 0,
                frequency: group.Frequency,
                weight: (double)group.Frequency / maxFreq
                )).ToList();
        }
    }
}
