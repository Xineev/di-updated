using NUnit.Framework;
using TagCloudGenerator.Infrastructure.Analyzers;

namespace TagCloudGeneratorTests
{
    public class WordsFrequencyAnalyzerTests
    {
        private WordsFrequencyAnalyzer frequencyAnalyzer  = new WordsFrequencyAnalyzer();

        [Test]
        public void Analyze_EmptyInput_ReturnsEmpty_Test()
        {
            var words = new List<string>();
            var result = frequencyAnalyzer.Analyze(words);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Analyze_SingleWord_ReturnsOneItemWithFrequencyOne_Test()
        {
            var words = new List<string> { "hello" };
            var result = frequencyAnalyzer.Analyze(words).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Word, Is.EqualTo("hello"));
            Assert.That(result[0].Frequency, Is.EqualTo(1));
        }

        [Test]
        public void Analyze_MultipleWords_ReturnsCorrectFrequencies_Test()
        {
            var words = new List<string> { "hello", "world", "hello", "test", "world", "hello" };
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
        public void Analyze_AllWordsEqualFrequency_ReturnsCorrectWeight_Test()
        {
            var words = new List<string> { "a", "b", "c", "d" };
            var result = frequencyAnalyzer.Analyze(words).ToList();

            Assert.That(result.Count, Is.EqualTo(4));
            foreach (var item in result)
            {
                Assert.That(item.Weight, Is.EqualTo(1.0).Within(0.001));
            }
        }

        [Test]
        public void Analyze_DifferentFrequencies_ReturnsCorrectWeights_Test()
        {
            var words = new List<string> { "a", "a", "a", "b", "b", "c" };
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
        public void Analyze_CaseSensitive_ReturnsSeparateItems_Test()
        {
            var words = new List<string> { "Hello", "hello", "HELLO" };
            var result = frequencyAnalyzer.Analyze(words).ToList();
            Assert.That(result.Count, Is.EqualTo(3));
        }
    }
}