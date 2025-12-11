using System;
using System.Collections.Generic;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IReader
    {
        List<string> TryRead(string filePath);
    }
}
