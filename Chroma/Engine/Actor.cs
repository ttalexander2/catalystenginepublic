﻿using Chroma.Engine.Physics;
using Microsoft.Xna.Framework;
using System;

namespace Chroma.Engine
{
    public class Actor : Component
    {

        public float xRemainder { get; private set; }
        public float yRemainder { get; private set; }

        public Vector2 Position { get; internal set; }


        public Actor() : base()
        {

        }
        public void MoveX(Entity entity, Velocity velocity, Action onCollide)
        {
            xRemainder += velocity.X;
            int move = (int)Math.Round(xRemainder);

            if (move != 0)
            {
                xRemainder -= move;
                int sign = Math.Sign(move);

                
                while (move != 0)
                {
                    if (!World.currentScene.ActorCollideWithSolid(this.UID, Position + new Vector2(sign, 0)))
                    {
                        //No solid immediately beside us
                        Position += new Vector2(sign, 0);
                        move -= sign;
                    }
                    else
                    {
                        //Hit a solid!
                        onCollide?.Invoke();
                        break;
                    }
                }
            }
        }

        public void MoveY(float amount, Action onCollide)
        {
            yRemainder += amount;
            int move = (int)Math.Round(yRemainder);

            if (move != 0)
            {
                yRemainder -= move;
                int sign = Math.Sign(move);


                while (move != 0)
                {
                    if (!World.currentScene.ActorCollideWithSolid(UID, Position + new Vector2(0, sign)))
                    {
                        //No solid immediately beside us
                        Position += new Vector2(0, sign);
                        move -= sign;
                    }
                    else
                    {
                        //Hit a solid!
                        onCollide?.Invoke();
                        break;
                    }
                }
            }
        }
    }
}