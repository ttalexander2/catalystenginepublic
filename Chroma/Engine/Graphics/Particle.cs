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
        private CParticleEmitter _emitter { get; set; }
        public bool Active { get; internal set; }
        public Vector2 Position { get; internal set; }
        public Vector2 Velocity
        {
            get 
            {
                if (Mode == VelocityMode.Exponential)
                {
                    return Position * new Vector2((float)(Speed * Math.Cos(Angle * Math.PI / 180)) / 500, (float)(-Speed * Math.Sin(Angle * Math.PI / 180)) / 500);
                }
                else
                {
                    return _velocity;
                }
            }
            private set { _velocity = value; }
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
            Exponential
        }

        public Particle(CParticleEmitter emitter)
        {
            this._emitter = emitter;
            this.Active = false;
        }

        public void Reset()
        {
            this.Active = true;
            this.Position = _emitter.Position + new Vector2(_emitter.Rand.Next((int)-_emitter.PositionVariance.X, (int)_emitter.PositionVariance.X), _emitter.Rand.Next((int)-_emitter.PositionVariance.Y, (int)_emitter.PositionVariance.Y)) + _emitter.Offset;
            this.Life = _emitter.Rand.Next(_emitter.Life - _emitter.LifeVariance, _emitter.Life + _emitter.LifeVariance);
            this.LifeStart = this.Life;
            this.Angle = _emitter.Rand.Next(_emitter.Angle - _emitter.Rand.Next(0, _emitter.AngleVariance), _emitter.Angle + _emitter.Rand.Next(0, _emitter.AngleVariance));
            this.Speed = _emitter.Rand.Next(_emitter.Speed - _emitter.SpeedVariance, _emitter.Speed + _emitter.SpeedVariance);
            this.StartColor = _emitter.StartColor;
            this.EndColor = _emitter.EndColor;
            this.StartAlpha = _emitter.StartAlpha;
            this.EndAlpha = _emitter.EndAlpha;
            this.Velocity = Position * new Vector2((float)(Speed * Math.Cos(Angle * Math.PI / 180)) / 500, (float)(-Speed * Math.Sin(Angle * Math.PI / 180)) / 500);
            this.Mode = _emitter.VelocityMode;
        }


    }
}
