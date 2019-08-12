using Microsoft.Xna.Framework;

namespace Chroma.Engine
{
    public class Component
    {
        public bool hasRender = true;
        public bool hasUpdate = true;

        public virtual void Start()
        {
        }

        public virtual void BeforeUpdate(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void AfterUpdate(GameTime gameTime)
        {
        }

        public virtual void BeforeRender(GameTime gameTime)
        {
        }

        public virtual void Render(GameTime gameTime)
        {
        }

        public virtual void AfterRender(GameTime gameTime)
        {
        }

        public virtual void End()
        {
        }
    }
}