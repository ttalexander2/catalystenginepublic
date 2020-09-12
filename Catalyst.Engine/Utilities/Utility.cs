using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static Catalyst.Engine.Rendering.Sprite2;

namespace Catalyst.Engine.Utilities
{
    //Class for reusable static methods
    public static class Utility
    {

        private static Texture2D AutoTrim(Texture2D texture)
        {
            var colorData = new Utilities.Color[texture.Width * texture.Height];
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
                    if (!pixel.Equals(Utilities.Color.Transparent)) none = false;
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
                    if (!pixel.Equals(Utilities.Color.Transparent)) none = false;
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
                    if (!pixel.Equals(Utilities.Color.Transparent)) none = false;
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
                    if (!pixel.Equals(Utilities.Color.Transparent)) none = false;
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

            var cropped = new Texture2D(Graphics.DeviceManager.GraphicsDevice, bounds.Width, bounds.Height);

            // Copy the data from the cropped region into a buffer, then into the new texture
            var data = new Utilities.Color[bounds.Width * bounds.Height];
            texture.GetData(0, bounds, data, 0, bounds.Width * bounds.Height);
            cropped.SetData(data);

            return cropped;
        }


        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

    }
}
