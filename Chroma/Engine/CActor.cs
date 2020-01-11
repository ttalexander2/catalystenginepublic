using Chroma.Engine.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.Serialization;

namespace Chroma.Engine
{
    [Serializable]
    public class CActor : AComponent
    {
        public static new string Name => "Actor";

        public CActor(Entity entity) : base(entity)
        {
            if (!entity.HasComponent<CTransform>())
            {
                entity.AddComponent<CTransform>();
            }
            if (!entity.HasComponent<CVelocity>())
            {
                entity.AddComponent<CVelocity>();
            }
        }
  
    }
}
