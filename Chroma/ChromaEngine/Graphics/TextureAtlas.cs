using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma
{
    public class TextureAtlas
    {
        public List<Texture2D> Textures = new List<Texture2D>();
        private float textureWidth;
        private float textureHeight;


        public Vector2 getDims()
        {
            if (textureWidth == 0 || textureHeight == 0){
                foreach (Texture2D texture in Textures)
                {
                    if (texture.Width > textureWidth)
                    {
                        textureWidth = texture.Width;
                    }
                    if (texture.Height > textureHeight)
                    {
                        textureHeight = texture.Height;
                    }
                }
            }
            return new Vector2(textureWidth, textureHeight);
        }

        public void AutoTrim()
        {
            List<Texture2D> trimmed = new List<Texture2D>();
            foreach (Texture2D texture in Textures)
            {
                trimmed.Add(AutoTrim(texture));
            }

            Textures = trimmed;
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

            for (int x = texture.Width - 1; x >= 0; x--)
            {
                bool none = true;
                for (int y = texture.Height - 1; y >= 0; y--)
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
