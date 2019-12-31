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
        public Entity Follow { get; set; }
        public bool FollowCamera { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 PositionVariance { get; set; }
        public Particle.VelocityMode VelocityMode { get; set; }
        public Vector2 Offset { get; set; }
        public List<Particle> Particles { get; private set; }
        public Texture2D Texture { get; set; }
        public ParticleMode Mode { get; set; }
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public float StartAlpha { get; set; }
        public float EndAlpha { get; set; }
        public int Life { get; set; }
        public int LifeVariance { get; set; }
        public int Speed { get; internal set; }
        public int SpeedVariance { get; set; }
        public int Angle { get; set; }
        public int AngleVariance { get; set; }
        public int Count { get; set; }
        internal Random Rand { get; private set; }
        public CParticleEmitter(Entity entity) : base(entity)
        {
            Particles = new List<Particle>();
            Mode = ParticleMode.Continuous;
            VelocityMode = Particle.VelocityMode.RecalculateAngle;
            StartColor = Color.Lerp(Color.Cyan, Color.White, 0.5f);
            EndColor = Color.Lerp(Color.White, Color.White, 01.0f);
            StartAlpha = 1.0f;
            EndAlpha = 0.0f;
            Life = 200;
            LifeVariance = 50;
            Angle = 90;
            AngleVariance = 45;
            Speed = 50;
            SpeedVariance = 40;
            Position = Vector2.Zero;
            PositionVariance = Vector2.Zero;
            Offset = Vector2.Zero;
            Count = 1000;
            Rand = new Random();
            Init(Count);


            FollowCamera = false;

            Launch();
        }

        public enum ParticleMode
        {
            Continuous,
            Burst
        }

        public void Init(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Particle p = new Particle(this);
                Particles.Add(p);
            }
        }

        public void Launch()
        {
            foreach(Particle p in Particles)
            {
                p.Reset();
                if(Mode == ParticleMode.Continuous)
                {
                    p.Life = Rand.Next(0, p.LifeStart);
                }

            }
        }
    }
}
