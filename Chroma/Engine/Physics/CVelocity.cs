using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Physics
{
    public class CVelocity : AComponent
    {
        public static new string Name => "Velocity";
        public Vector2 Vector { get; set; }

        internal CVelocity(int UID) : base(UID)
        {
            Vector = Vector2.Zero;
        }
    }
}
