using System;
using System.Collections.Generic;
using Chroma.Engine.Scenes;
using Microsoft.Xna.Framework;

namespace Chroma.Engine
{
    public static class World
    {
        public static int EntityIdNum = 0;

        public static List<Scene> Scenes = new List<Scene>();

        public static Scene currentScene;

        public static void Start()
        {
        }


        public static void BeforeUpdate(GameTime gameTime)
        {
            if (currentScene != null) currentScene.BeforeUpdate(gameTime);
        }

        public static void Update(GameTime gameTime)
        {
            if (currentScene != null) currentScene.Update(gameTime);
#if DEBUG
            if (currentScene == null) Console.WriteLine("World.currentScene is NULL. Update/Render could not occur.");
#endif
        }

        public static void AfterUpdate(GameTime gameTime)
        {
            if (currentScene != null) currentScene.AfterUpdate(gameTime);
        }

        public static void BeforeRender(GameTime gameTime)
        {
            if (currentScene != null) currentScene.BeforeRender(gameTime);
        }

        public static void Render(GameTime gameTime)
        {
            if (currentScene != null) currentScene.Render(gameTime);
        }

        public static void AfterRender(GameTime gameTime)
        {
            if (currentScene != null) currentScene.AfterRender(gameTime);
        }

        public static void End()
        {
        }
    }
}