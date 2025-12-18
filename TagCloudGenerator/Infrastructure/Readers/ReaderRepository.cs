using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Readers
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly IEnumerable<IFormatReader> _readers;

        public ReaderRepository(IEnumerable<IFormatReader> readers)
        {
            _readers = readers;
        }

        public bool CanRead(string filePath)
        {
            return _readers.Any(r => r.CanRead(filePath));
        }

        public IFormatReader TryGetReader(string filePath)
        {
            var reader = _readers.FirstOrDefault(r => r.CanRead(filePath));
            if (reader == null)
                throw new NotSupportedException("Формат не поддерживается");

            return reader;
        }
    }
}