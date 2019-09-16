using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine
{
    public abstract class ASystem
    {
        protected Scene scene;
        protected ECManager Manager
        {
            get
            {
                return scene.Manager;
            }
        }
        internal ASystem(Scene scene)
        {
            this.scene = scene;
        }
        public virtual bool Renders => false;
        public virtual void Initialize() { }
        public virtual void PreUpdate(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void PostUpdate(GameTime gameTime) { }
        public virtual void End(GameTime gameTime) { }
        public virtual void Save(GameTime gameTime) { }

    }
}
