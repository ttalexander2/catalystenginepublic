using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Input
{
    /// <summary>
    /// System to manage updating input components.
    /// </summary>
    [Serializable]
    public class InputSystem : System
    {
        /// <summary>
        /// Create and initialize InputSystem for a scene.
        /// </summary>
        /// <param name="scene"></param>
        public InputSystem(Scene scene) : base(scene) { }

        /// <summary>
        /// Update all input components. See <see cref="Catalyst.Engine.Input.Input"/>.
        /// Update occurs befor main update. Generally, this system is checked first.
        /// Currently, single player input is checked.
        /// </summary>
        /// <param name="gameTime">Current detla time.</param>
        public override void PreUpdate(GameTime gameTime)
        {
            foreach(Input input in Manager.GetComponents<Input>().Values)
            {
                input.PreviousKeyboardState = input.KeyboardState;
                input.KeyboardState = Keyboard.GetState();

                MouseState state = Mouse.GetState();
                input.MouseX = state.X;
                input.MouseY = state.Y;
                input.MousePressed = state.RightButton == ButtonState.Pressed;

                input.Capabilities = GamePad.GetCapabilities(
                                                   PlayerIndex.One);

                // If there a controller attached, handle it
                if (input.Capabilities.IsConnected)
                {
                    // Get the current state of Controller1
                    input.PreviousGPState = input.GPState;
                    input.GPState = GamePad.GetState(PlayerIndex.One);
                }
            }
        }

    }
}
