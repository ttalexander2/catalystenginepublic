﻿using Chroma.Engine.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Graphics
{
    public class CameraSystem : ASystem
    {
        public CameraSystem(Scene scene) : base(scene)
        {
        }


        public override void PostUpdate(GameTime gameTime)
        {
            if (scene.Camera.Following != null && scene.Camera.Following.HasComponent<CTransform>())
            {
                CTransform follow = scene.Camera.Following.GetComponent<CTransform>();
                Vector2 offset = follow.Dimensions;
                Camera2D cam = scene.Camera;
                scene.Camera.Move(Vector2.Lerp(cam.Position, follow.Position-(scene.Camera.Size/2)+offset, cam.Speed));
            }
        }
    }
}