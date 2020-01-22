using Chroma.Engine;
using Chroma.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Game
{
    [Serializable]
    public class Player : AComponent
    {
        [ImmediateInteger]
        public int HorizontalSpeed = 5;
        [ImmediateInteger]
        public int VerticalSpeed = 5;

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
