using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ITagCloudAlgorithm
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}
