using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chroma.Engine.Utilities.Utility;

namespace Chroma.Engine.Physics
{
    public class BoxCollider: Collider
    {
        public Rectangle BoundingBox { get; private set; }
        
        public BoxCollider(int UID, Vector2 pos, Vector2 dims): base(UID)
        {
            BoundingBox = new Rectangle((int)pos.X, (int)pos.Y, (int)(dims.X * Global.PixelScale), (int)(dims.Y * Global.PixelScale));
        }

        public BoxCollider(int UID, Vector2 pos, Vector2 dims, Origin origin) : base(UID)
        {
            Vector2 offset = Utility.OriginToVectorOffset(origin, dims);
            BoundingBox = new Rectangle((int)pos.X + (int)(offset.X * Global.PixelScale), (int)pos.Y + (int)(offset.Y * Global.PixelScale), (int)(dims.X * Global.PixelScale), (int)(dims.Y * Global.PixelScale));
        }

        public BoxCollider(int UID, int x, int y, int width, int height, Origin origin) : base(UID)
        {
            Vector2 offset = Utility.OriginToVectorOffset(origin, new Vector2(width, height));
            BoundingBox = new Rectangle(x + (int)(offset.X * Global.PixelScale), y + (int)(offset.Y * Global.PixelScale), width * (int)Global.PixelScale, height * (int)Global.PixelScale);
        }

        /**
         * TODO: Move to physics system
         */

        public override bool CollidesWith(Collider collider, Vector2 offset)
        {
            if (collider is BoxCollider)
            {
                return ((BoxCollider)collider).BoundingBox.Intersects(new Rectangle(BoundingBox.X + (int)offset.X, BoundingBox.Y + (int)offset.Y, BoundingBox.Width, BoundingBox.Height));
            }
            return false;
        }

        public override void Render(GameTime gameTime)
        {
            var t = new Texture2D(Global.Graphics.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });


            int bw = 1; // Border width

            Rectangle r = new Rectangle((int)BoundingBox.X, (int)BoundingBox.Y, (int)BoundingBox.Width, (int)BoundingBox.Height);

            Global.SpriteBatch.Draw(t, pos, new Rectangle(r.Left, r.Top, bw, r.Height), Color.Red * 0.5f, 0.0f, new Vector2(0, 0), 1, new SpriteEffects(), 0); // Left
            Global.SpriteBatch.Draw(t, new Vector2(pos.X + r.Width, pos.Y), new Rectangle(r.Right, r.Top, bw, r.Height + 1), Color.Red * 0.5f, 0.0f, new Vector2(0, 0), 1, new SpriteEffects(), 0); // Right
            Global.SpriteBatch.Draw(t, pos, new Rectangle(r.Left, r.Top, r.Width, bw), Color.Red * 0.5f, 0.0f, new Vector2(0, 0), 1, new SpriteEffects(), 0); // Top
            Global.SpriteBatch.Draw(t, new Vector2(pos.X, pos.Y + r.Height), new Rectangle(r.Left, r.Bottom, r.Width, bw), Color.Red * 0.5f, 0.0f, new Vector2(0, 0), 1, new SpriteEffects(), 0); // Bottom
        }
    }
}
