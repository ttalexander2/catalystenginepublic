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

        public Vector2 Velocity { get; set; }
        public CVelocity(Entity entity) : base(entity)
        {
            Velocity = new Vector2();
        }
    }
}
