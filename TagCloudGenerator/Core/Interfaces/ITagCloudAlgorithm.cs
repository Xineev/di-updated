using System.Drawing;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ITagCloudAlgorithm
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}
