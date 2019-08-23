using System;
using System.Collections.Generic;
using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Chroma.Engine.Utilities.Utility;

namespace Chroma.Engine.Graphics
{
    public class Sprite: Component
    {


        public string name;
        public Guid UID { get; private set; }
        public int layer;

        private int _textureHeight;
        private int _textureWidth;
        public List<Texture2D> Textures = new List<Texture2D>();
        public Texture2D currentTexture;
        public float animationSpeed = 2.0f; //Frames per second

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

        private TimeSpan _timeChanged = new TimeSpan();


        public Sprite(Guid UID, string name, int x, int y, Texture2D[] textures, Origin origin)
        {
            this.UID = UID;
            this.name = name;
            pos = new Vector2(x, y);
            this.Textures.AddRange(textures);
            dims = GetDims();
            this.origin = Utility.OriginToVectorOffset(origin, dims);
            animating = Textures.Count > 1;
            this.currentTexture = Textures[0];
        }

        public Sprite(Guid UID, string name, int x, int y, float scale, float rotation, Texture2D[] textures, Origin origin)
        {
            this.UID = UID;
            this.name = name;
            pos = new Vector2(x, y);
            this.Textures.AddRange(textures);
            dims = GetDims(); ;
            this.origin = Utility.OriginToVectorOffset(origin, dims);
            this.scale = scale;
            this.rotation = rotation;
            animating = Textures.Count > 1;
            this.currentTexture = Textures[0];
        }

        public Sprite(Guid UID, string name, int x, int y, Texture2D[] textures, int xOrigin, int yOrigin)
        {
            this.UID = UID;
            this.name = name;
            pos = new Vector2(x, y);
            this.Textures.AddRange(textures);
            dims = GetDims();
            this.origin = new Vector2(xOrigin, yOrigin);
            animating = Textures.Count > 1;
            this.currentTexture = Textures[0];
        }

        public Sprite(Guid UID, string name, int x, int y, float scale, float rotation, Texture2D[] textures, int xOrigin, int yOrigin)
        {
            this.UID = UID;
            this.name = name;
            pos = new Vector2(x, y);
            this.Textures.AddRange(textures);
            dims = GetDims();
            this.origin = new Vector2(xOrigin, yOrigin);
            this.scale = scale;
            this.rotation = rotation;
            animating = Textures.Count > 1;
            this.currentTexture = Textures[0];
        }



        public override void Render(GameTime gameTime)
        {
            if (Textures != null && currentTexture != null)
            {
                Global.SpriteBatch.Draw(currentTexture, pos, null, Color.White, rotation, origin, scale*(Global.PixelScale), spriteEffects, layer);
#if DebugMode
                var t = new Texture2D(Global.Graphics.GraphicsDevice, 1, 1);
                t.SetData(new[] { Color.White });


                int bw = 1; // Border width

                Rectangle r = new Rectangle((int)pos.X, (int)pos.Y, (int)dims.X, (int)dims.Y);

                Global.SpriteBatch.Draw(t, pos, new Rectangle(r.Left, r.Top, bw, r.Height), debugColor, rotation, origin, scale * (Global.PixelScale), spriteEffects, 0); // Left
                Global.SpriteBatch.Draw(t, new Vector2(pos.X+r.Width* scale * (Global.PixelScale), pos.Y), new Rectangle(r.Right, r.Top, bw, r.Height+1), debugColor, rotation, origin, scale * (Global.PixelScale), spriteEffects, 0); // Right
                Global.SpriteBatch.Draw(t, pos, new Rectangle(r.Left, r.Top, r.Width, bw), debugColor, rotation, origin, scale * (Global.PixelScale), spriteEffects, 0); // Top
                Global.SpriteBatch.Draw(t, new Vector2(pos.X, pos.Y + r.Height* scale * (Global.PixelScale)), new Rectangle(r.Left, r.Bottom, r.Width, bw), debugColor, rotation, origin, scale * (Global.PixelScale), spriteEffects, 0); // Bottom
#endif
            }
        }

        public virtual void OnFrameChange()
        {

        }

        public virtual void OnLastFrame()
        {

        }

        public override void Update(GameTime gameTime)
        {

            if(animating && (gameTime.TotalGameTime.Subtract(_timeChanged).Milliseconds >= (1000 / animationSpeed)))
            {
                _timeChanged = gameTime.TotalGameTime;
                frame++;
                OnFrameChange();
                if (frame >= Textures.Count) OnLastFrame();
                if (loop)
                {
                    frame %= Textures.Count;
                    currentTexture = Textures[frame];
                }
                else
                {
                    if (frame >= Textures.Count)
                    {
                        animating = false;
                    }
                    else
                    {
                        currentTexture = Textures[frame];
                    }
                }
            }
        }

        public override void AfterUpdate(GameTime gameTime)
        {
            Actor actor = World.currentScene?.GetActor(UID);
            if (actor != null)
            {
                pos = actor.Position;
            }
        }
        public Vector2 GetDims()
        {
            for (int i = 0; i < Textures.Count; i++)
            {
                if (Textures[i].Width > _textureWidth) _textureWidth = Textures[i].Width;
                if (Textures[i].Height > _textureHeight) _textureHeight = Textures[i].Height;
            }
            return new Vector2(_textureWidth, _textureHeight);
        }

        
    }




}

