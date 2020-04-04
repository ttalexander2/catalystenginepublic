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
using Catalyst.Engine.Physics;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public class ParticleEmitter : Component, Loadable
    {
        [GuiEntitySelector]
        public Entity Follow { get; set; }
        [GuiBoolean]
        public bool FollowCamera { get; set; }

        public Vector2 Position { get; set; }
        [GuiVector2]
        public Vector2 PositionVariance { get; set; }
        [GuiEnum]
        public Particle.VelocityMode VelocityMode { get; set; }
        [GuiVector2]
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

        public Sprite Sprite { get; set; }
        [GuiEnum]
        public ParticleMode Mode { get; set; }
        [GuiColor]
        public Color StartColor { get; set; }
        [GuiColor]
        public Color EndColor { get; set; }
        [GuiFloat(GuiFloatMode.Slider, 0, 1)]
        public float StartAlpha { get; set; }
        [GuiFloat(GuiFloatMode.Slider, 0, 1)]
        public float EndAlpha { get; set; }
        [GuiInteger(0, 999999)]
        public int Life { get; set; }
        [GuiInteger(0, 999999)]
        public int LifeVariance { get; set; }
        [GuiInteger(GuiIntegerMode.Slider, 0, 1000)]
        public int Speed { get; set; }
        [GuiInteger(GuiIntegerMode.Slider, 0, 1000)]
        public int SpeedVariance { get; set; }
        [GuiFloat(GuiFloatMode.Angle)]
        public float Angle { get; set; }
        [GuiFloat(GuiFloatMode.Angle)]
        public float AngleVariance { get; set; }
        [GuiInteger(GuiIntegerMode.Slider, 0, 100000)]
        public int Count { get; set; }

        [GuiFloat(GuiFloatMode.Slider, -200,200)]
        public float Gravity { get; set; }

        [GuiVector2]
        public Vector2 InitialForce { get; set; }
        [NonSerialized]
        private Random _random;
        internal Random Rand
        {
            get
            {
                if (_random == null)
                {
                    _random = new Random();
                }
                return _random;
            }
        }
        public ParticleEmitter(Entity entity) : base(entity)
        {
            if (!entity.HasComponent<Position>())
            {
                entity.AddComponent<Position>();
            }
            Sprite = new Sprite(entity, BasicShapes.GenerateCircleTexture(3, Color.White, 1));
            Mode = ParticleMode.Burst;
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
            Count = 50;
            Gravity = 0;
            FollowCamera = false;
            Follow = entity;
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

        [GuiButton("Launch Particles")]
        public void Launch()
        {
            foreach(Particle p in Particles)
            {
                p.Reset(this);
                if(Mode == ParticleMode.Continuous)
                {
                    p.Life = Rand.Next(0, Math.Abs(p.LifeStart));
                }
                p.Life = Rand.Next(0, Math.Abs(p.LifeStart));

            }
        }

        public void LoadContent()
        {
            Sprite = new Sprite(Entity, BasicShapes.GenerateCircleTexture(3, Color.White, 1));
            Initialize();
            if (Mode == ParticleMode.Continuous)
                Launch();
        }
    }
}
