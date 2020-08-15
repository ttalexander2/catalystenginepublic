using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalystEditor.Source
{
    public static class Input
    {
        public static KeyboardState keyboardState;
        public static MouseState mouseState;

        public static void Update()
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }
    }
}
