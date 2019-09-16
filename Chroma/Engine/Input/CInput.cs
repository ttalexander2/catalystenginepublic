using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chroma.Engine;

namespace Chroma.Engine.Input
{
    public sealed class CInput : AComponent
    {
        public static new string Name => "Input";
        public KeyboardState keyboardState { get; set; }
        public KeyboardState previousKeyboardState { get; set; }

        public int mouseX { get; set; }
        public int mouseY { get; set; }
        public bool mousePressed { get; set; }
        public GamePadState GPState { get; set; }

        public GamePadState previousGPState { get; set; }
        public int UID { get; private set; }
        public bool Active { get; set; }

        internal CInput(int UID) : base(UID)
        {
            this.keyboardState = new KeyboardState();
            this.previousKeyboardState = new KeyboardState();
            this.mouseX = 0;
            this.mouseY = 0;
            this.mousePressed = false;
            this.previousGPState = new GamePadState();
            this.GPState = new GamePadState();
        }

    }
}
