using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    [Serializable]
    public class CoroutineSystem : System
    {

        public CoroutineSystem(Scene scene) : base(scene)
        {

        }
        public override void Update(GameTime gameTime)
        {
            foreach (Coroutine c in this.scene.Manager.GetComponents<Coroutine>().Values)
            {
                c.Update();
            }
        }
    }
}
