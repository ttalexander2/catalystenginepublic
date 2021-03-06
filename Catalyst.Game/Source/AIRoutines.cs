﻿using Catalyst.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Game.Source
{
    public static class AIRoutines
    {
        public static IEnumerator MoveBackAndForth(this Actor actor, int speed)
        {
            yield return 0;


            double time = 0;
            int direction = 1;


            while (true)
            {
                if (time < 30)
                {
                    actor.MoveX(speed * direction * Time.DeltaTimeF);
                }
                else
                {
                    time = 0;
                    direction *= -1;
                }

                time += Time.DeltaTime;
                yield return 0f;

            }
        }
    }
}
