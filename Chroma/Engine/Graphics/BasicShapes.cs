using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Graphics
{
    public static class BasicShapes
    {
        public static Texture2D GenerateCircleTexture(int radius, Color color, float sharpness)
        {
            int diameter = radius * 2;
            Texture2D circleTexture = new Texture2D(Global.Graphics.GraphicsDevice, diameter, diameter, false, SurfaceFormat.Color);
            Color[] colorData = new Color[circleTexture.Width * circleTexture.Height];
            Vector2 center = new Vector2(radius);
            for (int colIndex = 0; colIndex < circleTexture.Width; colIndex++)
            {
                for (int rowIndex = 0; rowIndex < circleTexture.Height; rowIndex++)
                {
                    Vector2 position = new Vector2(colIndex, rowIndex);
                    float distance = Vector2.Distance(center, position);

                    // hermite iterpolation
                    float x = distance / diameter;
                    float edge0 = (radius * sharpness) / (float)diameter;
                    float edge1 = radius / (float)diameter;
                    float temp = MathHelper.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
                    float result = temp * temp * (3.0f - 2.0f * temp);

                    colorData[rowIndex * circleTexture.Width + colIndex] = color * (1f - result);
                }
            }
            circleTexture.SetData<Color>(colorData);

            return circleTexture;
        }
    }
}
