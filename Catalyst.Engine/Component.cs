using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Catalyst.Engine
{
    [KnownType("DerivedTypes")]
    [Serializable]
    public abstract class Component:GameObject
    {
        public static new string Name => "Component";
        
        public int UID { get; internal set; }
        
        public Entity Entity { get; internal set; }

        public Scene Scene { 
            get
            {
                return this.Entity.Scene;
            }
        }

        protected Component(Entity entity)
        {
            this.Entity = entity;
            this.UID = entity.UID;
            this.Active = true;
            this.Entity.Scene.Manager.Components[this.GetType().AssemblyQualifiedName][UID] = this;
            this.Entity.ComponentTypes.Add(this.GetType().AssemblyQualifiedName);
        }

        protected Component(Entity entity, Type t)
        {
            this.Entity = entity;
            this.UID = entity.UID;
            this.Active = true;
            this.Entity.Scene.Manager.Components[t.AssemblyQualifiedName][UID] = this;
            this.Entity.ComponentTypes.Add(t.AssemblyQualifiedName);
        }

        private static Type[] DerivedTypes()
        {
            return Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))).ToArray();
        }

        public void RemoveSelf()
        {
            Entity.RemoveComponent(this);
        }
    }
}