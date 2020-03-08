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
            if (scene.Camera.Following != null && scene.Camera.Following.HasComponent<Position>())
            {
                Position follow = scene.Camera.Following.GetComponent<Position>();
                Camera2D cam = scene.Camera;
                scene.Camera.Move(Utilities.Vector2.Lerp(cam.Position, follow.Coordinates-(scene.Camera.Size/2)+cam.Offset, cam.Speed));
            }
        }
    }
}
