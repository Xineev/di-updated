using TagCloudGenerator.Infrastructure.Analyzers;
using TagCloudGenerator.Infrastructure.Calculators;
using NUnit.Framework;
using System.Linq;

namespace TagCloudGeneratorTests
{
    public class WordsFrequencyAnalyzerTests
    {
        private WordsFrequencyAnalyzer frequencyAnalyzer;

        [SetUp]
        public void Setup()
        {
            frequencyAnalyzer = new WordsFrequencyAnalyzer();
        }

        [Test]
        public void Analyze_EmptyInput_ReturnsEmpty()
        {
            var words = Enumerable.Empty<string>();
            var result = frequencyAnalyzer.Analyze(words);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Analyze_SingleWord_ReturnsOneItemWithFrequencyOne()
        {
            var words = new[] { "hello" };
            var result = frequencyAnalyzer.Analyze(words).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Word, Is.EqualTo("hello"));
            Assert.That(result[0].Frequency, Is.EqualTo(1));
        }

        [Test]
        public void Analyze_MultipleWords_ReturnsCorrectFrequencies()
        {
            var words = new[] { "hello", "world", "hello", "test", "world", "hello" };
            var result = frequencyAnalyzer.Analyze(words).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Word, Is.EqualTo("hello"));
            Assert.That(result[0].Frequency, Is.EqualTo(3));
            Assert.That(result[1].Word, Is.EqualTo("world"));
            Assert.That(result[1].Frequency, Is.EqualTo(2));
            Assert.That(result[2].Word, Is.EqualTo("test"));
            Assert.That(result[2].Frequency, Is.EqualTo(1));
        }

        [Test]
        public void Analyze_AllWordsEqualFrequency_ReturnsCorrectWeight()
        {
            var words = new[] { "a", "b", "c", "d" };
            var result = frequencyAnalyzer.Analyze(words).ToList();

            Assert.That(result.Count, Is.EqualTo(4));
            foreach (var item in result)
            {
                Assert.That(item.Weight, Is.EqualTo(1.0).Within(0.001));
            }
        }

        [Test]
        public void Analyze_DifferentFrequencies_ReturnsCorrectWeights()
        {
            var words = new[] { "a", "a", "a", "b", "b", "c" };
            var result = frequencyAnalyzer.Analyze(words).ToList();

            var itemA = result.First(i => i.Word == "a");
            var itemB = result.First(i => i.Word == "b");
            var itemC = result.First(i => i.Word == "c");

            Assert.That(itemA.Frequency, Is.EqualTo(3));
            Assert.That(itemB.Frequency, Is.EqualTo(2));
            Assert.That(itemC.Frequency, Is.EqualTo(1));

            Assert.That(itemA.Weight, Is.EqualTo(1.0).Within(0.001));
            Assert.That(itemB.Weight, Is.EqualTo(0.666).Within(0.001));
            Assert.That(itemC.Weight, Is.EqualTo(0.333).Within(0.001));
        }

        [Test]
        public void Analyze_CaseSensitive_ReturnsSeparateItems()
        {
            var words = new[] { "Hello", "hello", "HELLO" };
            var result = frequencyAnalyzer.Analyze(words).ToList();
            Assert.That(result.Count, Is.EqualTo(3));
        }
    }
}