using System.Drawing;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Infrastructure.Analyzers
{
    public class WordsFrequencyAnalyzer : IAnalyzer
    {
        public List<(string Word, int Frequency)> Analyze(List<string> words)
        {
            var wordFreqDictionary = new Dictionary<string, int>();

            if (words == null || words.Count == 0)
                return new List<(string word, int freq)>();

            foreach (string word in words) 
            {
                if (!wordFreqDictionary.ContainsKey(word)) wordFreqDictionary.Add(word, 1);
                else wordFreqDictionary[word]++;
            }

            var result = wordFreqDictionary
                 .OrderByDescending(pair => pair.Value)
                 .ThenBy(pair => pair.Key)
                 .Select(pair => (pair.Key, pair.Value))
                 .ToList();

            return result;
        }
    }
}
