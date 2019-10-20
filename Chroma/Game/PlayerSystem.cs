using Chroma.Engine;
using Chroma.Engine.Input;
using Chroma.Engine.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Key = Chroma.Engine.Input.Controller;

namespace Chroma.Game
{
    [Serializable]
    public class PlayerSystem : ASystem
    {
        public PlayerSystem(Scene scene): base(scene){}

        public override void Update(GameTime gameTime)
        {
            foreach (CPlayer player in scene.Manager.GetComponents<CPlayer>().Values)
            {
                if (!player.Entity.HasComponent<CActor>())
                {
                    continue;
                }

                CVelocity velocity = scene.Manager.GetComponent<CVelocity>(player.UID);
                CInput input = scene.Manager.GetComponent<CInput>(player.UID);

                bool capable = input.Capabilities.IsConnected;

                if (input.KeyboardState.IsKeyDown(Key.KUp) || (capable && input.GPState.IsButtonDown(Key.GUp)))
                {
                    velocity.Velocity += new Vector2(0, -player.VerticalSpeed);
                }

                if (input.KeyboardState.IsKeyDown(Key.KDown) || (capable && input.GPState.IsButtonDown(Key.GDown)))
                {
                    velocity.Velocity += new Vector2(0, player.VerticalSpeed);
                }

                if (input.KeyboardState.IsKeyDown(Key.KLeft) || (capable && input.GPState.IsButtonDown(Key.GLeft)))
                {
                    velocity.Velocity += new Vector2(-player.HorizontalSpeed, 0);
                }
                if (input.KeyboardState.IsKeyDown(Key.KRight) || (capable && input.GPState.IsButtonDown(Key.GRight)))
                {
                    velocity.Velocity += new Vector2(player.HorizontalSpeed, 0);
                }


            }
        }
    }
}
