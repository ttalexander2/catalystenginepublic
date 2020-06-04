using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    [KnownType("DerivedTypes")]
    [Serializable]
    public abstract class System : GameObject, IUpdate
    {
        
        protected Scene scene;

        protected ECManager Manager
        {
            get
            {
                return scene.Manager;
            }
        }
        public System(Scene scene)
        {
            this.scene = scene;
        }

        private System() { }

        public virtual bool Renders => false;
        public virtual void Initialize() { }
        public virtual void PreUpdate(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void PostUpdate(GameTime gameTime) { }
        public virtual void End(GameTime gameTime) { }
        public virtual void Save(GameTime gameTime) { }

        private static Type[] DerivedTypes()
        {
            return Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(System))).ToArray();
        }

    }
}
