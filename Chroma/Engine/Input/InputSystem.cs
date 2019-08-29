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
        
        


        #region Keyboard
        public void UpdateKeyboard()
        {
            _previousState = _state;
            _state = Keyboard.GetState();
        }

        #endregion

        public void Initialize()
        {
            _previousState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            _previousState = _state;
            _state = Keyboard.GetState();
            
        }

        
    }
}
