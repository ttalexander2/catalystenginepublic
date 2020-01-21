using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chroma.Engine;
using System.Runtime.Serialization;

namespace Chroma.Engine.Input
{
    [Serializable]
    public sealed class CInput : AComponent
    {
        public static new string Name => "Input";
        [NonSerialized]
        public KeyboardState KeyboardState;
        [NonSerialized]
        public KeyboardState PreviousKeyboardState;

        public int MouseX { get; internal set; }
        public int MouseY { get; internal set; }
        public bool MousePressed { get; internal set; }
        [NonSerialized]
        public GamePadState GPState;
        [NonSerialized]
        public GamePadState PreviousGPState;
        [NonSerialized]
        public GamePadCapabilities Capabilities;

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
