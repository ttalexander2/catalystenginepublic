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
    /// <summary>
    /// Input component class, used to keep track of the input for a current frame.
    /// </summary>
    [Serializable]
    public sealed class Input : AComponent
    {
        public static new string Name => "Input";

        /// <summary>
        /// Current state of the keyboard. See <see cref="Microsoft.Xna.Framework.Input.KeyboardState"/>
        /// </summary>
        [NonSerialized]
        public KeyboardState KeyboardState;
        /// <summary>
        /// Previous frame's state of the keyboard. See <see cref="Microsoft.Xna.Framework.Input.KeyboardState"/>
        /// </summary>
        [NonSerialized]
        public KeyboardState PreviousKeyboardState;
        /// <summary>
        /// Current mouse's X position.
        /// </summary>
        [ImmediateLabel]
        public int MouseX { get; internal set; }
        /// <summary>
        /// Current mouse's Y position.
        /// </summary>
        [ImmediateLabel]
        public int MouseY { get; internal set; }
        /// <summary>
        /// Right click occured this frame.
        /// </summary>
        [ImmediateLabel]
        public bool MousePressed { get; internal set; }
        /// <summary>
        /// Current frame's state of the gamepad. See <see cref="Microsoft.Xna.Framework.Input.GamePadState"/>
        /// </summary>
        [NonSerialized]
        public GamePadState GPState;
        /// <summary>
        /// Previous frame's state of the gamepad. See <see cref="Microsoft.Xna.Framework.Input.GamePadState"/>
        /// </summary>
        [NonSerialized]
        public GamePadState PreviousGPState;
        /// <summary>
        /// Represent's the gamepad's capabilites. See <see cref="Microsoft.Xna.Framework.Input.GamePadState"/>
        /// </summary>
        [NonSerialized]
        public GamePadCapabilities Capabilities;

        /// <summary>
        /// Input component object. Initializes keyboard, mouse and gamepad states.
        /// </summary>
        /// <param name="entity"></param>
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
