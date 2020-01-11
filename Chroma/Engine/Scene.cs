using System;
using System.Collections.Generic;
using Chroma.Engine.Graphics;
using Chroma.Engine.Input;
using Chroma.Engine.Physics;
using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace Chroma.Engine
{
[Serializable]
    [assembly: InternalsVisibleTo("System.Runtime.Serialization")]
    public class Scene 
    {
        
        public string Name { get; set; }
        
        public ECManager Manager { get; private set; }
        
        public List<ASystem> Systems { get; private set; }
        
        public Camera2D Camera { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public Utilities.Vector2 Dimensions { get; private set; }

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
            Manager = new ECManager();
            Systems = new List<ASystem>();
            Dimensions = new Utilities.Vector2(width, height);
            Camera = new Camera2D(this, new Utilities.Vector2(width, height));
            this.Name = "scene_" + this.GetHashCode().ToString();
        }

        public Scene(int width, int height, string name)
        {
            Width = width;
            Height = height;
            Manager = new ECManager();
            Systems = new List<ASystem>();
            Dimensions = new Utilities.Vector2(width, height);
            Camera = new Camera2D(this, new Utilities.Vector2(width, height));
            this.Name = name;
        }

        private Scene() { }

#region [Loop]
        public void Initialize()
        {
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

        public virtual void RenderNative(GameTime gameTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                ASystem system = Systems[i];
                if (system.Renders)
                {
                    try
                    {
                        ((ARenderSystem)system).RenderNative(gameTime);
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
                ASystem system = Systems[i];
                if (system.Renders)
                {
                    try
                    {
                        ((ARenderSystem)system).RenderUI(gameTime);
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
    }


}