﻿using Catalyst.Engine.Utilities;
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
        public static string Name => "Abstract Component";
        
        public int UID { get; internal set; }
        
        public bool Active { get; set; }
        
        public Entity Entity { get; internal set; }

        public Component(Entity entity)
        {
            this.Entity = entity;
            this.UID = entity.UID;
            this.Active = true;
        }

        private static Type[] DerivedTypes()
        {
            return Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))).ToArray();
        }
    }
}