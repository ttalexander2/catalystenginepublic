using Catalyst.Engine.Rendering;
using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Catalyst.Engine.FileTree<Catalyst.Engine.GameObject>;

namespace Catalyst.Engine
{
    [Serializable]
    public class EntityManager
    {
        
        private int _id;

        public Scene CurrentScene { get; private set; }
        
        internal Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();
        
        internal Dictionary<string, Dictionary<int, Component>> Components = new Dictionary<string, Dictionary<int, Component>>();

        public HashSet<string> CreatableTypes { get; private set; } = new HashSet<string>();


        internal EntityManager(Scene scene)
        {
            CurrentScene = scene;
            _id = 0;
            RefreshTypes();
            
        }

        public Entity NewEntity()
        {
            return new Entity(CurrentScene);
        }

        internal int NewId()
        {
            _id++;
            return _id - 1;
        }

        public Dictionary<int, Entity> GetEntities()
        {
            return Entities;
        }

        public Entity GetEntity(int UID)
        {
            Entity val;
            Entities.TryGetValue(UID, out val);
            return val;
        }
        public T GetComponent<T>(int UID) where T : Component
        {
            Type t = typeof(T);

            Component val;
            Components[t.AssemblyQualifiedName].TryGetValue(UID, out val);
            return val != null ? (T)val : null;
        }

        public T GetComponent<T>(Entity e) where T : Component
        {
            Type t = typeof(T);

            Component val;
            Components[t.AssemblyQualifiedName].TryGetValue(e.UID, out val);
            return val != null ? (T)val : null;
        }

        public Dictionary<int, Component> GetComponents<T>() where T : Component
        {
            Type t = typeof(T);
            return Components[t.AssemblyQualifiedName];
        }

        public Dictionary<string, Dictionary<int, Component>> GetComponentDictionary()
        {
            return Components;
        }

        public void RemoveComponent(Component c)
        {
            Type t = c.GetType();
            Components[t.AssemblyQualifiedName].Remove(c.UID);
        }

        public Entity Duplicate(int uid)
        {
            Entity e = NewEntity();
            foreach (string t in Components.Keys)
            {
                Component val;
                Components[t].TryGetValue(uid, out val);
                if (val != null)
                {
                    e.AddComponent(val.DeepClone<Component>());
                    if (val is ParticleEmitter)
                    {
                        e.GetComponent<ParticleEmitter>().Initialize();
                        e.GetComponent<ParticleEmitter>().Launch();
                    }
                }

            }
            FolderNode parent = CurrentScene.HierarchyTree.SearchFile(GetEntity(uid)).Parent;
            FileNode node = CurrentScene.HierarchyTree.SearchFile(GetEntity(uid));

            if (node.Parent != null)
            {
                node.Parent.Values.Remove(node);
            }

            node.Parent = parent;

            if (parent != null)
            {
                parent.Values.Add(node);
            }
            return e;
        }

        public void RefreshTypes()
        {
            foreach (Type type in
            Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component)) && !Attribute.IsDefined(myType.BaseType, typeof(StoreChildren))))
            {
                if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                {
                    Components[type.AssemblyQualifiedName] = new Dictionary<int, Component>();
                }
            }

            foreach (Type type in
            Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component)) && !Attribute.IsDefined(myType.BaseType, typeof(StoreChildren))))
            {
                if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                {
                    Components[type.AssemblyQualifiedName] = new Dictionary<int, Component>();
                }
            }


            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in ass.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component)) && !Attribute.IsDefined(myType.BaseType, typeof(StoreChildren))))
                {
                    if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                    {
                        Components[type.AssemblyQualifiedName] = new Dictionary<int, Component>();
                    }
                }
            }

            foreach (Type type in
            Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))))
            {
                if (!CreatableTypes.Contains(type.AssemblyQualifiedName))
                {
                    CreatableTypes.Add(type.AssemblyQualifiedName);
                }
            }

            foreach (Type type in
            Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))))
            {
                if (!CreatableTypes.Contains(type.AssemblyQualifiedName))
                {
                    CreatableTypes.Add(type.AssemblyQualifiedName);
                }
            }


            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in ass.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))))
                {
                    if (!CreatableTypes.Contains(type.AssemblyQualifiedName))
                    {
                        CreatableTypes.Add(type.AssemblyQualifiedName);
                    }
                }
            }

            foreach (Type type in
            Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && myType.IsSubclassOf(typeof(Component)) && Attribute.IsDefined(myType, typeof(StoreChildren))))
            {
                Console.WriteLine(type);
                if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                {
                    Components[type.AssemblyQualifiedName] = new Dictionary<int, Component>();
                }
            }

            foreach (Type type in
            Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && myType.IsSubclassOf(typeof(Component)) && Attribute.IsDefined(myType, typeof(StoreChildren))))
            {
                Console.WriteLine(type);
                if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                {
                    Components[type.AssemblyQualifiedName] = new Dictionary<int, Component>();
                }
            }


            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in ass.GetTypes()
                .Where(myType => myType.IsClass && myType.IsSubclassOf(typeof(Component)) && Attribute.IsDefined(myType, typeof(StoreChildren))))
                {
                    Console.WriteLine(type);
                    if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                    {
                        Components[type.AssemblyQualifiedName] = new Dictionary<int, Component>();
                    }
                }
            }


        }



    }
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class StoreChildren : Attribute 
    { 
    }

}
