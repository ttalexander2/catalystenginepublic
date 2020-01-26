using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Input
{
    [Serializable]
    public static class Controller
    {
        public static Keys KUp = Keys.Up;
        public static Buttons GUp = Buttons.DPadUp;

        public static Keys KDown = Keys.Down;
        public static Buttons GDown = Buttons.DPadDown;

        public static Keys KLeft = Keys.Left;
        public static Buttons GLeft = Buttons.DPadLeft;

        public static Keys KRight = Keys.Right;
        public static Buttons GRight = Buttons.DPadRight;



    }
}
