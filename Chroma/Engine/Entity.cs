using System;
namespace Chroma.Engine
{
    public class Entity
    {
        public bool active = true;

        public int UID { get; private set; }

        public Entity()
        {
            this.UID = EntityManager.NewId();
        }

        
    }
}