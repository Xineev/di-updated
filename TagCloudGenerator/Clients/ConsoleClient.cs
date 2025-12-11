using CommandLine;
using System.Drawing;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Clients
{
    public class ConsoleClient : IClient
    {
        private readonly ITagCloudGenerator _generator;

        public ConsoleClient(ITagCloudGenerator generator)
        {
            _generator = generator;
        }

        public void Run(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunWithOptions)
                .WithNotParsed(errors => { });
        }

        private void RunWithOptions(Options opts)
        {
            if (!File.Exists(opts.InputFile))
            {
                Console.WriteLine($"Error: input file '{opts.InputFile}' does not exist.");
                return;
            }

            var canvasSettings = new CanvasSettings()
                .SetSize(opts.Width, opts.Height)
                .SetBackgroundColor(TryParseColor(opts.BackgroundColor))
                .WithCenterCloud()
                .WithShowRectangles()
                .SetPadding(opts.Padding);

            var textSettings = new TextSettings()
                .SetFontFamily(opts.FontFamily)
                .SetFontSizeRange(opts.MinFontSize, opts.MaxFontSize)
                .SetTextColor(TryParseColor(opts.TextColor));

            string inputFile = opts.InputFile;
            string outputFile = opts.OutputFile;

            Console.WriteLine("Starting tag cloud generation...");
            Console.WriteLine($"Input file: {inputFile}");
            Console.WriteLine($"Output file: {outputFile}");

            try
            {
                _generator.Generate(inputFile, outputFile, canvasSettings, textSettings);
                Console.WriteLine("Tag cloud generation completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during generation: {ex.Message}");
            }
        }

        private static Color? TryParseColor(string? colorStr)
        {
            if (string.IsNullOrWhiteSpace(colorStr))
                return null;

            try
            {
                var known = Color.FromName(colorStr);
                if (known.IsKnownColor)
                    return known;

                return ColorTranslator.FromHtml(colorStr);
            }
            catch
            {
                Console.WriteLine($"Color '{colorStr}' could not be parsed. Default color will be used.");
                return null;
            }
        }
    }
}