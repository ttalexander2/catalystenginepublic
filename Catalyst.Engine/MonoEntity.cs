using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    [Serializable]
    public abstract class MonoEntity : GameObject, IUpdatable, IRenderable
    {
        internal static int MonoEntityID { get; private set; }
        public Scene Scene { get; private set; }
        public MonoEntity(Scene scene) 
        {
            Name = string.Format("MonoEntity_{0}", MonoEntityID++);
            Scene = scene; 
        }
        public abstract void End();
        public abstract void Initialize();
        public abstract void LoadContent();

        public abstract void UnloadContent();
        public abstract void PostRender(GameTime gameTime);
        public abstract void PostUpdate(GameTime gameTime);
        public abstract void PreRender(GameTime gameTime);
        public abstract void PreUpdate(GameTime gameTime);
        public abstract void Render(GameTime gameTime);
        public abstract void RenderUI(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}
