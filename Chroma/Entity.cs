using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma
{
    class Entity
    {
        public bool active = true;
        public bool visible = true;
        public bool collidable = true;
        public Vector2 position;

        public virtual Scene scene { get; set; }

        public Entity(Vector2 position)
        {
            this.position = position;
        }


        public Entity()
        {
            this.position = Vector2.Zero;
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

        }

        public virtual void Update()
        {

        }

        public virtual void AfterUpdate()
        {

        }

        public virtual void Render()
        {

        }

        public virtual void DebugRender()
        {
            
        }
    }
}
