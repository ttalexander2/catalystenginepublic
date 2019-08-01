using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma
{
    public class Entity
    {
        public bool active = true;
        public bool visible = true;
        public bool collidable = true;

        public virtual Scene scene { get; set; }

        public List<Component> components = new List<Component>();


        public Entity()
        {
            
        }


        public virtual void Added(Scene scene)
        {
            this.scene = scene;
        }

        public virtual void Removed(Scene scene)
        {
            this.scene = null;
        }


        public virtual void BeforeUpdate()
        {
            foreach (Component component in components)
            {
                if (component.hasRender)
                {
                    component.BeforeUpdate();
                }
            }
        }

        public virtual void Update()
        {
            foreach (Component component in components)
            {
                if (component.hasRender)
                {
                    component.Update();
                }
            }
        }

        public virtual void AfterUpdate()
        {
            foreach (Component component in components)
            {
                if (component.hasRender)
                {
                    component.AfterUpdate();
                }
            }
        }

        public virtual void BeforeRender()
        {
            foreach (Component component in components)
            {
                if (component.hasRender)
                {
                    component.BeforeRender();
                }

            }
        }

        public virtual void Render()
        {
            foreach(Component component in components)
            {
                if (component.hasRender)
                {
                    component.Render();
                }

            }
        }

        public virtual void AfterRender()
        {
            foreach (Component component in components)
            {
                if (component.hasRender)
                {
                    component.AfterRender();
                }

            }
        }
    }
}
