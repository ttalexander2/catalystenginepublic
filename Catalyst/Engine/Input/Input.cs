using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalyst.Engine;
using System.Runtime.Serialization;

namespace Catalyst.Engine.Input
{
    [Serializable]
    public sealed class Input : AComponent
    {
        public static new string Name => "Input";

        [NonSerialized]
        public KeyboardState KeyboardState;
        [NonSerialized]
        public KeyboardState PreviousKeyboardState;
        [ImmediateLabel]
        public int MouseX { get; internal set; }
        [ImmediateLabel]
        public int MouseY { get; internal set; }
        [ImmediateLabel]
        public bool MousePressed { get; internal set; }
        [NonSerialized]
        public GamePadState GPState;
        [NonSerialized]
        public GamePadState PreviousGPState;
        [NonSerialized]
        public GamePadCapabilities Capabilities;

        public Input(Entity entity) : base(entity)
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
