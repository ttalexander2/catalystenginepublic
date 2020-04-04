using Catalyst.Engine;
using Catalyst.Engine.Physics;
using System;
using Vector2 = Catalyst.Engine.Utilities.Vector2;

namespace Catalyst.GameLogic
{
    [Serializable]
    public class GravitySystem : Engine.System
    {
        
        public float Gravity { get; set; }
        
        public GravitySystem(Scene scene): base(scene)
        {
            Gravity = 0.0f;
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (Actor actor in Manager.GetComponents<Actor>().Values)
            {
                actor.Entity.GetComponent<Velocity>().V += new Vector2(0, Gravity);
            }
        }
    }
}
