using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Chroma.Engine
{
    public class World
    {
        public int EntityIdNum = 0;

        public List<Scene> Scenes = new List<Scene>();

        public Scene currentScene;

        public void Start()
        {
        }


        public void BeforeUpdate(GameTime gameTime)
        {
            currentScene?.BeforeUpdate(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            currentScene?.Update(gameTime);
#if DEBUG
            if (currentScene == null) Console.WriteLine("World.currentScene is NULL. Update/Render could not occur.");
#endif
        }

        public void AfterUpdate(GameTime gameTime)
        {
            currentScene?.AfterUpdate(gameTime);
        }

        public void BeforeRender(GameTime gameTime)
        {
            currentScene?.BeforeRender(gameTime);
        }

        public void Render(GameTime gameTime)
        {
            currentScene?.Render(gameTime);
        }

        public void AfterRender(GameTime gameTime)
        {
            currentScene?.AfterRender(gameTime);
        }

        public void End()
        {
        }
    }
}