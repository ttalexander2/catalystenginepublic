using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Physics
{
    public abstract class Collider: Component
    {

        public Guid UID { get; private set; }
        public Vector2 pos { get; private set; }
        public Collider(Guid UID) {
            this.UID = UID;
        }

        public abstract bool CollidesWith(Collider collider, Vector2 offset);
    }
}
