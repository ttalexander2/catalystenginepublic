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
    [Serializable]
    public class Particle
    {
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
            this.Active = false;
        }

        public void Reset(ParticleEmitter emitter)
        {
            this.Active = true;
            this.Position = emitter.Position + new Vector2(emitter.Rand.Next((int)-emitter.PositionVariance.X, (int)emitter.PositionVariance.X), emitter.Rand.Next((int)-emitter.PositionVariance.Y, (int)emitter.PositionVariance.Y)) + emitter.Offset;
            this.Life = emitter.Rand.Next(emitter.Life - emitter.LifeVariance, emitter.Life + emitter.LifeVariance);
            this.LifeStart = this.Life;
            int min = (int)(emitter.Angle * 180 / Math.PI) - emitter.Rand.Next(0, (int)(emitter.AngleVariance * 180 / Math.PI));
            int max = (int)(emitter.Angle * 180 / Math.PI) + emitter.Rand.Next(0, (int)(emitter.AngleVariance * 180 / Math.PI));
            if (min<=max)
                this.Angle = emitter.Rand.Next(min, max)*Math.PI/180;
            else
                this.Angle = emitter.Rand.Next(max, min) * Math.PI / 180;
            this.Speed = emitter.Rand.Next(emitter.Speed - emitter.SpeedVariance, emitter.Speed + emitter.SpeedVariance);
            this.StartColor = emitter.StartColor;
            this.EndColor = emitter.EndColor;
            this.StartAlpha = emitter.StartAlpha;
            this.EndAlpha = emitter.EndAlpha;
            this.Mode = emitter.VelocityMode;
            if (this.Mode == VelocityMode.Linear)
            {
                this.Velocity = new Vector2((float)(Speed * Math.Cos(Angle)), (float)(-Speed * Math.Sin(Angle)));
                this.Velocity += emitter.InitialForce;
            }
            else
            {
                this.Velocity = this.Position * new Vector2((float)(Speed * Math.Cos(Angle)) / 500, (float)(-Speed * Math.Sin(Angle)) / 500);
            }
            

        }


    }
}
