using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalyst.Engine.Physics;
using Catalyst.Engine.Utilities;
using Catalyst.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public class ParticleSystem : RenderSystem
    {
        public ParticleSystem(Scene scene) : base(scene){}

        public override void Initialize()
        {
            base.Initialize();
            foreach (ParticleEmitter emitter in Manager.GetComponents<ParticleEmitter>().Values)
            {
                emitter.Initialize();


            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (ParticleEmitter emitter in Manager.GetComponents<ParticleEmitter>().Values)
            {
                if (emitter.Follow != null)
                {
                    emitter.Position = emitter.Follow.Position;
                }

                if (emitter.FollowCamera)
                {
                    emitter.Position = scene.Camera.Position;
                }

                foreach (Particle p in emitter.Particles)
                {
                    if (p.Active)
                    {
                        p.Life--;
                        if (p.Life > 0)
                        {
                            p.Velocity += new Utilities.Vector2(0, -emitter.Gravity);
                            p.Position += p.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {

                            if (emitter.Mode == ParticleEmitter.ParticleMode.Continuous)
                            {
                                p.Reset(emitter);
                            }
                            else
                            {
                                p.Active = false;
                            }
                        }
                    }
                }

                if (emitter.Particles.Count != emitter.Count)
                {
                    emitter.Initialize();
                    emitter.Launch();
                }


            }
        }
        public override void PostRender(GameTime gameTime)
        {
            foreach (ParticleEmitter emitter in Manager.GetComponents<ParticleEmitter>().Values)
            {
                foreach (Particle p in emitter.Particles)
                {
                    if (p.Active)
                        Graphics.SpriteBatch.Draw(emitter.Sprite.Texture, p.Position, emitter.Sprite.TextureRect, p.Color * p.Alpha, 0, Microsoft.Xna.Framework.Vector2.Zero, 1, new SpriteEffects(), 0);
                }

            }
        }
    }
}
