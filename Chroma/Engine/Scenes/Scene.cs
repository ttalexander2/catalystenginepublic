using System;
using System.Collections.Generic;
using Chroma.Engine.Collision;
using Chroma.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Scenes
{
    public class Scene
    {
        private readonly List<SceneLayer> _layers = new List<SceneLayer>();
        private QuadTree quad = new QuadTree(new Rectangle(0, 0, (int)(Global.NativeWidth * Global.Scale), (int)(Global.NativeHeight * Global.Scale)));

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
            for (int i = 0; i < _layers.Count; i++) _layers[i].BeforeUpdate(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            ProcessCollisions(gameTime);
            for (int i = 0; i < _layers.Count; i++) _layers[i].Update(gameTime);
        }

        public void AfterUpdate(GameTime gameTime)
        {
            for (int i = 0; i < _layers.Count; i++) _layers[i].AfterUpdate(gameTime);
        }

        public void BeforeRender(GameTime gameTime)
        {
            for (int i = 0; i < _layers.Count; i++) _layers[i].Render(gameTime);
        }

        public void Render(GameTime gameTime)
        {
            for (int i = 0; i < _layers.Count; i++) _layers[i].Render(gameTime);
        }

        public void AfterRender(GameTime gameTime)
        {
            for (int i = 0; i < _layers.Count; i++) _layers[i].AfterRender(gameTime);
        }

        private void ProcessCollisions(GameTime gameTime)
        {
            /**
             * Any item that can be collided with implements the ICollidable interface.
             * 
             * I need to figure out how this works.
             */

            quad.Clear();
            for (int i = 0; i < _layers.Count; i++)
            {
                SceneLayer layer = _layers[i];
                if (layer.hasCollisions)
                {
                    for (int j = 0; j < layer.Sprites.Count; j++)
                    {
                        if (layer.Sprites[j].collidable)
                        {
                            quad.insert(layer.Sprites[j]);
                        }
                    }
                }
            }
            List<Sprite> returnObjects = new List<Sprite>();
            for (int i = 0; i < _layers.Count; i++)
            {
                SceneLayer layer = _layers[i];
                if (layer.hasCollisions)
                {
                    for (int j = 0; j < layer.Sprites.Count; j++)
                    {
                        if (layer.Sprites[j].collidable)
                        {
                            returnObjects.Clear();
                            quad.retrieve(returnObjects, layer.Sprites[j]);

                            for (int x = 0; x < returnObjects.Count; x++)
                            {
                                //Check Collision
                            }
                        }
                    }
                }
            }
        }
    }
}