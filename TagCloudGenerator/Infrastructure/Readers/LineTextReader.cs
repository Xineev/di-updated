using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Reader
{
    public class LineTextReader : IReader
    {
        public List<string> TryRead(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath).ToList();
            }
            catch(IOException e)
            {
               Console.WriteLine($"Error reading file: {e.Message}");
               return null;
            }
        }
    }
}
