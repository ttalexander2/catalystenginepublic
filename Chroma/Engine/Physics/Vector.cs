using Microsoft.Xna.Framework;
using System;

namespace Chroma.Engine.Physics
{
    public class Vec2
    {
        public float X { get; set; }
        public float Y { get; set; }


        public Vec2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public static Vec2 operator +(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vec2 operator -(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vec2 operator *(Vec2 v1, float m)
        {
            return new Vec2(v1.X * m, v1.Y * m);
        }

        public static Vec2 operator /(Vec2 v1, float m)
        {
            return new Vec2(v1.X/m, v1.Y/m);
        }

        public static Vec2 InnerMultiply(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static Vec2 InnerDivide(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.X / v2.X, v1.Y / v2.Y);
        }

        public static float DistanceFrom(Vec2 v1, Vec2 v2)
        {
            return (float)Math.Abs(Math.Sqrt((v2.X-v1.X)* (v2.X - v1.X) + (v2.Y - v1.Y) * (v2.Y - v1.Y)));
        }


        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }


        public override string ToString()
        {
            return "Vec2:{" + X + ", " + Y + "}";
        }
    }
}
