using Chroma.Engine;
using Chroma.Engine.Physics;
using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Chroma.Engine.Utilities.Vector2;

namespace Chroma.Game
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
            foreach (CActor actor in Manager.GetComponents<CActor>().Values)
            {
                actor.Entity.GetComponent<CVelocity>().Velocity += new Vector2(0, Gravity);
            }
        }
    }
}
