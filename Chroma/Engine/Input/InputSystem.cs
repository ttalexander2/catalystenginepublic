using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Input
{
    public class InputSystem : ASystem
    {
        public InputSystem(Scene scene) : base(scene) { }

        public override void PreUpdate(GameTime gameTime)
        {
            foreach(CInput input in Manager.GetComponents<CInput>().Values)
            {
                input.previousKeyboardState = input.keyboardState;
                input.keyboardState = Keyboard.GetState();

                MouseState state = Mouse.GetState();
                input.mouseX = state.X;
                input.mouseY = state.Y;
                input.mousePressed = state.RightButton == ButtonState.Pressed;

                GamePadCapabilities capabilities = GamePad.GetCapabilities(
                                                   PlayerIndex.One);

                // If there a controller attached, handle it
                if (capabilities.IsConnected)
                {
                    // Get the current state of Controller1
                    input.previousGPState = input.GPState;
                    input.GPState = GamePad.GetState(PlayerIndex.One);
                }
            }
        }

    }
}
