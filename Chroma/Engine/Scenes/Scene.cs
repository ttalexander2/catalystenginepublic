using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Scenes
{
    public class Scene
    {
        private readonly List<SceneLayer> _layers = new List<SceneLayer>();

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }

        public SceneLayer CreateLayer()
        {
            _layers.Add(new SceneLayer("untitled" + _layers.Count));
            return _layers[_layers.Count - 1];
        }

        public List<SceneLayer> GetLayerList()
        {
            return _layers;
        }

        public void BeforeUpdate(GameTime gameTime)
        {
            foreach (var layer in _layers) layer.BeforeUpdate(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var layer in _layers) layer.Update(gameTime);
        }

        public void AfterUpdate(GameTime gameTime)
        {
            foreach (var layer in _layers) layer.AfterUpdate(gameTime);
        }

        public void BeforeRender(GameTime gameTime)
        {
            foreach (var layer in _layers) layer.Render(gameTime);
        }

        public void Render(GameTime gameTime)
        {
            foreach (var layer in _layers) layer.Render(gameTime);
        }

        public void AfterRender(GameTime gameTime)
        {
            foreach (var layer in _layers) layer.AfterRender(gameTime);
        }
    }
}