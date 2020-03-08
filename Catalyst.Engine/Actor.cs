using Catalyst.Engine.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.Serialization;

namespace Catalyst.Engine
{
    [Serializable]
    public class Actor : Component
    {
        public static new string Name => "Actor";

        public Actor(Entity entity) : base(entity)
        {
            if (!entity.HasComponent<Position>())
            {
                entity.AddComponent<Position>();
            }
            if (!entity.HasComponent<Velocity>())
            {
                entity.AddComponent<Velocity>();
            }
        }
  
    }
}
