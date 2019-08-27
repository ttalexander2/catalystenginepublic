using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Input
{
    public class InputSystem
    {
        private KeyboardState _state;
        private KeyboardState _previousState;

        /**
        TODO: This is probably a bad idea. Come up with a better solution
        */
        public enum Input
        {
            Up,
            Down,
            Left,
            Right,
            Start,
            A,
            B,
            X,
            Y,
            LT,
            RT,
            LB,
            RB
        }

        public void Initialize()
        {
            _previousState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            _previousState = _state;
            _state = Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                
            }
        }

        
    }
}
