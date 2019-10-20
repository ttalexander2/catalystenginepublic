using Chroma.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Chroma.Engine
{
    public static class Global
    {
        public static ContentManager Content;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager Graphics;
        public static GameTime gameTime;

        public static int Width = 640;
        public static int Height = 360;
        public static float PixelScale = 3.0f;

    }
}