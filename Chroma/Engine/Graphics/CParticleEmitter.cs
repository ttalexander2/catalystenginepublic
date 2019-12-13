using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Graphics
{
    public class CParticleEmitter : AComponent
    {
        public Vector2 Position { get; set; }
        public Vector2 PositionVariance { get; internal set; }
        public List<Particle> Particles { get; private set; }
        public Texture2D Texture { get; set; }
        public ParticleMode Mode { get; set; }
        public Color StartColor { get; internal set; }
        public Color EndColor { get; internal set; }
        public float StartAlpha { get; set; }
        public float EndAlpha { get; set; }
        public int Life { get; internal set; }
        public int LifeVariance { get; set; }
        public int Speed { get; internal set; }
        public int SpeedVariance { get; internal set; }
        public int Angle { get; internal set; }
        public int AngleVariance { get; internal set; }
        public int Amount { get; set; }
        public CParticleEmitter(Entity entity) : base(entity)
        {
            Particles = new List<Particle>();
            Mode = ParticleMode.Continuous;
            StartColor = Color.Lerp(Color.White, Color.Red, 0.01f);
            EndColor = Color.Lerp(Color.White, Color.Orange, 0.3f);
            StartAlpha = 1.0f;
            EndAlpha = 0.2f;
            Life = 2000;
            LifeVariance = 150;
            Angle = 180;
            AngleVariance = 30;
            Speed = 90;
            SpeedVariance = 20;
            Position = Vector2.Zero;
            PositionVariance = Vector2.Zero;
            Amount = 10;
        }

        public enum ParticleMode
        {
            OneShot,
            Continuous,
            Burst
        }

        public void Launch()
        {
            Random r = new Random();
            Particle p = new Particle(this.Position + new Vector2(r.Next((int)-PositionVariance.X, (int)PositionVariance.X), r.Next((int)-PositionVariance.Y, (int)PositionVariance.Y)), r.Next(Life - LifeVariance, Life + LifeVariance), r.Next(Angle - AngleVariance, Angle + AngleVariance), r.Next(Speed - SpeedVariance, Speed + SpeedVariance));
            p.StartColor = StartColor;
            p.EndColor = EndColor;
            p.StartAlpha = StartAlpha;
            p.EndAlpha = EndAlpha;
            Particles.Add(p);
        }
    }
}
