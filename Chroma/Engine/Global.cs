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

        public static int PreferredWindowWidth = 1920;
        public static int PreferredWindowHeight = 1080;

        public static Utilities.Vector2 SpriteScale
        {
            get
            {
                return new Utilities.Vector2(PreferredWindowWidth / Width, PreferredWindowHeight / Height);
            }
            private set { }
        }

        public static Utilities.Vector2 ScreenOffset { get; internal set; }
        public static Utilities.Rectangle RenderBounds
        {
            get
            {
                return ChromaGame.Instance.Screen;
            }
            set { }
        }

        public static void DrawNative(Texture2D texture, Utilities.Vector2 position, Utilities.Rectangle? sourceRect, Utilities.Color color, float rotation, Utilities.Vector2 origin, float scale, SpriteEffects spriteEffect, float layer)
        {
            SpriteBatch.Draw(texture, position * SpriteScale + ScreenOffset, sourceRect, color, rotation, origin, scale, spriteEffect, layer);
        }

    }
}