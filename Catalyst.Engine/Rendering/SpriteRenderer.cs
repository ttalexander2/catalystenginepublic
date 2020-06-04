using Catalyst.Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public class SpriteRenderer : RenderSystem
    {
        public SpriteRenderer(Scene scene) : base(scene) { }

        public override void Update(GameTime gameTime)
        {
            foreach (Sprite sprite in Manager.GetComponents<Sprite>().Values)
            {
                if (sprite is AnimatedSprite && sprite.Active)
                    UpdateSprite(gameTime, (AnimatedSprite)sprite);
            }
        }

        public override void Render(GameTime gameTime)
        {
            foreach (Sprite sprite in Manager.GetComponents<Sprite>().Values)
            {
                if (sprite.Visible)
                    RenderSprite(gameTime, sprite);
            }
        }

        public void RenderSprite(GameTime gameTime, Sprite sprite)
        {
            if (sprite.Visible && sprite.Texture != null)
            {
                Graphics.SpriteBatch.Draw(sprite.Texture, sprite.Entity.Position, sprite.TextureRect, Utilities.Color.White, sprite.Rotation, sprite.OriginVec2, sprite.Scale, sprite.spriteEffects, sprite.Layer);
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

        private void UpdateSprite(GameTime gameTime, AnimatedSprite sprite)
        {
            if (gameTime.TotalGameTime.Subtract(sprite.TimeChanged).Milliseconds >= (1000 / sprite.AnimationSpeed))
            {
                sprite.TimeChanged = gameTime.TotalGameTime;
                sprite.Frame++;
                OnFrameChange();
                if (sprite.Frame >= sprite.Count) OnLastFrame();
                if (sprite.Loop)
                {
                    sprite.Frame %= sprite.Count;
                    sprite.CurrentTexture = sprite.Frame;
                }
                else
                {
                    if (sprite.Frame >= sprite.Count)
                    {
                        sprite.Animating = false;
                    }
                    else
                    {
                        sprite.CurrentTexture = sprite.Frame;
                    }
                }
            }
        }

    }
}
