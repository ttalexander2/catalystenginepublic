using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Physics
{
    public class CTransform : AComponent
    {
        internal CTransform(int UID): base(UID) { }
        public Vector2 Position;
        public float Rotation;

    }
}
