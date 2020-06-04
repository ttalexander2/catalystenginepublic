using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.IO;
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
        
        public bool Active { get; set; }
        
        public Entity Entity { get; internal set; }

        protected Component(Entity entity)
        {
            this.Entity = entity;
            this.UID = entity.UID;
            this.Active = true;
            this.Entity.Scene.Manager.Components[this.GetType().AssemblyQualifiedName][UID] = this;
            this.Entity.ComponentTypes.Add(this.GetType().AssemblyQualifiedName);
        }

        protected Component(Entity entity, Type type)
        {
            this.Entity = entity;
            this.UID = entity.UID;
            this.Active = true;
            this.Entity.Scene.Manager.Components[type.AssemblyQualifiedName][UID] = this;
            this.Entity.ComponentTypes.Add(type.AssemblyQualifiedName);
        }

        private static Type[] DerivedTypes()
        {
            return Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))).ToArray();
        }
    }
}