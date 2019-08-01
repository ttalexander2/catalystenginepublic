using System;
namespace Chroma
{
    public class Component
    {
        public bool hasUpdate = true;
        public bool hasRender = true;

        public virtual void BeforeUpdate()
        {

        }
        public virtual void Update()
        {

        }

        public virtual void AfterUpdate()
        {

        }

        public virtual void BeforeRender()
        {

        }
        public virtual void Render()
        {

        }

        public virtual void AfterRender()
        {

        }
    }
}
