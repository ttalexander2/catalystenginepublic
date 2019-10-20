using Chroma.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Game
{
    [Serializable]
    public class CPlayer : AComponent
    {
        public int HorizontalSpeed = 5;
        public int VerticalSpeed = 5;

        public static new string Name => "Player Component";
        public CPlayer(Entity entity) : base(entity)
        {

        }
    }
}
