using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Reader
{
    public class LineTextReader : IReader
    {
        public IEnumerable<string>? TryRead(string filePath)
        {
            try
            {
                string[] fileContent = File.ReadAllLines(filePath);
                return fileContent.Select(w => w.ToLower());
            }
            catch(IOException e)
            {
               Console.WriteLine($"Error reading file: {e.Message}");
               return null;
            }
        }
    }
}
