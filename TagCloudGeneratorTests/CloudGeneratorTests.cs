using Moq;
using NUnit.Framework;
using System.Drawing;
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
        public void Generate_EmptyFile_DoesNothing_Test()
        {
            readerMock.Setup(r => r.TryRead(It.IsAny<string>())).Returns(new List<string>());

            cloudGenerator.Generate("input.txt", "output.png", new CanvasSettings(), new TextSettings());

            rendererMock.Verify(r => r.Render(
                It.IsAny<IEnumerable<CloudItem>>(),
                It.IsAny<CanvasSettings>(),
                It.IsAny<TextSettings>(),
                It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Generate_AllWordsFilteredOut_DoesNothing_Test()
        {
            var words = new List<string> { "in", "a", "for" };
            readerMock.Setup(r => r.TryRead(It.IsAny<string>())).Returns(words);
            filterMock.Setup(f => f.Filter(It.IsAny<List<string>>())).Returns(new List<string>());

            cloudGenerator.Generate("input.txt", "output.png", new CanvasSettings(), new TextSettings());

            rendererMock.Verify(r => r.Render(
                It.IsAny<IEnumerable<CloudItem>>(),
                It.IsAny<CanvasSettings>(),
                It.IsAny<TextSettings>(),
                It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Generate_NormalFlow_CallsAllDependencies_Test()
        {
            var words = new List<string> { "hello", "world", "test" };
            var filteredWords = new List<string> { "hello", "world", "test" };
            var cloudItems = new List<CloudItem>
            {
                new CloudItem("hello", Rectangle.Empty, 0, frequency: 2),
                new CloudItem("world", Rectangle.Empty, 0, frequency: 1)
            };

            readerMock.Setup(r => r.TryRead("input.txt")).Returns(words);
            filterMock.Setup(f => f.Filter(words)).Returns(filteredWords);
            analyzerMock.Setup(a => a.Analyze(filteredWords)).Returns(cloudItems);

            var textSettings = new TextSettings
            {
                FontFamily = "Arial",
                MinFontSize = 12f,
                MaxFontSize = 72f,
                TextColor = Color.Black
            };

            fontSizeCalculatorMock.Setup(f => f.Calculate(2, 1, 2, 12f, 72f)).Returns(50f);
            fontSizeCalculatorMock.Setup(f => f.Calculate(1, 1, 2, 12f, 72f)).Returns(20f);

            textMeasurerMock.Setup(t => t.Measure("hello", 50f, "Arial")).Returns(new Size(100, 30));
            textMeasurerMock.Setup(t => t.Measure("world", 20f, "Arial")).Returns(new Size(80, 25));

            algorithmMock.Setup(a => a.PutNextRectangle(new Size(100, 30))).Returns(new Rectangle(0, 0, 100, 30));
            algorithmMock.Setup(a => a.PutNextRectangle(new Size(80, 25))).Returns(new Rectangle(100, 0, 80, 25));

            cloudGenerator.Generate("input.txt", "output.png", new CanvasSettings(), textSettings);

            readerMock.Verify(r => r.TryRead("input.txt"), Times.Once);
            filterMock.Verify(f => f.Filter(words), Times.Once);
            analyzerMock.Verify(a => a.Analyze(filteredWords), Times.Once);

            fontSizeCalculatorMock.Verify(f =>
                f.Calculate(It.IsAny<int>(), 1, 2, 12f, 72f), Times.Exactly(2));
            textMeasurerMock.Verify(t =>
                t.Measure(It.IsAny<string>(), It.IsAny<float>(), "Arial"), Times.Exactly(2));
            algorithmMock.Verify(a => a.PutNextRectangle(It.IsAny<Size>()), Times.Exactly(2));

            rendererMock.Verify(r => r.Render(
                It.Is<IEnumerable<CloudItem>>(items => items.Count() == 2),
                It.IsAny<CanvasSettings>(),
                It.Is<TextSettings>(ts =>
                    ts.FontFamily == "Arial" &&
                    ts.MinFontSize == 12f &&
                    ts.MaxFontSize == 72f),
                "output.png"), Times.Once);
        }
    }
}
