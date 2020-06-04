using Catalyst.Engine;
using Catalyst.Engine.Input;
using Catalyst.Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
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
                KeyboardState Key = Keyboard.GetState();

                Velocity velocity = scene.Manager.GetComponent<Velocity>(player.UID);
                Input input = scene.Manager.GetComponent<Input>(player.UID);

                bool capable = input.Capabilities.IsConnected;

                if (input.KeyboardState.IsKeyDown(Keys.Up) || (capable && input.GPState.IsButtonDown(Buttons.DPadUp)))
                {
                    velocity.V += new Vector2(0, -player.VerticalSpeed);
                }

                if (input.KeyboardState.IsKeyDown(Keys.Down) || (capable && input.GPState.IsButtonDown(Buttons.DPadDown)))
                {
                    velocity.V += new Vector2(0, player.VerticalSpeed);
                }

                if (input.KeyboardState.IsKeyDown(Keys.Left) || (capable && input.GPState.IsButtonDown(Buttons.DPadLeft)))
                {
                    velocity.V += new Vector2(-player.HorizontalSpeed, 0);
                }
                if (input.KeyboardState.IsKeyDown(Keys.Right) || (capable && input.GPState.IsButtonDown(Buttons.DPadRight)))
                {
                    velocity.V += new Vector2(player.HorizontalSpeed, 0);
                }


            }
        }
    }
}
