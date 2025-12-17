using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface INormalizer
    {
        public List<string> Normalize(List<string> words);
    }
}
