using Catalyst.Engine.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Rectangle = Catalyst.Engine.Utilities.Rectangle;

namespace Catalyst.Engine
{
    [Serializable]
    public class CollisionSystem : System, IDebugRender
    {
        public QuadTree Quad;

        private List<Collider2D> _collisionsToCheck = new List<Collider2D>();
        public CollisionSystem(Scene scene) : base(scene)
        {
            Quad = new QuadTree(new Rectangle(0, 0, (int)scene.Dimensions.X, (int)scene.Dimensions.Y));
        }
        public override void PreUpdate(GameTime gameTime)
        {
            foreach(Collider2D collider in scene.Manager.GetComponents<Collider2D>().Values)
            {
                collider.Bounds.X = (int)collider.Entity.Position.X;
                collider.Bounds.Y = (int)collider.Entity.Position.Y;
                collider.Colliding = false;
            }
        }

        public void DebugRender(GameTime gameTime)
        {
            foreach(Collider2D collider in scene.Manager.GetComponents<Collider2D>().Values)
            {
                collider.DebugRender();
            }
        }

        public bool CheckCollision(Collider2D collider)
        {
            return CheckCollision(collider, CollisionMask.All);
        }

        public bool CheckCollision(Collider2D collider, CollisionMask mask)
        {
            bool toReturn = false;
            _collisionsToCheck.Clear();

            Quad.Retrieve(_collisionsToCheck, collider);
            foreach (Collider2D c in _collisionsToCheck)
            {
                if (collider != c && c.Mask == mask && Intersects(collider, c))
                {
                    collider.Colliding = true;
                    toReturn = true;
                    collider.Collisions.Add(c);
                }
            }

            return toReturn;
        }


        private bool Intersects(Collider2D a, Collider2D b)
        {
            //TODO: Add actual algorithms
            if (a is BoxCollider2D && b is BoxCollider2D)
            {
                return a.Bounds.Intersects(b.Bounds);
            }
                
            return false;
        }
    }

    public enum CollisionMask
    {
        Actors,
        Solids,
        All
    }
}
