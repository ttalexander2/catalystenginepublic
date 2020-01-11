using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Physics
{
    [Serializable]
    public class CVelocity : AComponent
    {
        
        public Utilities.Vector2 Velocity { get; set; }
        public CVelocity(Entity entity) : base(entity)
        {
            Velocity = new Utilities.Vector2();
        }
    }
}
