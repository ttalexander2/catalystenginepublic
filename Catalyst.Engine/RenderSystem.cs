using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    [KnownType("DerivedTypes")]
    [Serializable]
    public abstract class RenderSystem : System, IRenderable
    {

        internal RenderSystem(Scene scene) : base(scene)
        {
        }
        public override bool Renders => true;

        public virtual void PreRender(GameTime gameTime) { }
        public virtual void Render(GameTime gameTime) { }
        public virtual void PostRender(GameTime gameTime) { }

        public virtual void DebugRender(GameTime gameTime) { }

        public virtual void RenderUI(GameTime gameTime) { }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        private static Type[] DerivedTypes()
        {
            return Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(RenderSystem))).ToArray();
        }
    }
}
