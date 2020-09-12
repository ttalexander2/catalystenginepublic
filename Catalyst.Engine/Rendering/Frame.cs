using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Rendering
{
    public struct Frame
    {
        public string Unpacked;
        public string Packed;
        public Rectangle PackedBounds;
        public Rectangle UnpackedBounds;
    }
}
