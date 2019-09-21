using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Chroma.Engine
{
    [Serializable]
    public class World
    {

        public List<Scene> Scenes = new List<Scene>();

        public Scene CurrentScene { get; set; }
        public ECManager Manager { get
            {
                return CurrentScene.Manager;
            }
        }

        public void Start()
        {
        }


        public void BeforeUpdate(GameTime gameTime)
        {
            CurrentScene?.PreUpdate(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            CurrentScene?.Update(gameTime);
#if DEBUG
            if (CurrentScene == null) Console.WriteLine("World.currentScene is NULL. Update/Render could not occur.");
#endif
        }

        public void AfterUpdate(GameTime gameTime)
        {
            CurrentScene?.PostUpdate(gameTime);
        }

        public void BeforeRender(GameTime gameTime)
        {
            CurrentScene?.PreUpdate(gameTime);
        }

        public void Render(GameTime gameTime)
        {
            CurrentScene?.Render(gameTime);
        }

        public void AfterRender(GameTime gameTime)
        {
            CurrentScene?.PostUpdate(gameTime);
        }

        public void End()
        {
        }
    }
}