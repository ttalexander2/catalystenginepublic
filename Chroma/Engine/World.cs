using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Chroma.Engine
{
    public class World
    {

        public List<Scene> Scenes = new List<Scene>();

        public Scene CurrentScene { get; set; }
        public ECManager Manager { get
            {
                return CurrentScene.Manager;
            }
        }

        public void Initialize()
        {
            CurrentScene?.Initialize();
        }


        public void PreUpdate(GameTime gameTime)
        {
            CurrentScene?.PreUpdate(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            CurrentScene?.Update(gameTime);
#if DEBUG
            if (CurrentScene == null) Console.WriteLine("World.CurrentScene is NULL. Update/Render could not occur.");
#endif
        }

        public void PostUpdate(GameTime gameTime)
        {
            CurrentScene?.PostUpdate(gameTime);
        }

        public void PreRender(GameTime gameTime)
        {
            CurrentScene?.PreRender(gameTime);
        }

        public void Render(GameTime gameTime)
        {
            CurrentScene?.Render(gameTime);
        }

        public void PostRender(GameTime gameTime)
        {
            CurrentScene?.PostRender(gameTime);
        }

        public void RenderNative(GameTime gameTime)
        {
            CurrentScene?.RenderNative(gameTime);
        }

        public void RenderUI(GameTime gameTime)
        {
            CurrentScene?.RenderUI(gameTime);
        }

        public void End()
        {
        }
    }
}