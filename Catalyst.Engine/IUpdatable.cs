using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    public interface IUpdatable
    {
        void Initialize();
        void PreUpdate(GameTime gameTime);
        void Update(GameTime gameTime);
        void PostUpdate(GameTime gameTime);
    }
}
