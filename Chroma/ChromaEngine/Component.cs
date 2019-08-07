using Microsoft.Xna.Framework;
using System;
namespace Chroma
{
    public class Component
    {
        public bool hasUpdate = true;
        public bool hasRender = true;

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
    }
}
