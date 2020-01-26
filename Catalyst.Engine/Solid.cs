using Catalyst.Engine.Physics;
using System;
using System.Runtime.Serialization;

namespace Catalyst.Engine
{
    [Serializable]
    public class Solid : AComponent
    {
        
        public static new string Name => "Solid Transform";

        public Solid(Entity entity) : base(entity)
        {
            if (!entity.HasComponent<Transform>())
            {
                entity.AddComponent<Transform>();
            }
        }

    }
}