using System.Drawing;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Infrastructure.Analyzers
{
    public class WordsFrequencyAnalyzer : IAnalyzer
    {
        public Dictionary<string, int> Analyze(List<string> words)
        {
            var wordFreqDictionary = new Dictionary<string, int>();

            if (words == null || words.Count == 0)
                return new Dictionary<string, int>();

            foreach (string word in words) 
            {
                if (!wordFreqDictionary.ContainsKey(word)) wordFreqDictionary.Add(word, 1);
                else wordFreqDictionary[word]++;
            }

            return wordFreqDictionary;
        }
    }
}
