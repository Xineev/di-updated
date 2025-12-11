using System.Drawing;
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

        public void Generate(string inputFile, string outputFile, CanvasSettings canvasSettings, TextSettings textSettings)
        {
            var words = _reader.TryRead(inputFile).ToList();
            
            if (words == null || words.Count == 0)
                return;

            words = ApplyFilters(words);
            if(words.Count == 0)
                return;

            var cloudItems = _analyzer.Analyze(words).ToList();

            var preparedItems = PrepareCloudItems(cloudItems, textSettings).ToList();

            var arrangedItems = ArrangeCloudItems(preparedItems).ToList();

            _renderer.Render(arrangedItems, canvasSettings, textSettings, outputFile);
        }

        private List<string> ApplyFilters(List<string> words)
        {
            var filteredWords = words;
            foreach (var filter in _filters)
            {
                filteredWords = filter.Filter(filteredWords);
            }
            return filteredWords;
        }

        private IEnumerable<CloudItem> PrepareCloudItems(IEnumerable<CloudItem> items, TextSettings settings)
        {
            var itemsList = new List<CloudItem>();

            var minFrequency = items.Min(i => i.Frequency);
            var maxFrequency = items.Max(i => i.Frequency);

            foreach (var item in items)
            {
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

                itemsList.Add(
                    new CloudItem(
                        word: item.Word,
                        rectangle: new Rectangle(Point.Empty, textSize),
                        fontSize: fontSize,
                        color: settings.TextColor,
                        fontFamily: settings.FontFamily,
                        frequency: item.Frequency,
                        weight: item.Weight
                        ));
            }
            return itemsList;
        }

        private List<CloudItem> ArrangeCloudItems(List<CloudItem> items)
        {
            return items.Select(item => item.WithRectangle(_algorithm.PutNextRectangle(item.Rectangle.Size)))
                .ToList();
        }
    }
}
