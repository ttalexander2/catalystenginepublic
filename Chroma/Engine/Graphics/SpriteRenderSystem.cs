using Chroma.Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Graphics
{
    [Serializable]
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
                Global.SpriteBatch.Draw(sprite.Texture, transform.Position, null, Utilities.Color.White, transform.Rotation, transform.Origin, transform.Scale, sprite.spriteEffects, sprite.layer);
#if Debug
                Texture2D rect = new Texture2D(Global.Graphics.GraphicsDevice, (int)transform.CollisionDims.X, (int)transform.CollisionDims.Y);

                Utilities.Color[] data = new Utilities.Color[(int)transform.CollisionDims.X * (int)transform.CollisionDims.Y];
                for (int i = 0; i < data.Length; ++i) data[i] = Utilities.Color.Red * 0.5f;
                rect.SetData(data);

                Utilities.Vector2 coor = new Utilities.Vector2(10, 20);

                Global.SpriteBatch.Draw(rect, transform.Position + transform.CollisionOffset, null, Utilities.Color.White, transform.Rotation, transform.Origin, transform.Scale, new SpriteEffects(), sprite.layer);
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
