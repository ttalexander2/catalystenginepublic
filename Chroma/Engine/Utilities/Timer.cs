using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Utilities
{
    public class Timer : Component
    {
        private TimeSpan _time;
        private IScript _script;
        private bool _loop;

        public Timer(IScript script, bool loop, GameTime gameTime, float seconds)
        {
            _script = script;
            _loop = loop;
            _time = new TimeSpan((int)seconds/10000000);
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
