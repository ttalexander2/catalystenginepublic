using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chroma.Engine.Physics;
using Chroma.Engine.Utilities;
using Chroma.Game;
using Chroma.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chroma.Engine.Graphics
{
    public class ParticleSystem : ARenderSystem
    {
        public ParticleSystem(Scene scene) : base(scene){}

        public override void Update(GameTime gameTime)
        {
            foreach (CParticleEmitter emitter in Manager.GetComponents<CParticleEmitter>().Values)
            {

                if (emitter.Follow != null)
                {
                    emitter.Position = emitter.Follow.GetComponent<CTransform>().Position;
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
                            p.Position += p.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            if (emitter.Mode == CParticleEmitter.ParticleMode.Continuous)
                            {
                                p.Reset();
                            }
                            else
                            {
                                p.Active = false;
                            }
                        }
                    }

                }


            }
        }
        public override void PostRender(GameTime gameTime)
        {
            foreach (CParticleEmitter emitter in Manager.GetComponents<CParticleEmitter>().Values)
            {
                foreach (Particle p in emitter.Particles)
                {
                    Global.SpriteBatch.Draw(emitter.Texture, p.Position, null, p.Color * p.Alpha, 0, Vector2.Zero, 1, new SpriteEffects(), 0);
                }

            }
        }
    }
}
