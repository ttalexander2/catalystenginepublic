using Catalyst.Engine.Physics;
using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Catalyst.Engine.Utilities.Rectangle;

namespace Catalyst.Engine
{
    public class CollisionSystem : System, IDebugRender
    {
        public QuadTree<Collider2D> Quad;
        public CollisionSystem(Scene scene) : base(scene)
        {
            Quad = new QuadTree<Collider2D>();
            Quad.Bounds = new Rectangle(0, 0, (int)scene.Dimensions.X, (int)scene.Dimensions.Y);
        }
        public override void PreUpdate(GameTime gameTime)
        {
            foreach(Collider2D collider in scene.Manager.GetComponents<BoxCollider2D>().Values)
            {

            }
        }

        public void DebugRender(GameTime gameTime)
        {
            foreach(Collider2D collider in scene.Manager.GetComponents<BoxCollider2D>().Values)
            {
                collider.DebugRender();
            }
        }
    }
}
