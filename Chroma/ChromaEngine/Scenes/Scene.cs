using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework;

namespace Chroma
{
    public class Scene
    {
        private List<SceneLayer> layers = new List<SceneLayer>();
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public SceneLayer CreateLayer()
        {
            layers.Add(new SceneLayer("untitled" + layers.Count));
            return layers[layers.Count-1];
        }

        public List<SceneLayer> GetLayerList()
        {
            return layers;
        }

        public void BeforeUpdate(GameTime gameTime)
        {
            foreach (SceneLayer layer in layers)
            {
                layer.BeforeUpdate(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (SceneLayer layer in layers)
            {
                layer.Update(gameTime);
            }
        }

        public void AfterUpdate(GameTime gameTime)
        {
            foreach (SceneLayer layer in layers)
            {
                layer.AfterUpdate(gameTime);
            }
        }

        public void BeforeRender(GameTime gameTime)
        {
            foreach (SceneLayer layer in layers)
            {
                layer.Render(gameTime);
            }
        }

        public void Render(GameTime gameTime)
        {
            foreach (SceneLayer layer in layers)
            {
                layer.Render(gameTime);
            }
        }

        public void AfterRender(GameTime gameTime)
        {
            foreach (SceneLayer layer in layers)
            {
                layer.AfterRender(gameTime);
            }
        }
    }
}
