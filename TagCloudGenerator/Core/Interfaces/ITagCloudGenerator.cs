using System.Drawing;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ITagCloudGenerator
    {
        public Bitmap? Generate(List<string> words, CanvasSettings canvasSettings, TextSettings textSettings, IEnumerable<IFilter> filters);
    }
}
