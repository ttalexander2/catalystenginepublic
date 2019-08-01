using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma
{
    public class Sprite : Component
    {


        public string name;
        public Texture2D texture;
        public float scale = 2f;
        public float rotation = 0f;
        public Vector2 pos;
        private Vector2 origin, dims;
        private SpriteEffects spriteEffects = new SpriteEffects();
        private Color debugColor = Color.Red;


        public Sprite(string name, int x, int y, Texture2D texture, Origin origin)
        {
            this.name = name;
            pos = new Vector2(x, y);
            this.texture = AutoTrim(texture);
            dims = new Vector2(this.texture.Width, this.texture.Height);
            this.origin = CalculateOffset(origin);   
        }

        public Sprite(string name, int x, int y, float scale, float rotation, Texture2D texture, Origin origin)
        {
            this.name = name;
            pos = new Vector2(x, y);
            this.texture = AutoTrim(texture);
            dims = new Vector2(this.texture.Width, this.texture.Height);
            this.origin = CalculateOffset(origin);
            this.scale = scale;
            this.rotation = rotation;
        }

        public Sprite(string name, int x, int y, Texture2D texture, int xOrigin, int yOrigin)
        {
            this.name = name;
            pos = new Vector2(x, y);
            this.texture = AutoTrim(texture);
            dims = new Vector2(this.texture.Width, this.texture.Height);
            this.origin = new Vector2(xOrigin, yOrigin);
        }

        public Sprite(string name, int x, int y, float scale, float rotation, Texture2D texture, int xOrigin, int yOrigin)
        {
            this.name = name;
            pos = new Vector2(x, y);
            this.texture = AutoTrim(texture);
            dims = new Vector2(this.texture.Width, this.texture.Height);
            this.origin = new Vector2(xOrigin, yOrigin);
            this.scale = scale;
            this.rotation = rotation;
        }

        public enum Origin
        {
            TopLeft,
            BottomLeft,
            TopRight,
            BottomRight,
            Center,
            TopCenter,
            BottomCenter,
            CenterLeft,
            CenterRight
        }

        public override void Render()
        {
            if (texture != null)
            {
                Global.SpriteBatch.Draw(texture, pos, null, Color.White, rotation, origin, scale*Global.Scale, spriteEffects, 0);
#if Debug
                var t = new Texture2D(Global.Graphics.GraphicsDevice, 1, 1);
                t.SetData(new[] { Color.White });


                int bw = 1; // Border width

                Rectangle r = texture.Bounds;

                Global.SpriteBatch.Draw(t, pos, new Rectangle(r.Left, r.Top, bw, r.Height), debugColor, rotation, origin, scale, spriteEffects, 0); // Left
                Global.SpriteBatch.Draw(t, new Vector2(pos.X+r.Width*scale, pos.Y), new Rectangle(r.Right, r.Top, bw, r.Height+1), debugColor, rotation, origin, scale, spriteEffects, 0); // Right
                Global.SpriteBatch.Draw(t, pos, new Rectangle(r.Left, r.Top, r.Width, bw), debugColor, rotation, origin, scale, spriteEffects, 0); // Top
                Global.SpriteBatch.Draw(t, new Vector2(pos.X, pos.Y + r.Height*scale), new Rectangle(r.Left, r.Bottom, r.Width, bw), debugColor, rotation, origin, scale, spriteEffects, 0); // Bottom
#endif
            }
        }

        private Vector2 CalculateOffset(Origin origin)
        {
            switch (origin)
            {
                case (Origin.TopLeft):
                    return new Vector2(0, 0);
                case (Origin.TopRight):
                    return new Vector2(this.dims.X, 0);
                case (Origin.TopCenter):
                    return new Vector2((int)(this.dims.X/2), 0);
                case (Origin.CenterLeft):
                    return new Vector2(0, (int)(this.dims.Y / 2));
                case (Origin.CenterRight):
                    return new Vector2(this.dims.X, (int)(this.dims.Y / 2));
                case (Origin.Center):
                    return new Vector2((int)(this.dims.X / 2), (int)(this.dims.Y / 2));
                case (Origin.BottomLeft):
                    return new Vector2(0, this.dims.Y);
                case (Origin.BottomRight):
                    return new Vector2(this.dims.X, this.dims.Y);
                case (Origin.BottomCenter):
                    return new Vector2((int)(this.dims.X / 2), this.dims.Y);
                default:
                    return new Vector2(this.origin.X, this.origin.Y);
            }
                
        }

        private Texture2D AutoTrim(Texture2D texture)
        {
            Color[] colorData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colorData);

            /// Loop through the array and change the RGB values you choose

            int left = 0;
            int right = 0;
            int up = 0;
            int down = 0;

            for (int x = 0; x < texture.Width; x++)
            {
                bool none = true;
                for (int y = 0; y < texture.Height; y++)
                {
                    Color pixel = colorData[x * texture.Height + y];
                    /// Check if the color is within the range
                    if (!pixel.Equals(Color.Transparent))
                    {
                        none = false;
                    }
                }
                if (none)
                {
                    left++;
                }
                else
                {
                    break;
                }
            }

            for (int x = texture.Width-1; x >= 0; x--)
            {
                bool none = true;
                for (int y = texture.Height-1; y >= 0; y--)
                {
                    Color pixel = colorData[x * texture.Height + y];
                    /// Check if the color is within the range
                    if (!pixel.Equals(Color.Transparent))
                    {
                        none = false;
                    }
                }
                if (none)
                {
                    right++;
                }
                else
                {
                    break;
                }
            }

            for (int x = 0; x < texture.Height; x++)
            {
                bool none = true;
                for (int y = 0; y < texture.Width; y++)
                {
                    Color pixel = colorData[y * texture.Height + x];
                    /// Check if the color is within the range
                    if (!pixel.Equals(Color.Transparent))
                    {
                        none = false;
                    }
                }
                if (none)
                {
                    up++;
                }
                else
                {
                    break;
                }
            }

            for (int x = texture.Height - 1; x >= 0; x--)
            {
                bool none = true;
                for (int y = texture.Width - 1; y >= 0; y--)
                {
                    Color pixel = colorData[y * texture.Height + x];
                    /// Check if the color is within the range
                    if (!pixel.Equals(Color.Transparent))
                    {
                        none = false;
                    }
                }
                if (none)
                {
                    down++;
                }
                else
                {
                    break;
                }
            }
            Rectangle bounds = texture.Bounds;

            bounds.X += left;
            bounds.Y += up;
            bounds.Width = bounds.Width - right - left;
            bounds.Height = bounds.Height - down - up;

            Texture2D cropped = new Texture2D(Global.Graphics.GraphicsDevice, bounds.Width, bounds.Height);

            // Copy the data from the cropped region into a buffer, then into the new texture
            Color[] data = new Color[bounds.Width * bounds.Height];
            texture.GetData(0, bounds, data, 0, bounds.Width * bounds.Height);
            cropped.SetData(data);

            return cropped;
        }


    }
}
