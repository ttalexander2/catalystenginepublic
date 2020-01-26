using Catalyst.Engine;
using Catalyst.Engine.Physics;
using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Catalyst.Engine.Utilities.Vector2;

namespace Catalyst.Game
{
    [Serializable]
    public class GravitySystem : ASystem
    {
        
        public float Gravity { get; set; }
        
        public GravitySystem(Scene scene): base(scene)
        {
            Gravity = 3.0f;
        }

        public override void PreUpdate(GameTime gameTime)
        {
            foreach (Actor actor in Manager.GetComponents<Actor>().Values)
            {
                actor.Entity.GetComponent<Velocity>().V += new Vector2(0, Gravity);
            }
        }
    }
}
