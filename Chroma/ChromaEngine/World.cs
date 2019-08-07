using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma
{
    public static class World
    {
        public static int EntityIdNum = 0;

        public static List<Scene> Scenes = new List<Scene>();

        public static Scene currentScene;


        public static void BeforeUpdate(GameTime gameTime)
        {
            currentScene.BeforeUpdate(gameTime);
        }
        public static void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);
        }

        public static void AfterUpdate(GameTime gameTime)
        {
            currentScene.AfterUpdate(gameTime);
        }
        public static void BeforeRender(GameTime gameTime)
        {
            currentScene.BeforeRender(gameTime);
        }

        public static void Render(GameTime gameTime)
        {
            currentScene.Render(gameTime);
        }

        public static void AfterRender(GameTime gameTime)
        {
            currentScene.AfterRender(gameTime);
        }
    }
}
