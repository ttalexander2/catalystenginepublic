using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma
{
    class Entity : IEnumerable<Component>, IEnumerable
    {
        public bool active = true;
        public bool visible = true;
        public bool collidable = true;

        public virtual Scene scene { get; set; }

        public ComponentList component { get; set; }


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

        public IEnumerator<Component> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
