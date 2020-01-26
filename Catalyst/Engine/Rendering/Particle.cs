using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Catalyst.Engine.Utilities.Vector2;
using Vector3 = Catalyst.Engine.Utilities.Vector3;
using Vector4 = Catalyst.Engine.Utilities.Vector4;
using Color = Catalyst.Engine.Utilities.Color;
using Rectangle = Catalyst.Engine.Utilities.Rectangle;

namespace Catalyst.Engine.Rendering
{
    public class Particle
    {
        public ParticleEmitter Emitter { get; set; }
        public bool Active { get; internal set; }
        public Vector2 Position { get; internal set; }
        public Vector2 Velocity
        {
            get 
            {
                if (Mode == VelocityMode.RecalculateAngle)
                {
                    return new Vector2((float)(Speed * Math.Cos(Angle * Math.PI / 180)), (float)(-Speed * Math.Sin(Angle * Math.PI / 180)));
                }
                else
                {
                    return _velocity;
                }
            }
            internal set { _velocity = value; }
        }
        private Vector2 _velocity;
        public VelocityMode Mode { get; internal set; }
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

        public float StartAlpha { get; internal set; }
        public float EndAlpha { get; internal set; }
        public float Alpha
        {
            get
            {
                return MathHelper.Lerp(StartAlpha, EndAlpha, (float)(LifeStart - Life) / (float)LifeStart);
            }
        }

        public enum VelocityMode
        {
            Linear,
            RecalculateAngle
        }

        public Particle(ParticleEmitter emitter)
        {
            this.Emitter = emitter;
            this.Active = false;
        }

        public void Reset()
        {
            this.Active = true;
            this.Position = Emitter.Position + new Vector2(Emitter.Rand.Next((int)-Emitter.PositionVariance.X, (int)Emitter.PositionVariance.X), Emitter.Rand.Next((int)-Emitter.PositionVariance.Y, (int)Emitter.PositionVariance.Y)) + Emitter.Offset;
            this.Life = Emitter.Rand.Next(Emitter.Life - Emitter.LifeVariance, Emitter.Life + Emitter.LifeVariance);
            this.LifeStart = this.Life;
            this.Angle = Emitter.Rand.Next((int)Emitter.Angle - Emitter.Rand.Next(0, (int)Emitter.AngleVariance), (int)Emitter.Angle + Emitter.Rand.Next(0, (int)Emitter.AngleVariance));
            this.Speed = Emitter.Rand.Next(Emitter.Speed - Emitter.SpeedVariance, Emitter.Speed + Emitter.SpeedVariance);
            this.StartColor = Emitter.StartColor;
            this.EndColor = Emitter.EndColor;
            this.StartAlpha = Emitter.StartAlpha;
            this.EndAlpha = Emitter.EndAlpha;
            this.Mode = Emitter.VelocityMode;
            if (this.Mode == VelocityMode.Linear)
            {
                this.Velocity = new Vector2((float)(Speed * Math.Cos(Angle * Math.PI / 180)), (float)(-Speed * Math.Sin(Angle * Math.PI / 180)));
            }
            else
            {
                this.Velocity = this.Position * new Vector2((float)(Speed * Math.Cos(Angle * Math.PI / 180)) / 500, (float)(-Speed * Math.Sin(Angle * Math.PI / 180)) / 500);
            }
            

        }


    }
}
