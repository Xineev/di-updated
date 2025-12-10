using System;
using System.Drawing;
using System.IO;
using System.Linq;
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
            Console.WriteLine("=== Tag Cloud Generator ===");
            Console.WriteLine("Type '--help' to see available commands");
            Console.WriteLine();

            while (true)
            {
                Console.Write("> ");
                var commandLine = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(commandLine))
                    continue;

                if (commandLine == "exit" || commandLine == "quit")
                    break;

                var commandArgs = ParseCommandLine(commandLine);

                if (commandArgs.Length == 0)
                    continue;

                if (commandArgs[0] == "--help" || commandArgs[0] == "-h")
                {
                    PrintHelp();
                    continue;
                }

                var result = ParseArguments(commandArgs);
                if (!result.IsValid)
                {
                    Console.WriteLine("Error: Input file is required!");
                    Console.WriteLine("Usage: <input.txt> [options]");
                    Console.WriteLine("Use --help for more information");
                    continue;
                }

                if (!File.Exists(result.InputFile))
                {
                    Console.WriteLine($"Error: File '{result.InputFile}' not found!");
                    continue;
                }

                Console.WriteLine("\nSettings:");
                PrintSettings(result);

                Console.WriteLine("\nStarting generation...");
                try
                {
                    _generator.Generate(result.InputFile, result.OutputFile, result.RenderSettings);
                    Console.WriteLine($"\nSuccess! Image saved to bin folder of the project as: {result.OutputFile}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                }

                Console.WriteLine();
            }
        }

        private string[] ParseCommandLine(string commandLine)
        {
            var parts = new System.Collections.Generic.List<string>();
            var currentPart = new System.Text.StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < commandLine.Length; i++)
            {
                char c = commandLine[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ' ' && !inQuotes)
                {
                    if (currentPart.Length > 0)
                    {
                        parts.Add(currentPart.ToString());
                        currentPart.Clear();
                    }
                }
                else
                {
                    currentPart.Append(c);
                }
            }

            if (currentPart.Length > 0)
            {
                parts.Add(currentPart.ToString());
            }

            return parts.ToArray();
        }

        private ParseResult ParseArguments(string[] args)
        {
            var result = new ParseResult
            {
                RenderSettings = new RenderSettings
                {
                    FontFamily = "Arial",
                    MinFontSize = 12f,
                    MaxFontSize = 72f,
                    BackgroundColor = Color.White,
                    TextColor = Color.Black,
                    CanvasSize = new Size(800, 600),
                    CenterCloud = true,
                    ShowRectangles = false,
                    Padding = 50
                }
            };

            string inputFile = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (!args[i].StartsWith("-") && !args[i].StartsWith("--"))
                {
                    if (inputFile == null && args[i].EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        inputFile = args[i];
                    }
                }
            }

            if (inputFile == null)
            {
                result.IsValid = false;
                return result;
            }

            result.InputFile = inputFile;
            result.OutputFile = GenerateOutputFilename(inputFile);
            result.IsValid = true;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--font":
                    case "-f":
                        if (i + 1 < args.Length && !IsFlag(args[i + 1]))
                        {
                            result.RenderSettings.FontFamily = args[++i];
                        }
                        break;

                    case "--background":
                    case "-bg":
                        if (i + 1 < args.Length && !IsFlag(args[i + 1]))
                        {
                            result.RenderSettings.BackgroundColor = ParseColor(args[++i], result.RenderSettings.BackgroundColor);
                        }
                        break;

                    case "--text-color":
                    case "-tc":
                        if (i + 1 < args.Length && !IsFlag(args[i + 1]))
                        {
                            result.RenderSettings.TextColor = ParseColor(args[++i], result.RenderSettings.TextColor);
                        }
                        break;

                    case "--width":
                    case "-w":
                        if (i + 1 < args.Length && int.TryParse(args[i + 1], out int width))
                        {
                            result.RenderSettings.CanvasSize = new Size(width, result.RenderSettings.CanvasSize.Height);
                            i++;
                        }
                        break;

                    case "--height":
                        if (i + 1 < args.Length && int.TryParse(args[i + 1], out int height))
                        {
                            result.RenderSettings.CanvasSize = new Size(result.RenderSettings.CanvasSize.Width, height);
                            i++;
                        }
                        break;

                    case "--show-rectangles":
                        result.RenderSettings.ShowRectangles = true;
                        break;

                    case "--output":
                    case "-o":
                        if (i + 1 < args.Length && !IsFlag(args[i + 1]))
                        {
                            result.OutputFile = args[++i];
                            if (!result.OutputFile.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                                result.OutputFile += ".png";
                        }
                        break;
                }
            }

            return result;
        }

        private bool IsFlag(string arg)
        {
            return arg.StartsWith("-") || arg.StartsWith("--");
        }

        private string GenerateOutputFilename(string inputFile)
        {
            string fileName = Path.GetFileNameWithoutExtension(inputFile);
            string directory = Path.Combine(Path.GetDirectoryName(inputFile), "CLouds");
            Directory.CreateDirectory(directory);

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string outputFile = $"{fileName}_cloud_{timestamp}.png";

            if (!string.IsNullOrEmpty(directory))
            {
                outputFile = Path.Combine(directory, outputFile);
            }

            return outputFile;
        }

        private Color ParseColor(string colorString, Color defaultColor)
        {
            try
            {
                var color = Color.FromName(colorString);
                if (color.IsKnownColor)
                    return color;

                if (colorString.StartsWith("#"))
                {
                    return ColorTranslator.FromHtml(colorString);
                }

                if (colorString.Contains(","))
                {
                    var parts = colorString.Split(',');
                    if (parts.Length == 3)
                    {
                        return Color.FromArgb(
                            int.Parse(parts[0].Trim()),
                            int.Parse(parts[1].Trim()),
                            int.Parse(parts[2].Trim()));
                    }
                }

                return defaultColor;
            }
            catch
            {
                return defaultColor;
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine("=== Tag Cloud Generator Help ===");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  <input.txt> [options]");
            Console.WriteLine("  --help");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("  <input.txt>               Input text file (required)");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -f, --font <name>         Font family (default: Arial)");
            Console.WriteLine("  -bg, --background <color> Background color (default: White)");
            Console.WriteLine("  -tc, --text-color <color> Text color (default: Black)");
            Console.WriteLine("  -w, --width <pixels>      Canvas width (default: 800)");
            Console.WriteLine("      --height <pixels>     Canvas height (default: 600)");
            Console.WriteLine("  -o, --output <file>       Output file (default: <input>_cloud_<timestamp>.png)");
            Console.WriteLine("      --show-rectangles     Show word rectangles (debug mode)");
            Console.WriteLine("      --help                Show this help message");
            Console.WriteLine();
            Console.WriteLine("Color formats:");
            Console.WriteLine("  - Named colors: White, Black, Red, Blue, Green, etc.");
            Console.WriteLine("  - HEX: #FFFFFF, #000000, #FF0000");
            Console.WriteLine("  - RGB: 255,255,255 or 0,0,0");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  document.txt");
            Console.WriteLine("  words.txt --font \"Times New Roman\"");
            Console.WriteLine("  input.txt -bg White -tc Navy --width 1200 --height 800");
            Console.WriteLine("  text.txt --background #F0F0F0 --text-color #333333 -o cloud.png");
            Console.WriteLine("  data.txt --show-rectangles");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  --help                    Show this help");
            Console.WriteLine("  exit, quit                Exit program");
        }

        private void PrintSettings(ParseResult result)
        {
            Console.WriteLine($"  Input file: {result.InputFile}");
            Console.WriteLine($"  Output file: {result.OutputFile}");
            Console.WriteLine($"  Font: {result.RenderSettings.FontFamily}");
            Console.WriteLine($"  Background: {result.RenderSettings.BackgroundColor.Name}");
            Console.WriteLine($"  Text color: {result.RenderSettings.TextColor.Name}");
            Console.WriteLine($"  Canvas size: {result.RenderSettings.CanvasSize.Width}x{result.RenderSettings.CanvasSize.Height}");
            Console.WriteLine($"  Show rectangles: {result.RenderSettings.ShowRectangles}");
        }

        private class ParseResult
        {
            public bool IsValid { get; set; }
            public string InputFile { get; set; }
            public string OutputFile { get; set; }
            public RenderSettings RenderSettings { get; set; }
        }
    }
}