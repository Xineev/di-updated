using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IReader
    {
        IEnumerable<string> TryRead(string filePath);
    }
}
