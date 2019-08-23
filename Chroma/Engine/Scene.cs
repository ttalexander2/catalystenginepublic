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
        public Dictionary<Guid, Actor> Actors = new Dictionary<Guid, Actor>();
        public Dictionary<Guid, Solid> Solids = new Dictionary<Guid, Solid>();
        public Dictionary<Guid, Sprite> Sprites = new Dictionary<Guid, Sprite>();
        public Dictionary<Guid, Collider> Colliders = new Dictionary<Guid, Collider>();
        public Dictionary<Guid, Alarm> Alarms = new Dictionary<Guid, Alarm>();

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
            foreach (KeyValuePair<Guid, Sprite> entry in Sprites) entry.Value.BeforeUpdate(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<Guid, Sprite> entry in Sprites) entry.Value.Update(gameTime);
            foreach (KeyValuePair<Guid, Alarm> entry in Alarms) entry.Value.Update(gameTime);
        }

        public virtual void AfterUpdate(GameTime gameTime)
        {
            foreach (KeyValuePair<Guid, Sprite> entry in Sprites) entry.Value.AfterUpdate(gameTime);
        }

        public virtual void BeforeRender(GameTime gameTime)
        {
            foreach (KeyValuePair<Guid, Sprite> entry in Sprites) entry.Value.BeforeRender(gameTime);
        }

        public virtual void Render(GameTime gameTime)
        {
            foreach (KeyValuePair<Guid, Sprite> entry in Sprites) entry.Value.Render(gameTime);
            foreach (KeyValuePair<Guid, Collider> entry in Colliders) entry.Value.Render(gameTime);
        }

        public virtual void AfterRender(GameTime gameTime)
        {
            foreach (KeyValuePair<Guid, Sprite> entry in Sprites) entry.Value.AfterRender(gameTime);
        }

        public virtual void End()
        {

        }
        #endregion



        public bool ActorCollideWithSolid(Guid UID, Vector2 offset)
        {
            foreach (KeyValuePair<Guid, Solid> entry in Solids)
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

        public Actor GetActor(Guid UID)
        {
            Actor actor;
            if (Actors.TryGetValue(UID, out actor))
            {
                if (actor != null)
                {
                    return actor;
                }
            }
            return null;
        }
    }
}