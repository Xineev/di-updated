using Moq;
using NUnit.Framework;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Infrastructure.Readers;

namespace TagCloudGeneratorTests
{
    public class CompositeReaderTests
    {
        private Mock<IFormatReader> reader1Mock;
        private Mock<IFormatReader> reader2Mock;
        private CompositeReader compositeReader;

        [SetUp]
        public void Setup()
        {
            reader1Mock = new Mock<IFormatReader>();
            reader2Mock = new Mock<IFormatReader>();

            compositeReader = new CompositeReader(
                new[] { reader1Mock.Object, reader2Mock.Object });
        }

        [Test]
        public void CanRead_WhenAnyReaderCanRead_ReturnsTrue()
        {
            reader1Mock.Setup(r => r.CanRead("file.docx")).Returns(false);
            reader2Mock.Setup(r => r.CanRead("file.docx")).Returns(true);

            var result = compositeReader.CanRead("file.docx");

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanRead_WhenNoReaderCanRead_ReturnsFalse()
        {
            reader1Mock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);
            reader2Mock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);

            var result = compositeReader.CanRead("file.unknown");

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryRead_UsesFirstMatchingReader()
        {
            var expected = new List<string> { "word1", "word2" };

            reader1Mock.Setup(r => r.CanRead("file.docx")).Returns(false);
            reader2Mock.Setup(r => r.CanRead("file.docx")).Returns(true);
            reader2Mock.Setup(r => r.TryRead("file.docx")).Returns(expected);

            var result = compositeReader.TryRead("file.docx");

            Assert.That(result, Is.EqualTo(expected));

            reader2Mock.Verify(r => r.TryRead("file.docx"), Times.Once);
            reader1Mock.Verify(r => r.TryRead(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void TryRead_WhenNoReaderFound_ThrowsException()
        {
            reader1Mock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);
            reader2Mock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);

            Assert.Throws<NotSupportedException>(() =>
                compositeReader.TryRead("file.xyz"));
        }
    }
}
