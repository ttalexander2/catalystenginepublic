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
    public class Sprite : Component
    {
        public static bool UsePacked = true;

        public bool Visible = true;
        public bool Animated { get { return Frames.Count > 1; } }

        internal FrameList Frames = new FrameList();


        public float Rotation = 0.0f;
        public float Layer = 0.0f;
        public Vector2 Origin = Vector2.Zero;
        public Vector2 Scale = Vector2.One;

        public Sprite(Entity entity) : base(entity)
        {
            
        }

        private void Render(Vector2 position)
        {
            if (Visible && UsePacked)
            {
                Texture2D t = TextureCache.GetTexture(Frames.CurrentFrame.Packed);
                if (t == null) return;

                Graphics.SpriteBatch.Draw(t, position, Frames.CurrentFrame.UnpackedBounds, Microsoft.Xna.Framework.Color.White, Rotation, Origin, Scale, SpriteEffects.None, Layer);
            }
            else if (Visible && !UsePacked)
            {
                Texture2D t = TextureCache.GetTexture(Frames.CurrentFrame.Unpacked);
                if (t == null) return;

                Graphics.SpriteBatch.Draw(t, position, Frames.CurrentFrame.PackedBounds, Microsoft.Xna.Framework.Color.White, Rotation, Origin, Scale, SpriteEffects.None, Layer);
            }
        }

        public void LoadContent()
        {
            foreach (Frame f in Frames)
            {
                if (UsePacked)
                {
                    TextureCache.LoadTexture(f.Packed);
                }
                else
                {
                    TextureCache.LoadTexture(f.Unpacked);
                }
            }
        }
    }
}
