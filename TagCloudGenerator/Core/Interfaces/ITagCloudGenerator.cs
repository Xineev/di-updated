using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ITagCloudGenerator
    {
        public void Generate(string inputFile, string outputFile, CanvasSettings canvasSettings, TextSettings textSettings);
    }
}
