﻿using Catalyst.Engine.Physics;
using Catalyst.Engine.Rendering;
using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    [Serializable]
    public class Actor : Entity
    {

        private float _xRemainder;
        private float _yRemainder;

        public Sprite2 Sprite;

        public Actor(Scene scene) : base(scene)
        {

        }

        public void MoveX(float amount, Action onCollide)
        {
            _xRemainder += amount;
            int move = (int)Math.Round(_xRemainder, 0);

            if (move != 0)
            {
                _xRemainder -= move;
                int sign = Math.Sign(move);
                while (move != 0)
                {
                    if (Collider != null && CollideAt(Collider, CollisionMask.Solids, Position + new Vector2(sign, 0)))
                    {
                        //Hit a solid
                        onCollide?.Invoke();
                        break;
                    }
                    else
                    {
                        //No solid immediately beside us
                        Position.X += sign;
                        move -= sign;
                    }
                }
            }
        }

        public void MoveX(float amount)
        {
            MoveX(amount, null);
        }

        public void MoveY(float amount)
        {
            MoveY(amount, null);
        }

        public void MoveY(float amount, Action onCollide)
        {
            _yRemainder += amount;
            int move = (int)Math.Round(_yRemainder, 0);

            if (move != 0)
            {
                _yRemainder -= move;
                int sign = Math.Sign(move);
                while (move != 0)
                {
                    if (Collider != null && CollideAt(Collider, CollisionMask.Solids, Position + new Vector2(0, sign)))
                    {
                        //Hit a solid
                        onCollide?.Invoke();
                        break;
                    }
                    else
                    {
                        //No solid immediately beside us
                        Position.Y += sign;
                        move -= sign;
                    }
                }


            }
        }

        private bool CollideAt(Collider2D collider, CollisionMask mask, Vector2 position)
        {
            Rectangle pos = collider.Bounds;
            collider.Bounds.X = (int)position.X;
            collider.Bounds.Y = (int)position.Y;

            bool collision = Collider.System.CheckCollision(collider, mask);

            collider.Bounds = pos;

            return collision;
        }
    }
}
