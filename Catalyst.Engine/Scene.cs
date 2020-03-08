using System;
using System.Collections.Generic;
using Catalyst.Engine.Rendering;
using Catalyst.Engine.Input;
using Catalyst.Engine.Physics;
using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using Catalyst.Engine.Audio;

namespace Catalyst.Engine
{
    [Serializable]
    [assembly: InternalsVisibleTo("System.Runtime.Serialization")]
    public class Scene
    {

        public string Name { get; set; }

        public ECManager Manager { get; private set; }

        public List<System> Systems { get; private set; }

        public Camera2D Camera { get; set; }

        [NonSerialized]
        private AudioManager _audio;

        public AudioManager Audio { get { return _audio; } private set { _audio = value; } }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public Utilities.Vector2 Dimensions { get; private set; }

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
            Manager = new ECManager();
            Systems = new List<System>();
            Dimensions = new Utilities.Vector2(width, height);
            Camera = new Camera2D(this, new Utilities.Vector2(width, height));
            this.Name = "scene_" + this.GetHashCode().ToString();
        }

        public Scene(int width, int height, string name)
        {
            Width = width;
            Height = height;
            Manager = new ECManager();
            Systems = new List<System>();
            Dimensions = new Utilities.Vector2(width, height);
            Camera = new Camera2D(this, new Utilities.Vector2(width, height));
            this.Name = name;
        }

        private Scene() { }

#region [Loop]
        public virtual void Initialize()
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                Systems[i].Initialize();
            }
            Audio = new AudioManager();
        }

        public virtual void LoadContent()
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i] is RenderSystem)
                {
                    ((RenderSystem)Systems[i]).LoadContent();
                }
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
                System system = Systems[i];
                if (system.Renders)
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
        }

        public virtual void Render(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                System system = Systems[i];
                if (system.Renders)
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
        }

        public virtual void PostRender(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                System system = Systems[i];
                if (system.Renders)
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
        }

        public virtual void RenderUI(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                System system = Systems[i];
                if (system.Renders)
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
        }

        public virtual void End()
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                Systems[i].End(null);
            }
        }

        #endregion
    }


}