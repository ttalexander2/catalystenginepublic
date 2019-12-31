using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Graphics
{
    public interface ParticlePath
    {
        void CalculatePath(GameTime gameTime, Particle p);
    }

    public class ParticlePathLinear : ParticlePath
    {
        public void CalculatePath(GameTime gameTime, Particle p)
        {
            p.Angle += p.Emitter.Rand.Next(-1, 1) / 50;
            p.Position += p.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
