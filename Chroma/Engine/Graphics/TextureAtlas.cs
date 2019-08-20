using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chroma.Engine.Graphics
{
    public class TextureAtlas
    {
        private int _textureHeight;
        public List<Texture2D> Textures = new List<Texture2D>();
        private int _textureWidth;


        public Vector2 GetDims()
        {
            if (_textureWidth == 0 || _textureHeight == 0)
                for (int i = 0; i < Textures.Count; i++)
                {
                    if (Textures[i].Width > _textureWidth) _textureWidth = Textures[i].Width;
                    if (Textures[i].Height > _textureHeight) _textureHeight = Textures[i].Height;
                }

            return new Vector2(_textureWidth, _textureHeight);
        }

        public void AutoTrim()
        {
            var trimmed = new List<Texture2D>();
            for (int i = 0; i < Textures.Count; i++) trimmed.Add(AutoTrim(Textures[i]));

            Textures = trimmed;
        }

        private Texture2D AutoTrim(Texture2D texture)
        {
            var colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);

            // Loop through the array and change the RGB values you choose

            var left = 0;
            var right = 0;
            var up = 0;
            var down = 0;

            for (var x = 0; x < texture.Width; x++)
            {
                var none = true;
                for (var y = 0; y < texture.Height; y++)
                {
                    var pixel = colorData[x * texture.Height + y];
                    // Check if the color is within the range
                    if (!pixel.Equals(Color.Transparent)) none = false;
                }

                if (none)
                    left++;
                else
                    break;
            }

            for (var x = texture.Width - 1; x >= 0; x--)
            {
                var none = true;
                for (var y = texture.Height - 1; y >= 0; y--)
                {
                    var pixel = colorData[x * texture.Height + y];
                    // Check if the color is within the range
                    if (!pixel.Equals(Color.Transparent)) none = false;
                }

                if (none)
                    right++;
                else
                    break;
            }

            for (var x = 0; x < texture.Height; x++)
            {
                var none = true;
                for (var y = 0; y < texture.Width; y++)
                {
                    var pixel = colorData[y * texture.Height + x];
                    // Check if the color is within the range
                    if (!pixel.Equals(Color.Transparent)) none = false;
                }

                if (none)
                    up++;
                else
                    break;
            }

            for (var x = texture.Height - 1; x >= 0; x--)
            {
                var none = true;
                for (var y = texture.Width - 1; y >= 0; y--)
                {
                    var pixel = colorData[y * texture.Height + x];
                    // Check if the color is within the range
                    if (!pixel.Equals(Color.Transparent)) none = false;
                }

                if (none)
                    down++;
                else
                    break;
            }

            var bounds = texture.Bounds;

            bounds.X += left;
            bounds.Y += up;
            bounds.Width = bounds.Width - right - left;
            bounds.Height = bounds.Height - down - up;

            var cropped = new Texture2D(Global.Graphics.GraphicsDevice, bounds.Width, bounds.Height);

            // Copy the data from the cropped region into a buffer, then into the new texture
            var data = new Color[bounds.Width * bounds.Height];
            texture.GetData(0, bounds, data, 0, bounds.Width * bounds.Height);
            cropped.SetData(data);

            return cropped;
        }
    }
}