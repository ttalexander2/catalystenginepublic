using Chroma.Engine.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.Serialization;

namespace Chroma.Engine
{
    [Serializable]
    public class Actor : AComponent
    {
        public static new string Name => "Actor";

        public Actor(Entity entity) : base(entity)
        {
            if (!entity.HasComponent<Transform>())
            {
                entity.AddComponent<Transform>();
            }
            if (!entity.HasComponent<Velocity>())
            {
                entity.AddComponent<Velocity>();
            }
        }
  
    }
}
