using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IAnalyzer
    {
        List<(string Word, int Frequency)> Analyze(List<string> words);
    }
}
