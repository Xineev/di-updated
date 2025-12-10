using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Core.Services;


namespace TagCloudGeneratorTests
{
    public class CloudGeneratorTests
    {
        private Mock<ITagCloudAlgorithm> algorithmMock;
        private Mock<IReader> readerMock;
        private Mock<IFilter> filterMock;
        private Mock<IAnalyzer> analyzerMock;
        private Mock<IRenderer> rendererMock;
        private Mock<IFontSizeCalculator> fontSizeCalculatorMock;
        private Mock<ITextMeasurer> textMeasurerMock;
        private CloudGenerator cloudGenerator;

        [SetUp]
        public void Setup()
        {
            algorithmMock = new Mock<ITagCloudAlgorithm>();
            readerMock = new Mock<IReader>();
            filterMock = new Mock<IFilter>();
            analyzerMock = new Mock<IAnalyzer>();
            rendererMock = new Mock<IRenderer>();
            fontSizeCalculatorMock = new Mock<IFontSizeCalculator>();
            textMeasurerMock = new Mock<ITextMeasurer>();

            cloudGenerator = new CloudGenerator(
                algorithmMock.Object,
                readerMock.Object,
                new[] { filterMock.Object },
                analyzerMock.Object,
                rendererMock.Object,
                fontSizeCalculatorMock.Object,
                textMeasurerMock.Object);
        }

        [Test]
        public void Generate_EmptyFile_DoesNothing()
        {
            var words = Enumerable.Empty<string>();
            readerMock.Setup(r => r.TryRead(It.IsAny<string>())).Returns(words);

            cloudGenerator.Generate("input.txt", "output.png", new RenderSettings());

            rendererMock.Verify(r => r.Render(It.IsAny<IEnumerable<CloudItem>>(), It.IsAny<RenderSettings>()), Times.Never);
        }

        [Test]
        public void Generate_AllWordsFilteredOut_DoesNothing()
        {
            var words = new[] { "in", "a", "for" };
            readerMock.Setup(r => r.TryRead(It.IsAny<string>())).Returns(words);
            filterMock.Setup(f => f.Filter(It.IsAny<IEnumerable<string>>())).Returns(Enumerable.Empty<string>());

            cloudGenerator.Generate("input.txt", "output.png", new RenderSettings());

            rendererMock.Verify(r => r.Render(It.IsAny<IEnumerable<CloudItem>>(), It.IsAny<RenderSettings>()), Times.Never);
        }

        [Test]
        public void Generate_NormalFlow_CallsAllDependencies()
        {
            var words = new[] { "hello", "world", "test" };
            var filteredWords = new[] { "hello", "world", "test" };
            var cloudItems = new[]
            {
                new CloudItem("hello", Rectangle.Empty, 0, frequency: 2),
                new CloudItem("world", Rectangle.Empty, 0, frequency: 1)
            };
            var renderSettings = new RenderSettings
            {
                MinFontSize = 12f,
                MaxFontSize = 72f,
                FontFamily = "Arial"
            };

            readerMock.Setup(r => r.TryRead("input.txt")).Returns(words);
            filterMock.Setup(f => f.Filter(words)).Returns(filteredWords);
            analyzerMock.Setup(a => a.Analyze(filteredWords)).Returns(cloudItems);

            fontSizeCalculatorMock.Setup(f => f.Calculate(2, 1, 2, 12f, 72f)).Returns(50f);
            fontSizeCalculatorMock.Setup(f => f.Calculate(1, 1, 2, 12f, 72f)).Returns(20f);

            textMeasurerMock.Setup(t => t.Measure("hello", 50f, "Arial")).Returns(new Size(100, 30));
            textMeasurerMock.Setup(t => t.Measure("world", 20f, "Arial")).Returns(new Size(80, 25));

            algorithmMock.Setup(a => a.PutNextRectangle(new Size(100, 30))).Returns(new Rectangle(0, 0, 100, 30));
            algorithmMock.Setup(a => a.PutNextRectangle(new Size(80, 25))).Returns(new Rectangle(100, 0, 80, 25));

            cloudGenerator.Generate("input.txt", "output.png", renderSettings);

            readerMock.Verify(r => r.TryRead("input.txt"), Times.Once);
            filterMock.Verify(f => f.Filter(words), Times.Once);
            analyzerMock.Verify(a => a.Analyze(filteredWords), Times.Once);
            fontSizeCalculatorMock.Verify(f => f.Calculate(It.IsAny<int>(), 1, 2, 12f, 72f), Times.Exactly(2));
            textMeasurerMock.Verify(t => t.Measure(It.IsAny<string>(), It.IsAny<float>(), "Arial"), Times.Exactly(2));
            algorithmMock.Verify(a => a.PutNextRectangle(It.IsAny<Size>()), Times.Exactly(2));
            rendererMock.Verify(r => r.Render(It.IsAny<IEnumerable<CloudItem>>(), It.Is<RenderSettings>(s => s.OutputPath == "output.png")), Times.Once);
        }
    }
}
