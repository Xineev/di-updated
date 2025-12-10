using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Infrastructure.Analyzers
{
    public class WordsFrequencyAnalyzer : IAnalyzer
    {
        public IEnumerable<CloudItem> Analyze(IEnumerable<string> words)
        {
            var wordGroups = words
                .GroupBy(word => word)
                .Select(group => new
                {
                    Word = group.Key,
                    Frequency = group.Count()
                })
                .OrderByDescending(x => x.Frequency);

            foreach (var group in wordGroups)
            {
                yield return new CloudItem(
                    word: group.Word,
                    rectangle: Rectangle.Empty,
                    fontSize: 0,
                    frequency: group.Frequency,
                    weight: (double)group.Frequency / wordGroups.Max(g => g.Frequency)
                );
            }
        }
    }
}
