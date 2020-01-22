using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Chroma.Engine
{
    [KnownType("DerivedTypes")]
    [Serializable]
    public abstract class AComponent
    {
        public static string Name => "Abstract Component";
        
        public int UID { get; internal set; }
        
        public bool Active { get; set; }
        
        public Entity Entity { get; internal set; }

        internal AComponent(Entity entity)
        {
            this.Entity = entity;
            this.UID = entity.UID;
            this.Active = true;
        }

        private static Type[] DerivedTypes()
        {
            return Assembly.GetAssembly(typeof(AComponent)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AComponent))).ToArray();
        }
    }
}