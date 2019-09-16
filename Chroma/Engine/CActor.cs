using Chroma.Engine.Physics;
using Microsoft.Xna.Framework;
using System;

namespace Chroma.Engine
{
    public class CActor : AComponent
    {
        public static new string Name => "Actor";

        internal CActor(int UID) : base(UID)
        {

        }
  
    }
}
