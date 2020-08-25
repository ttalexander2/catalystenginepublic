using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public struct PackedTexture
    {
        public int ID { get; private set; }
        public string Tag { get; private set; }
        public Atlas Atlas { get; private set; }
        public bool TextureRotated { get; private set; }
        public Rectangle Bounds { get; private set; }
        public bool Animated { get; private set; }

        public PackedTexture(int id, string tag, Atlas atlas, Rectangle bounds, bool rotated, bool animated)
        {
            this.ID = id;
            this.Tag = tag;
            this.Atlas = atlas;
            this.TextureRotated = rotated;
            this.Bounds = bounds;
            this.Animated = animated;
        }
    }
}
