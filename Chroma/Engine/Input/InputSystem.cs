using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Input
{
    [Serializable]
    public class InputSystem : ASystem
    {
        public InputSystem(Scene scene) : base(scene) { }

        public override void PreUpdate(GameTime gameTime)
        {
            foreach(CInput input in Manager.GetComponents<CInput>().Values)
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
