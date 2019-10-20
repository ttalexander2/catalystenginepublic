using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Physics
{
    [Serializable]
    public class CollisionDetectionSystem : ASystem
    {

        private QuadTree quad;

        public CollisionDetectionSystem(Scene scene) : base(scene)
        {
            quad = new QuadTree(new Rectangle(0, 0, Global.Width, Global.Height));
        }

        public override void Update(GameTime gameTime)
        {

        }

        

    }
}
