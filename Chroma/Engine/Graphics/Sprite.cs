using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chroma.Engine.Graphics
{
    public class Sprite : Component
    {


        public string name;
        public TextureAtlas textures;
        public Texture2D currentTexture;
        public float animationSpeed = 2.0f; //Frames per second

        public bool collidable = false; //Represents whether a moving entity can collide with this sprite
        public bool visible = true;

        public int frame { get; private set; }
        public bool loop = true;
        public bool animating = true;
        public float scale = 1.0f;
        public float rotation = 0.0f;
        public Vector2 pos;
        public Vector2 origin, dims;
        public SpriteEffects spriteEffects = new SpriteEffects();
        public Color debugColor = Color.Red * 0.4f; //Float = transparency
        public float speed = 5;

        private TimeSpan _timeChanged = new TimeSpan();


        public Sprite(string name, int x, int y, TextureAtlas textures, Origin origin)
        {
            this.name = name;
            pos = new Vector2(x, y);
            this.textures = textures;
            dims = this.textures.GetDims();
            this.origin = CalculateOffset(origin);
            animating = textures.Textures.Count > 1;
            this.currentTexture = textures.Textures[0];
        }

        public Sprite(string name, int x, int y, float scale, float rotation, TextureAtlas textures, Origin origin)
        {
            this.name = name;
            pos = new Vector2(x, y);
            this.textures = textures;
            dims = this.textures.GetDims();
            this.origin = CalculateOffset(origin);
            this.scale = scale;
            this.rotation = rotation;
            animating = textures.Textures.Count > 1;
            this.currentTexture = textures.Textures[0];
        }

        public Sprite(string name, int x, int y, TextureAtlas textures, int xOrigin, int yOrigin)
        {
            this.name = name;
            pos = new Vector2(x, y);
            this.textures = textures;
            dims = this.textures.GetDims();
            this.origin = new Vector2(xOrigin, yOrigin);
            animating = textures.Textures.Count > 1;
            this.currentTexture = textures.Textures[0];
        }

        public Sprite(string name, int x, int y, float scale, float rotation, TextureAtlas textures, int xOrigin, int yOrigin)
        {
            this.name = name;
            pos = new Vector2(x, y);
            this.textures = textures;
            dims = this.textures.GetDims();
            this.origin = new Vector2(xOrigin, yOrigin);
            this.scale = scale;
            this.rotation = rotation;
            animating = textures.Textures.Count > 1;
            this.currentTexture = textures.Textures[0];
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

        public override void Render(GameTime gameTime)
        {
            if (textures != null && currentTexture != null)
            {
                Global.SpriteBatch.Draw(currentTexture, pos, null, Color.White, rotation, origin, scale*(Global.Scale), spriteEffects, 0);
#if debug
                var t = new Texture2D(Global.Graphics.GraphicsDevice, 1, 1);
                t.SetData(new[] { Color.White });


                int bw = 1; // Border width

                Rectangle r = new Rectangle((int)pos.X, (int)pos.Y, (int)textures.getDims().X, (int)textures.getDims().Y);

                Global.SpriteBatch.Draw(t, pos, new Rectangle(r.Left, r.Top, bw, r.Height), debugColor, rotation, origin, scale * (Global.Scale), spriteEffects, 0); // Left
                Global.SpriteBatch.Draw(t, new Vector2(pos.X+r.Width* scale * (Global.Scale), pos.Y), new Rectangle(r.Right, r.Top, bw, r.Height+1), debugColor, rotation, origin, scale * (Global.Scale), spriteEffects, 0); // Right
                Global.SpriteBatch.Draw(t, pos, new Rectangle(r.Left, r.Top, r.Width, bw), debugColor, rotation, origin, scale * (Global.Scale), spriteEffects, 0); // Top
                Global.SpriteBatch.Draw(t, new Vector2(pos.X, pos.Y + r.Height* scale * (Global.Scale)), new Rectangle(r.Left, r.Bottom, r.Width, bw), debugColor, rotation, origin, scale * (Global.Scale), spriteEffects, 0); // Bottom
#endif
            }
        }

        public override void Update(GameTime gameTime)
        {

            if (gameTime.TotalGameTime.Subtract(_timeChanged).Milliseconds >= (1000 / animationSpeed) && animating)
            {
                _timeChanged = gameTime.TotalGameTime;
                frame++;
                if (loop)
                {
                    frame %= textures.Textures.Count;
                    currentTexture = textures.Textures[frame];
                }
                else
                {
                    if (frame >= textures.Textures.Count)
                    {
                        animating = false;
                    }
                    else
                    {
                        currentTexture = textures.Textures[frame];
                    }
                }
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

        


    }
}
