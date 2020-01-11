using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine
{
    [KnownType("DerivedTypes")]
    [Serializable]
    public abstract class ARenderSystem : ASystem
    {

        internal ARenderSystem(Scene scene) : base(scene)
        {
        }
        public override bool Renders => true;

        public virtual void PreRender(GameTime gameTime) { }
        public virtual void Render(GameTime gameTime) { }
        public virtual void PostRender(GameTime gameTime) { }

        public virtual void DebugRender(GameTime gameTime) { }

        public virtual void RenderNative(GameTime gameTime) { }

        public virtual void RenderUI(GameTime gameTime) { }

        private static Type[] DerivedTypes()
        {
            return Assembly.GetAssembly(typeof(AComponent)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ARenderSystem))).ToArray();
        }
    }
}
