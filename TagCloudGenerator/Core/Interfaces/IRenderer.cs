using System.Drawing;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IRenderer
    {
        public Bitmap Render(IEnumerable<CloudItem> items, CanvasSettings canvasSettings, TextSettings textSettings);
    }
}
