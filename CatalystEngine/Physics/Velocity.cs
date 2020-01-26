using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Physics
{
    [Serializable]
    public class Velocity : AComponent
    {
        [ImmediateVector2]
        public Utilities.Vector2 V { get; set; }
        public Velocity(Entity entity) : base(entity)
        {
            V = new Utilities.Vector2();
        }
    }
}
