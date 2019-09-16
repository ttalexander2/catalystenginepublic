using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Physics
{
    public abstract class ACollider: AComponent
    {
        public enum ColliderType
        {
            Actor,
            Solid
        }

        public int UID { get; private set; }

        public bool Active { get; set; }
        public Vector2 Position { get; private set; }
        internal ACollider(int UID): base(UID)
        {
            this.UID = UID;
        }
    }
}
