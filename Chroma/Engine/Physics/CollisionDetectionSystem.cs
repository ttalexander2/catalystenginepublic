using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Physics
{
    public class CollisionDetectionSystem : ARenderSystem
    {

        public CollisionDetectionSystem(Scene scene) : base(scene) { }
        public override void DebugRender(GameTime gameTree)
        {
            base.DebugRender(gameTree);
        }

    }
}
