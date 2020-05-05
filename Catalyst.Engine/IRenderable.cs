using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    public interface IRenderable
    {
        void LoadContent();
        void UnloadContent();
        void PreRender(GameTime gameTime);
        void Render(GameTime gameTime);
        void PostRender(GameTime gameTime);
        void RenderUI(GameTime gameTime);
    }
}
