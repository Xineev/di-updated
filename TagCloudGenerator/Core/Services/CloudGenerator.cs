using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Core.Services
{
    public class CloudGenerator : ITagCloudGenerator
    {
        private readonly ITagCloudAlgorithm _algorithm;
        private readonly IReader _reader;
        private readonly IEnumerable<IFilter> _filters;
        private readonly IAnalyzer _analyzer;
        private readonly IRenderer _renderer;
        private readonly IFontSizeCalculator _fontSizeCalculator;
        private readonly ITextMeasurer _textMeasurer;
        private readonly Point _center;

        public CloudGenerator(ITagCloudAlgorithm algorithm, 
            IReader reader, 
            IEnumerable<IFilter> filters, 
            IAnalyzer analyzer, 
            IRenderer renderer,
            IFontSizeCalculator fontSizeCalculator,
            ITextMeasurer textMeasurer)
        {
            _algorithm = algorithm;
            _reader = reader;
            _filters = filters;
            _analyzer = analyzer;
            _renderer = renderer;
            _fontSizeCalculator = fontSizeCalculator;
            _textMeasurer = textMeasurer;
            _center = new Point(0, 0);
        }

        public void Generate(string inputFile, string outputFile, RenderSettings renderSettings)
        {
            var words = _reader.TryRead(inputFile);
            if (words == null || !words.Any())
            {
                return;
            }

            words = ApplyFilters(words);
            if (!words.Any())
            {
                return;
            }

            var cloudItems = _analyzer.Analyze(words).ToList();

            var preparedItems = PrepareCloudItems(cloudItems, renderSettings).ToList();

            var arrangedItems = ArrangeCloudItems(preparedItems).ToList();

            renderSettings.OutputPath = outputFile;

            _renderer.Render(arrangedItems, renderSettings);
        }

        private IEnumerable<string> ApplyFilters(IEnumerable<string> words)
        {
            var filteredWords = words;
            foreach (var filter in _filters)
            {
                filteredWords = filter.Filter(filteredWords);
            }
            return filteredWords;
        }

        private IEnumerable<CloudItem> PrepareCloudItems(IEnumerable<CloudItem> items, RenderSettings settings)
        {
            var itemsList = items.ToList();
            var frequencies = itemsList.Select(i => i.Frequency).ToList();
            var minFrequency = frequencies.Min();
            var maxFrequency = frequencies.Max();

            for (int i = 0; i < itemsList.Count; i++)
            {
                var item = itemsList[i];

                var fontSize = _fontSizeCalculator.Calculate(
                    item.Frequency,
                    minFrequency,
                    maxFrequency,
                    settings.MinFontSize,
                    settings.MaxFontSize);

                var textSize = _textMeasurer.Measure(
                    item.Word,
                    fontSize,
                    settings.FontFamily);

                yield return new CloudItem(
                    word: item.Word,
                    rectangle: new Rectangle(Point.Empty, textSize),
                    fontSize: fontSize,
                    color: settings.TextColor,
                    fontFamily: settings.FontFamily,
                    frequency: item.Frequency,
                    weight: item.Weight
                );
            }
        }

        private IEnumerable<CloudItem> ArrangeCloudItems(IEnumerable<CloudItem> items)
        {

            foreach (var item in items)
            {
               var newRectangle = _algorithm.PutNextRectangle(item.Rectangle.Size);
                yield return item.WithRectangle(newRectangle);
            }
        }
    }
}
