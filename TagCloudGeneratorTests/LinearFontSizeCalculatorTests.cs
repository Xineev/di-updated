using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TagCloudGenerator.Infrastructure.Calculators;

namespace TagCloudGeneratorTests
{
    public class LinearFontSizeCalculatorTests
    {
        private LinearFontSizeCalculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = new LinearFontSizeCalculator();
        }

        [Test]
        public void Calculate_MinFrequency_ReturnsMinFontSize()
        {
            var result = calculator.Calculate(1, 1, 10, 12f, 72f);
            Assert.That(result, Is.EqualTo(12f));
        }

        [Test]
        public void Calculate_MaxFrequency_ReturnsMaxFontSize()
        {
            var result = calculator.Calculate(10, 1, 10, 12f, 72f);
            Assert.That(result, Is.EqualTo(72f));
        }

        [Test]
        public void Calculate_MiddleFrequency_ReturnsProportionalSize()
        {
            var result = calculator.Calculate(5, 1, 9, 10f, 50f);
            Assert.That(result, Is.EqualTo(30f).Within(0.001f));
        }

        [Test]
        public void Calculate_SameMinMaxFrequency_ReturnsAverage()
        {
            var result = calculator.Calculate(5, 5, 5, 12f, 72f);
            Assert.That(result, Is.EqualTo(42f));
        }

        [Test]
        public void Calculate_FrequencyBelowMin_ReturnsLessThanMinFontSize()
        {
            var result = calculator.Calculate(0, 1, 10, 12f, 72f);
            Assert.That(result, Is.EqualTo(5.333f).Within(0.001f));
        }

        [Test]
        public void Calculate_FrequencyAboveMax_ReturnsMoreThanMaxFontSize()
        {
            var result = calculator.Calculate(15, 1, 10, 12f, 72f);
            Assert.That(result, Is.EqualTo(105.333f).Within(0.001f));
        }

        [Test]
        public void Calculate_ZeroRangeFontSizes_ReturnsMinFontSize()
        {
            var result = calculator.Calculate(5, 1, 10, 20f, 20f);
            Assert.That(result, Is.EqualTo(20f));
        }

        [Test]
        [TestCase(1, 10f)]
        [TestCase(3, 20f)]
        [TestCase(5, 30f)]
        [TestCase(7, 40f)]
        [TestCase(9, 50f)]
        public void Calculate_MultipleTestCases_ReturnsCorrectValues(int frequency, float expected)
        {
            var result = calculator.Calculate(frequency, 1, 9, 10f, 50f);
            Assert.That(result, Is.EqualTo(expected).Within(0.001f));
        }
    }
}
