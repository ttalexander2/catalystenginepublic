using System;
using System.Collections.Generic;
using Catalyst.Engine.Rendering;
using Microsoft.Xna.Framework;
using Catalyst.Engine.Audio;
using Newtonsoft.Json;

namespace Catalyst.Engine
{
    [Serializable]
    [assembly: InternalsVisibleTo("System.Runtime.Serialization")]
    public class Scene
    {
        public string Name { get; set; }

        public EntityManager Manager { get; private set; }

        public List<System> Systems { get; private set; }

        public CollisionSystem CollisionSystem;

        public Camera Camera { get; set; }

        public List<Camera> Cameras { get; private set; }

        [NonSerialized]
        private AudioManager _audio;

        public AudioManager Audio { get { return _audio; } private set { _audio = value; } }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public Utilities.Vector2 Dimensions { get; private set; }

        public FileTree<GameObject> HierarchyTree = new FileTree<GameObject>(true);

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
            Manager = new EntityManager(this);
            Systems = new List<System>();
            Cameras = new List<Camera>();
            Dimensions = new Utilities.Vector2(width, height);
            Camera = new Camera(this, new Utilities.Vector2(width, height));
            HierarchyTree.AddElement(Camera, Camera.Name);
            this.Name = "scene_" + this.GetHashCode().ToString();
            CollisionSystem = new CollisionSystem(this);
            Systems.Add(new CoroutineSystem(this));


        }

        public Scene(int width, int height, string name)
        {
            Width = width;
            Height = height;
            Manager = new EntityManager(this);
            Dimensions = new Utilities.Vector2(width, height);
            Camera = new Camera(this, new Utilities.Vector2(width, height));
            HierarchyTree.AddElement(Camera, Camera.Name);
            this.Name = name;
        }

        private Scene() { }

        public void NewCamera()
        {
            Camera c = new Camera(this, new Utilities.Vector2(Width, Height));
            Cameras.Add(c);
            HierarchyTree.AddElement(c, c.Name);

        }

        #region [Loop]
        public virtual void Initialize()
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].Active)
                    Systems[i].Initialize();
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                e.Initialize();
            }
            Audio = new AudioManager();
        }

        public virtual void LoadContent()
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i] is RenderSystem && Systems[i].Active)
                {
                    ((RenderSystem)Systems[i]).LoadContent();
                }
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                if (e is ILoad)
                {
                    ((ILoad)e).LoadContent();
                }
            }
        }

        public virtual void PreUpdate(GameTime gameTime)
        {
            CollisionSystem.PreUpdate(gameTime);
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].Active)
                    Systems[i].PreUpdate(gameTime);
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                if (e is IPreUpdate)
                {
                    ((IPreUpdate)e).PreUpdate(gameTime);
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].Active)
                    Systems[i].Update(gameTime);
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                if (e is IUpdate)
                {
                    ((IUpdate)e).Update(gameTime);
                }
            }
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].Active)
                    Systems[i].PostUpdate(gameTime);
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                if (e is IPostUpdate)
                {
                    ((IPostUpdate)e).PostUpdate(gameTime);
                }
            }
        }

        public virtual void PreRender(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                System system = Systems[i];
                if (system.Renders && system.Visible)
                {
                    try {
                        ((RenderSystem)system).PreRender(gameTime);
                    } catch (InvalidCastException e)
                    {
#if DEBUG
                        Console.WriteLine("Couldn't cast system to Render system! " + e.ToString());
#endif
                    }
                    
                }
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                if (e is IPreRender)
                {
                    ((IPreRender)e).PreRender(gameTime);
                }
            }
        }

        public virtual void Render(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                System system = Systems[i];
                if (system.Renders && system.Visible)
                {
                    try
                    {
                        ((RenderSystem)system).Render(gameTime);
                    }
                    catch (InvalidCastException e)
                    {
#if DEBUG
                        Console.WriteLine("Couldn't cast system to Render system! " + e.ToString());
#endif
                    }

                }
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                if (e is IRender)
                {
                    ((IRender)e).Render(gameTime);
                }
            }
        }

        public virtual void PostRender(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                System system = Systems[i];
                if (system.Renders && system.Visible)
                {
                    try
                    {
                        ((RenderSystem)system).PostRender(gameTime);
                    }
                    catch (InvalidCastException e)
                    {
#if DEBUG
                        Console.WriteLine("Couldn't cast system to Render system! " + e.ToString());
#endif
                    }

                }
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                if (e is IPostRender)
                {
                    ((IPostRender)e).PostRender(gameTime);
                }
            }
        }

        public virtual void DebugRender(GameTime gameTime)
        {
            CollisionSystem.DebugRender(gameTime);
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i] is IDebugRender)
                {
                    ((IDebugRender)Systems[i]).DebugRender(gameTime);
                }
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                if (e is IDebugRender)
                {
                    ((IDebugRender)e).DebugRender(gameTime);
                }
            }
        }

        public virtual void RenderUI(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                System system = Systems[i];
                if (system.Renders && system.Visible)
                {
                    try
                    {
                        ((RenderSystem)system).RenderUI(gameTime);
                    }
                    catch (InvalidCastException e)
                    {
#if DEBUG
                        Console.WriteLine("Couldn't cast system to Render system! " + e.ToString());
#endif
                    }

                }
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                if (e is IRenderUI)
                {
                    ((IRenderUI)e).RenderUI(gameTime);
                }
            }
        }

        public virtual void End()
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].Active)
                    Systems[i].End(null);
            }
        }

        public void UnloadContent()
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                System system = Systems[i];
                if (system.Renders && system.Active)
                {
                    try
                    {
                        ((RenderSystem)system).UnloadContent();
                    }
                    catch (InvalidCastException e)
                    {
#if DEBUG
                        Console.WriteLine("Couldn't cast system to Render system! " + e.ToString());
#endif
                    }

                }
            }
            foreach (Entity e in Manager.Entities.Values)
            {
                if (e is ILoad)
                {
                    ((ILoad)e).UnloadContent();
                }
            }
        }

        #endregion
    }


}