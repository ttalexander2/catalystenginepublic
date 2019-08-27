using System;
using System.Collections.Generic;
using Chroma.Engine.Graphics;
using Chroma.Engine.Physics;
using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;

namespace Chroma.Engine
{
    public class Scene
    {
        public Dictionary<int, Entity> Entites = new Dictionary<int, Entity>();
        public Dictionary<int, Sprite> Sprites = new Dictionary<int, Sprite>();
        public Dictionary<int, Collider> Colliders = new Dictionary<int, Collider>();
        public Dictionary<int, Alarm> Alarms = new Dictionary<int, Alarm>();

        private QuadTree quad = new QuadTree(new Rectangle(0, 0, (int)(Global.NativeWidth * Global.PixelScale), (int)(Global.NativeHeight * Global.PixelScale)));

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }

        #region [Loop]
        public void Start()
        {

        }

        public virtual void BeforeUpdate(GameTime gameTime)
        {
            foreach (KeyValuePair<int, Sprite> entry in Sprites) entry.Value.BeforeUpdate(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<int, Sprite> entry in Sprites) entry.Value.Update(gameTime);
            foreach (KeyValuePair<int, Alarm> entry in Alarms) entry.Value.Update(gameTime);
        }

        public virtual void AfterUpdate(GameTime gameTime)
        {
            foreach (KeyValuePair<int, Sprite> entry in Sprites) entry.Value.AfterUpdate(gameTime);
        }

        public virtual void BeforeRender(GameTime gameTime)
        {
            foreach (KeyValuePair<int, Sprite> entry in Sprites) entry.Value.BeforeRender(gameTime);
        }

        public virtual void Render(GameTime gameTime)
        {
            foreach (KeyValuePair<int, Sprite> entry in Sprites) entry.Value.Render(gameTime);
            foreach (KeyValuePair<int, Collider> entry in Colliders) entry.Value.Render(gameTime);
        }

        public virtual void AfterRender(GameTime gameTime)
        {
            foreach (KeyValuePair<int, Sprite> entry in Sprites) entry.Value.AfterRender(gameTime);
        }

        public virtual void End()
        {

        }
        #endregion


        
        public bool EntityCollision(int UID, Vector2 offset)
        {
            foreach (KeyValuePair<int, Entity> entry in Entites)
            {
                Collider collider;
                Colliders.TryGetValue(UID, out collider);
                if (collider.CollidesWith(Colliders[entry.Key], offset)){
                    return true;
                }
            }
            return false;
        }

        private void ProcessCollisions(GameTime gameTime)
        {
            /**
             * Any item that can be collided with implements the ICollidable interface.
             * 
             * I need to figure out how this works.
             

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
    */
        }

        public Entity GenEntity(int UID)
        {
            Entity entity;
            if (Entites.TryGetValue(UID, out entity))
            {
                if (entity != null)
                {
                    return entity;
                }
            }
            return null;
        }
    }
}