using Chroma.Engine.Physics;

namespace Chroma.Engine {
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