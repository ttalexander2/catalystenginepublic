using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Physics
{
    public abstract class Collider2D : Component
    {
        protected Collider2D(Entity entity) : base(entity)
        {
        }

        public abstract void DebugRender();

    }
}
