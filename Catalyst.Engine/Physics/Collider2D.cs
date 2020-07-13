using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;

namespace Catalyst.Engine
{
    [Serializable]
    [StoreChildren]
    public abstract class Collider2D : Component
    {
        public Rectangle Bounds;
        public bool Colliding;
        public HashSet<Collider2D> Collisions;
        public CollisionSystem System;
        [GuiEnum]
        public CollisionMask Mask;
        protected Collider2D(Entity entity) : base(entity, typeof(Collider2D))
        {
            Bounds = Rectangle.Empty;
            Colliding = false;
            Collisions = new HashSet<Collider2D>();
            System = Entity.Scene.CollisionSystem;
            System.Quad.Insert(this);
            if (entity is Actor)
                Mask = CollisionMask.Actors;
            else if (entity is Solid)
                Mask = CollisionMask.Solids;
            else Mask = CollisionMask.All;
        }

        public abstract void DebugRender();

    }
}
