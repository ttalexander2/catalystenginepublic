using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Vector2 = Catalyst.Engine.Utilities.Vector2;
using Color = Catalyst.Engine.Utilities.Color;
using Rectangle = Catalyst.Engine.Utilities.Rectangle;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public class ParticleEmitter : AComponent
    {
        [ImmediateEntitySelector]
        public Entity Follow { get; set; }
        [ImmediateBoolean]
        public bool FollowCamera { get; set; }

        public Vector2 Position { get; set; }
        [ImmediateVector2]
        public Vector2 PositionVariance { get; set; }
        [ImmediateEnum]
        public Particle.VelocityMode VelocityMode { get; set; }
        [ImmediateVector2]
        public Vector2 Offset { get; set; }

        [NonSerialized]
        private List<Particle> _particles;
        public List<Particle> Particles
        {
            get
            {
                return _particles;
            }
        }

        public MTexture Texture { get; set; }
        [ImmediateEnum]
        public ParticleMode Mode { get; set; }
        [ImmediateColor]
        public Color StartColor { get; set; }
        [ImmediateColor]
        public Color EndColor { get; set; }
        [ImmediateFloat(ImmediateFloatMode.Slider, 0, 1)]
        public float StartAlpha { get; set; }
        [ImmediateFloat(ImmediateFloatMode.Slider, 0, 1)]
        public float EndAlpha { get; set; }
        [ImmediateInteger(0, 999999)]
        public int Life { get; set; }
        [ImmediateInteger(0, 999999)]
        public int LifeVariance { get; set; }
        [ImmediateInteger(ImmediateIntegerMode.Slider, 0, 1000)]
        public int Speed { get; internal set; }
        [ImmediateInteger(ImmediateIntegerMode.Slider, 0, 1000)]
        public int SpeedVariance { get; set; }
        [ImmediateFloat(ImmediateFloatMode.Angle)]
        public float Angle { get; set; }
        [ImmediateFloat(ImmediateFloatMode.Angle)]
        public float AngleVariance { get; set; }
        [ImmediateInteger(ImmediateIntegerMode.Slider, 0, 10000)]
        public int Count { get; set; }
        internal Random Rand { get; private set; }
        public ParticleEmitter(Entity entity) : base(entity)
        {
            Mode = ParticleMode.Continuous;
            VelocityMode = Particle.VelocityMode.Linear;
            StartColor = Color.Lerp(Color.Cyan, Color.White, 0.5f);
            EndColor = Color.Lerp(Color.White, Color.White, 01.0f);
            StartAlpha = 1.0f;
            EndAlpha = 0.0f;
            Life = 200;
            LifeVariance = 0;
            Angle = 0;
            AngleVariance = 0;
            Speed = 50;
            SpeedVariance = 0;
            Position = Vector2.Zero;
            PositionVariance = Vector2.Zero;
            Offset = Vector2.Zero;
            Count = 1000;
            Rand = new Random();
            FollowCamera = false;
            Initialize();




            Launch();
        }
        [Serializable]
        public enum ParticleMode
        {
            Continuous,
            Burst
        }

        public void Initialize()
        {
            _particles = new List<Particle>();
            for (int i = 0; i < Count; i++)
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
