using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chroma.Engine;

namespace Chroma.Engine.Input
{
    [Serializable]
    public sealed class CInput : AComponent
    {
        public static new string Name => "Input";
        public KeyboardState KeyboardState { get; set; }
        public KeyboardState PreviousKeyboardState { get; set; }

        public int MouseX { get; set; }
        public int MouseY { get; set; }
        public bool MousePressed { get; set; }
        public GamePadState GPState { get; set; }

        public GamePadState PreviousGPState { get; set; }

        public GamePadCapabilities Capabilities { get; set; }

        public CInput(Entity entity) : base(entity)
        {
            this.KeyboardState = new KeyboardState();
            this.PreviousKeyboardState = new KeyboardState();
            this.MouseX = 0;
            this.MouseY = 0;
            this.MousePressed = false;
            this.PreviousGPState = new GamePadState();
            this.GPState = new GamePadState();
        }

    }
}
