using Moq;
using NUnit.Framework;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Infrastructure.Readers;

namespace TagCloudGeneratorTests
{
    public class ReaderRepositoryTests
    {
        private Mock<IFormatReader> firstReaderMock;
        private Mock<IFormatReader> reader2Mock;
        private ReaderRepository readerRepository;

        [SetUp]
        public void Setup()
        {
            firstReaderMock = new Mock<IFormatReader>();
            reader2Mock = new Mock<IFormatReader>();

            readerRepository = new ReaderRepository(
                new[] { firstReaderMock.Object, reader2Mock.Object });
        }

        [Test]
        public void CanRead_WhenAnyReaderCanRead_ReturnsTrue()
        {
            firstReaderMock.Setup(r => r.CanRead("file.docx")).Returns(false);
            reader2Mock.Setup(r => r.CanRead("file.docx")).Returns(true);

            var result = readerRepository.CanRead("file.docx");

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanRead_WhenNoReaderCanRead_ReturnsFalse()
        {
            firstReaderMock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);
            reader2Mock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);

            var result = readerRepository.CanRead("file.unknown");

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryRead_UsesFirstMatchingReader()
        {
            var expected = new List<string> { "word1", "word2" };

            firstReaderMock.Setup(r => r.CanRead("file.docx")).Returns(false);
            reader2Mock.Setup(r => r.CanRead("file.docx")).Returns(true);
            reader2Mock.Setup(r => r.TryRead("file.docx")).Returns(expected);

            var reader = readerRepository.TryGetReader("file.docx");
            var result = reader.TryRead("file.docx");

            Assert.That(result, Is.EqualTo(expected));

            reader2Mock.Verify(r => r.TryRead("file.docx"), Times.Once);
            firstReaderMock.Verify(r => r.TryRead(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void TryRead_WhenNoReaderFound_ThrowsException()
        {
            firstReaderMock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);
            reader2Mock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);

            Assert.Throws<NotSupportedException>(() =>
                readerRepository.TryGetReader("file.xyz"));
        }
    }
}
