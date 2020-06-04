using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Utilities
{
    [Serializable]
    public enum RectangleOrigin
    {
        TopLeft,
        BottomLeft,
        TopRight,
        BottomRight,
        Center,
        TopCenter,
        BottomCenter,
        CenterLeft,
        CenterRight,
        Custom
    }
}
