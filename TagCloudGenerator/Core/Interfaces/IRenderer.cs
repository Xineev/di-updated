using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IRenderer
    {
        void Render(IEnumerable<CloudItem> items, CanvasSettings canvasSettings, TextSettings textSettings, string outputFile);
    }
}
