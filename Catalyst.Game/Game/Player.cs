using Catalyst.Engine;
using Catalyst.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Game
{
    [Serializable]
    public class Player : Component
    {
        [GuiInteger]
        public int HorizontalSpeed = 10;
        [GuiInteger]
        public int VerticalSpeed = 10;

        public static new string Name => "Player Component";
        public Player(Entity entity) : base(entity)
        {
            if (!entity.HasComponent<Input>())
            {
                entity.AddComponent<Input>();
            }
            if (!entity.HasComponent<Actor>())
            {
                entity.AddComponent<Actor>();
            }
        }
    }
}
