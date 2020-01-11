using Chroma.Engine.Physics;
using System;
using System.Runtime.Serialization;

namespace Chroma.Engine
{
    [Serializable]
    public class CSolid : AComponent
    {
        
        public static new string Name => "Solid Transform";

        public CSolid(Entity entity) : base(entity)
        {
            if (!entity.HasComponent<CTransform>())
            {
                entity.AddComponent<CTransform>();
            }
        }

    }
}