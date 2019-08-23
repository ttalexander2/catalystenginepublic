using Microsoft.Xna.Framework;
using System;

namespace Chroma.Engine.Physics
{
    public class Velocity : Component
    {
        public float X { get; set; }
        public float Y { get; set; }


        public Velocity(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public static Velocity operator +(Velocity v1, Velocity v2)
        {
            return new Velocity(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Velocity operator -(Velocity v1, Velocity v2)
        {
            return new Velocity(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Velocity operator *(Velocity v1, float m)
        {
            return new Velocity(v1.X * m, v1.Y * m);
        }

        public static Velocity operator /(Velocity v1, float m)
        {
            return new Velocity(v1.X/m, v1.Y/m);
        }

        public Velocity InnerMultiply(Velocity v1)
        {
            return new Velocity(v1.X * X, v1.Y * Y);
        }

        public Velocity InnerDivide(Velocity v1)
        {
            return new Velocity(v1.X * X, v1.Y * Y);
        }

        public float DistanceFrom(Velocity v1)
        {
            return (float)Math.Abs(Math.Sqrt((X-v1.X)* (X - v1.X) + (Y - v1.Y) * (Y - v1.Y)));
        }


        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }



        public override string ToString()
        {
            return "Velocity:{" + X + ", " + Y + "}";
        }
    }
}
