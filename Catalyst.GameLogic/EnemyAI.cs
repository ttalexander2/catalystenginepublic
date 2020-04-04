using Catalyst.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using static Catalyst.GameLogic.EnemySystem;

namespace Catalyst.GameLogic
{
    public class EnemyAI : Component
    {
        [GuiEnum]
        public EnemyState State { get; set; }
        [GuiInteger]
        public int Health { get; set; }
        public EnemyAI(Entity entity) : base(entity)
        {
        }
    }
}
