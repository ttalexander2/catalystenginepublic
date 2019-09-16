using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine
{
    public abstract class ARenderSystem : ASystem
    {

        internal ARenderSystem(Scene scene) : base(scene)
        {
        }
        public override bool Renders => true;

        public virtual void PreRender(GameTime gameTime) { }
        public virtual void Render(GameTime gameTime) { }
        public virtual void PostRender(GameTime gameTime) { }

        public virtual void DebugRender(GameTime gameTree) { }
    }
}
