using System;
using System.Collections.Generic;
using Catalyst.Engine.Rendering;
using Microsoft.Xna.Framework;
using Catalyst.Engine.Audio;

namespace Catalyst.Engine
{
    [Serializable]
    [assembly: InternalsVisibleTo("System.Runtime.Serialization")]
    public class Scene : IUpdatable, IRenderable
    {

        public string Name { get; set; }

        public ECManager Manager { get; private set; }

        public List<System> Systems { get; private set; }

        public List<MonoEntity> MonoEntities { get; private set; }

        public Camera Camera { get; set; }

        public List<Camera> Cameras { get; private set; }

        [NonSerialized]
        private AudioManager _audio;

        public AudioManager Audio { get { return _audio; } private set { _audio = value; } }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public Utilities.Vector2 Dimensions { get; private set; }

        public FileTree<GameObject> HierarchyTree = new FileTree<GameObject>();

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
            Manager = new ECManager(this);
            Systems = new List<System>();
            Cameras = new List<Camera>();
            MonoEntities = new List<MonoEntity>();
            Dimensions = new Utilities.Vector2(width, height);
            Camera = new Camera(this, new Utilities.Vector2(width, height));
            HierarchyTree.AddElement(Camera, Camera.Name);
            this.Name = "scene_" + this.GetHashCode().ToString();
            AddMonoEntity(new Test(this));

        }

        public Scene(int width, int height, string name)
        {
            Width = width;
            Height = height;
            Manager = new ECManager(this);
            MonoEntities = new List<MonoEntity>();
            Dimensions = new Utilities.Vector2(width, height);
            Camera = new Camera(this, new Utilities.Vector2(width, height));
            HierarchyTree.AddElement(Camera, Camera.Name);
            this.Name = name;
        }

        private Scene() { }

        public void AddMonoEntity(MonoEntity entity)
        {
            MonoEntities.Add(entity);
            HierarchyTree.AddElement(entity, entity.Name);
        }

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
            for (int i = 0; i < MonoEntities.Count; i++)
            {
                if (MonoEntities[i].Active)
                    MonoEntities[i].Initialize();
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
            for (int i = 0; i < MonoEntities.Count; i++)
            {
                if (MonoEntities[i].Active)
                    MonoEntities[i].LoadContent();
            }
        }

        public virtual void PreUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].Active)
                    Systems[i].PreUpdate(gameTime);
            }
            for (int i = 0; i < MonoEntities.Count; i++)
            {
                if (MonoEntities[i].Active)
                    MonoEntities[i].PreUpdate(gameTime);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].Active)
                    Systems[i].Update(gameTime);
            }
            for (int i = 0; i < MonoEntities.Count; i++)
            {
                if (MonoEntities[i].Active)
                    MonoEntities[i].Update(gameTime);
            }
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].Active)
                    Systems[i].PostUpdate(gameTime);
            }
            for (int i = 0; i < MonoEntities.Count; i++)
            {
                if (MonoEntities[i].Active)
                    MonoEntities[i].PostUpdate(gameTime);
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
            for (int i = 0; i < MonoEntities.Count; i++)
            {
                if (MonoEntities[i].Visible)
                    MonoEntities[i].PreRender(gameTime);
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
            for (int i = 0; i < MonoEntities.Count; i++)
            {
                if (MonoEntities[i].Visible)
                    MonoEntities[i].Render(gameTime);
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
            for (int i = 0; i < MonoEntities.Count; i++)
            {
                if (MonoEntities[i].Visible)
                    MonoEntities[i].PostRender(gameTime);
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
            for (int i = 0; i < MonoEntities.Count; i++)
            {
                if (MonoEntities[i].Visible)
                    MonoEntities[i].RenderUI(gameTime);
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
            for (int i = 0; i < MonoEntities.Count; i++)
            {
                if (MonoEntities[i].Active)
                    MonoEntities[i].UnloadContent();
            }
        }

        #endregion
    }


}