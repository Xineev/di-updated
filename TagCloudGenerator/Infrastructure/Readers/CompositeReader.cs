using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Readers
{
    public class CompositeReader : IReader
    {
        private readonly IEnumerable<IFormatReader> _readers;

        public CompositeReader(IEnumerable<IFormatReader> readers)
        {
            _readers = readers;
        }

        public bool CanRead(string filePath)
        {
            return _readers.Any(r => r.CanRead(filePath));
        }

        public List<string> TryRead(string filePath)
        {
            var reader = _readers.FirstOrDefault(r => r.CanRead(filePath));
            if (reader == null)
                throw new NotSupportedException("Формат не поддерживается");

            return reader.TryRead(filePath);
        }
    }
}