using System;
using System.Collections.Generic;
using System.Text;
using Catalyst.Engine;
using Microsoft.Xna.Framework;

namespace Catalyst.GameLogic
{
    public class EnemySystem : Catalyst.Engine.System
    {
        public EnemySystem(Scene scene) : base(scene)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (EnemyAI r in Manager.GetComponents<EnemyAI>().Values)
            {
                if (r.State == EnemyState.Attack)
                {
                    
                }
            }
        }

        public enum EnemyState
        {
            Attack,
            Jump,
            Move,
            Rest,
            Die
        }
    }
}
