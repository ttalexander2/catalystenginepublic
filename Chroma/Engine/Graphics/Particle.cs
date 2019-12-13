using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Graphics
{
    public class Particle
    {
        public Vector2 Position { get; internal set; }
        public Vector2 Velocity
        { 
            get 
            {
                return Position * new Vector2((float)(Speed * Math.Cos(Angle * Math.PI / 180))/500, (float)(-Speed * Math.Sin(Angle * Math.PI / 180))/500);
            }
            private set { }
        }
        public int Life { get; internal set; }
        public int LifeStart { get; internal set; }
        public double Speed { get; internal set; }
        public double Angle { get; internal set; }
        public Color StartColor { get; internal set; }
        public Color EndColor { get; internal set; }
        public Color Color
        {
            get
            {
                return Color.Lerp(StartColor, EndColor, (float)(LifeStart - Life) / (float)LifeStart);
            }
        }

        public float StartAlpha { get; set; }
        public float EndAlpha { get; set; }
        public float Alpha
        {
            get
            {
                return MathHelper.Lerp(StartAlpha, EndAlpha, (float)(LifeStart - Life) / (float)LifeStart);
            }
        }

        public Particle(Vector2 pos, int life, int angle, int speed)
        {
            this.Position = pos;
            this.Life = life;
            this.LifeStart = life;
            this.Angle = angle;
            this.Speed = speed;
        }


    }
}
