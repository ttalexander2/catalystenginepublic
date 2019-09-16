using System;
using System.Collections.Generic;
using Chroma.Engine.Graphics;
using Chroma.Engine.Input;
using Chroma.Engine.Physics;
using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;

namespace Chroma.Engine
{
    public class Scene
    {
        public ECManager Manager { get; private set; }
        public List<ASystem> Systems { get; private set; }

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
            Manager = new ECManager(this);
            Systems = new List<ASystem>();
        }

        public int Width { get; }
        public int Height { get; }

        #region [Loop]
        public void Initialize()
        {
            Systems.Add(new InputSystem(this));
            for (int i = 0; i < Systems.Count; i++)
            {
                Systems[i].Initialize();
            }
        }

        public virtual void PreUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                Systems[i].PreUpdate(gameTime);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                Systems[i].Update(gameTime);
            }
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                Systems[i].PostUpdate(gameTime);
            }
        }

        public virtual void PreRender(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                ASystem system = Systems[i];
                if (system.Renders)
                {
                    try {
                        ((ARenderSystem)system).PreRender(gameTime);
                    } catch (InvalidCastException e)
                    {
#if DEBUG
                        Console.WriteLine("Couldn't cast system to Render system! " + e.ToString());
#endif
                    }
                    
                }
            }
        }

        public virtual void Render(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                ASystem system = Systems[i];
                if (system.Renders)
                {
                    try
                    {
                        ((ARenderSystem)system).Render(gameTime);
                    }
                    catch (InvalidCastException e)
                    {
#if DEBUG
                        Console.WriteLine("Couldn't cast system to Render system! " + e.ToString());
#endif
                    }

                }
            }
        }

        public virtual void PostRender(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                ASystem system = Systems[i];
                if (system.Renders)
                {
                    try
                    {
                        ((ARenderSystem)system).PostRender(gameTime);
                    }
                    catch (InvalidCastException e)
                    {
#if DEBUG
                        Console.WriteLine("Couldn't cast system to Render system! " + e.ToString());
#endif
                    }

                }
            }
        }

        public virtual void End()
        {

        }
#endregion


        /**
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
    
        }

        public Entity GenEntity(int UID)
        {
            Manager
        }

    */
    }
}