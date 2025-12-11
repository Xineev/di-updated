using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IAnalyzer
    {
        List<CloudItem> Analyze(List<string> words);
    }
}
