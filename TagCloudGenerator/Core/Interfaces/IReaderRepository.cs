
namespace TagCloudGenerator.Core.Interfaces
{
    public interface IReaderRepository
    {
        public IFormatReader TryGetReader(string filePath);
    }
}
