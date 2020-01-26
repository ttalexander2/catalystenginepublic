using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public struct MTexture
    {
        public int ID { get; private set; }
        public Rectangle Bounds { get; private set; }
        public string Tag { get; private set; }
        public TextureAtlas Atlas { get; private set; }

        public int X
        {
            get
            {
                return Bounds.X;
            }
        }

        public int Y
        {
            get
            {
                return Bounds.Y;
            }
        }

        public int Width
        {
            get
            {
                return Bounds.Width;
            }
        }

        public int Height
        {
            get
            {
                return Bounds.Height;
            }
        }



        public MTexture(int id, Rectangle bounds, string tag, TextureAtlas atlas)
        {
            ID = id;
            Bounds = bounds;
            Tag = tag;
            Atlas = atlas;
        }
    }
}
