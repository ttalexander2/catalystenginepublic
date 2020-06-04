using Catalyst.Engine.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public class CameraSystem : System
    {
        public CameraSystem(Scene scene) : base(scene)
        {
        }


        public override void PostUpdate(GameTime gameTime)
        {
            if (scene.Camera.Following != null)
            {
                Camera cam = scene.Camera;
                scene.Camera.Move(Utilities.Vector2.Lerp(cam.Position, scene.Camera.Following.Position-(scene.Camera.Size/2)+cam.Offset, cam.Speed));
            }
        }
    }
}
