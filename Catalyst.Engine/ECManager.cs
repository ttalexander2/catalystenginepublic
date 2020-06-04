using Catalyst.Engine.Rendering;
using Catalyst.Engine.Utilities;
using Catalyst.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static Catalyst.Engine.FileTree<Catalyst.Engine.GameObject>;

namespace Catalyst.Engine
{
    [Serializable]
    public class ECManager
    {
        
        private int _id;

        public Scene CurrentScene { get; private set; }
        
        internal Dictionary<int, Entity> EntityDict = new Dictionary<int, Entity>();
        
        internal Dictionary<string, Dictionary<int, Component>> Components = new Dictionary<string, Dictionary<int, Component>>();


        internal ECManager(Scene scene)
        {
            CurrentScene = scene;
            _id = 0;
            RefreshTypes();
            
        }

        public Entity NewEntity()
        {
            Entity e = new Entity(CurrentScene);
            EntityDict.Add(e.UID, e);
            CurrentScene.HierarchyTree.AddElement(e, e.Name);
            return e;
        }

        internal int NewId()
        {
            _id++;
            return _id - 1;
        }

        public Dictionary<int, Entity> GetEntities()
        {
            return EntityDict;
        }

        public Entity GetEntity(int UID)
        {
            Entity val;
            EntityDict.TryGetValue(UID, out val);
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
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))))
            {
                if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                {
                    Components[type.AssemblyQualifiedName] = new Dictionary<int, Component>();
                }
            }

            foreach (Type type in
            Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))))
            {
                if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                {
                    Components[type.AssemblyQualifiedName] = new Dictionary<int, Component>();
                }
            }

            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in ass.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))))
                {
                    if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                    {
                        Components[type.AssemblyQualifiedName] = new Dictionary<int, Component>();
                    }
                }
            }

        }



    }

    public class StoreInParent : Attribute { }
}
