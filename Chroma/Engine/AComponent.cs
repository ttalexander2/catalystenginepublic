using Microsoft.Xna.Framework;
using System;

namespace Chroma.Engine
{
    [Serializable]
    public abstract class AComponent
    {

        public static string Name => "Abstract Component";
        public int UID { get; private set; }
        public bool Active { get; set; }

        public Entity Entity { get; private set; }

        internal AComponent(Entity entity)
        {
            this.Entity = entity;
            this.UID = entity.UID;
            this.Active = true;
        }





    }
}