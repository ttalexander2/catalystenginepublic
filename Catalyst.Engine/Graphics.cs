using Catalyst.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst.Engine
{
    public static class Graphics
    {
        public static ContentManager Content;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager DeviceManager;

        public static int Width = 640;
        public static int Height = 360;

        public static int PreferredWindowWidth = 1280;
        public static int PreferredWindowHeight = 720;

        public static double FPSCap = 256d;

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
                return Engine.Instance.Screen;
            }
            set { }
        }

        public static void DrawNative(Texture2D texture, Utilities.Vector2 position, Utilities.Rectangle? sourceRect, Utilities.Color color, float rotation, Utilities.Vector2 origin, float scale, SpriteEffects spriteEffect, float layer)
        {
            SpriteBatch.Draw(texture, position * SpriteScale + ScreenOffset, sourceRect, color, rotation, origin, scale, spriteEffect, layer);
        }

    }
}