using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{

    public interface IInitialize
    {
        void Initialize();
    }

    public interface ILoad
    {
        void LoadContent();
        void UnloadContent();
    }

    public interface IUpdate
    {
        void Update(GameTime gameTime);
    }
    public interface IPreUpdate
    {
        void PreUpdate(GameTime gameTime);
    }
    public interface IPostUpdate
    {
        void PostUpdate(GameTime gameTime);
    }

    public interface IRender : ILoad
    {
        void Render(GameTime gameTime);
    }
    public interface IPreRender : ILoad
    {
        void PreRender(GameTime gameTime);
    }
    public interface IPostRender : ILoad
    {
        void PostRender(GameTime gameTime);
    }
    public interface IRenderUI : ILoad
    {
        void RenderUI(GameTime gameTime);
    }

    public interface IDebugRender
    {
        void DebugRender(GameTime gameTime);
    }
}
