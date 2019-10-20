using Chroma.Engine.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Graphics
{
    public class SpriteRenderSystem : ARenderSystem
    {
        public SpriteRenderSystem(Scene scene) : base(scene) { }

        public override void Update(GameTime gameTime)
        {
            foreach (CSprite sprite in Manager.GetComponents<CSprite>().Values)
            {
                UpdateSprite(gameTime, sprite);
            }
        }

        public override void Render(GameTime gameTime)
        {
            foreach (CSprite sprite in Manager.GetComponents<CSprite>().Values)
            {
                RenderSprite(gameTime, sprite);
            }
        }

        public void RenderSprite(GameTime gameTime, CSprite sprite)
        {
            if (sprite.Textures != null && sprite.Texture != null)
            {
                CTransform transform = sprite.Entity.GetComponent<CTransform>();
                Global.SpriteBatch.Draw(sprite.Texture, transform.Position, null, Color.White, transform.Rotation, transform.Origin, transform.Scale * (Global.PixelScale), sprite.spriteEffects, sprite.layer);
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

        public void UpdateSprite(GameTime gameTime, CSprite sprite)
        {
            if (sprite.animating && (gameTime.TotalGameTime.Subtract(sprite.TimeChanged).Milliseconds >= (1000 / sprite.animationSpeed)))
            {
                sprite.TimeChanged = gameTime.TotalGameTime;
                sprite.frame++;
                OnFrameChange();
                if (sprite.frame >= sprite.Textures.Count) OnLastFrame();
                if (sprite.loop)
                {
                    sprite.frame %= sprite.Textures.Count;
                    sprite.CurrentTexture = sprite.frame;
                }
                else
                {
                    if (sprite.frame >= sprite.Textures.Count)
                    {
                        sprite.animating = false;
                    }
                    else
                    {
                        sprite.CurrentTexture = sprite.frame;
                    }
                }
            }
        }

    }
}
