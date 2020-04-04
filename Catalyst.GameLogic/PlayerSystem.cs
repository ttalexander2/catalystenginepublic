using Catalyst.Engine;
using Catalyst.Engine.Input;
using Catalyst.Engine.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Key = Catalyst.Engine.Input.Controller;
using Vector2 = Catalyst.Engine.Utilities.Vector2;

namespace Catalyst.GameLogic
{
    [Serializable]
    public class PlayerSystem : Engine.System
    {
        public PlayerSystem(Scene scene): base(scene){}

        public override void Update(GameTime gameTime)
        {
            foreach (Player player in scene.Manager.GetComponents<Player>().Values)
            {
                if (!player.Entity.HasComponent<Actor>())
                {
                    continue;
                }

                Velocity velocity = scene.Manager.GetComponent<Velocity>(player.UID);
                Input input = scene.Manager.GetComponent<Input>(player.UID);

                bool capable = input.Capabilities.IsConnected;

                if (input.KeyboardState.IsKeyDown(Key.KUp) || (capable && input.GPState.IsButtonDown(Key.GUp)))
                {
                    velocity.V += new Vector2(0, -player.VerticalSpeed);
                }

                if (input.KeyboardState.IsKeyDown(Key.KDown) || (capable && input.GPState.IsButtonDown(Key.GDown)))
                {
                    velocity.V += new Vector2(0, player.VerticalSpeed);
                }

                if (input.KeyboardState.IsKeyDown(Key.KLeft) || (capable && input.GPState.IsButtonDown(Key.GLeft)))
                {
                    velocity.V += new Vector2(-player.HorizontalSpeed, 0);
                }
                if (input.KeyboardState.IsKeyDown(Key.KRight) || (capable && input.GPState.IsButtonDown(Key.GRight)))
                {
                    velocity.V += new Vector2(player.HorizontalSpeed, 0);
                }


            }
        }
    }
}
