using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    [Serializable]
    public class ECManager
    {
        
        private int _id;
        
        internal Dictionary<int, Entity> EntityDict = new Dictionary<int, Entity>();
        
        internal Dictionary<string, Dictionary<int, AComponent>> Components = new Dictionary<string, Dictionary<int, AComponent>>();

        internal ECManager()
        {
            _id = 0;
            RefreshTypes();
            
        }

        public Entity NewEntity()
        {
            Entity e = new Entity(this);
            EntityDict.Add(e.UID, e);
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
        public T GetComponent<T>(int UID) where T : AComponent
        {
            Type t = typeof(T);

            AComponent val;
            Components[t.AssemblyQualifiedName].TryGetValue(UID, out val);
            return val != null ? (T)val : null;
        }

        public T GetComponent<T>(Entity e) where T : AComponent
        {
            Type t = typeof(T);

            AComponent val;
            Components[t.AssemblyQualifiedName].TryGetValue(e.UID, out val);
            return val != null ? (T)val : null;
        }

        public Dictionary<int, AComponent> GetComponents<T>() where T : AComponent
        {
            Type t = typeof(T);
            return Components[t.AssemblyQualifiedName];
        }

        public Dictionary<string, Dictionary<int, AComponent>> GetComponentDictionary()
        {
            return Components;
        }

        public void RemoveComponent(AComponent c)
        {
            Type t = c.GetType();
            Components[t.AssemblyQualifiedName].Remove(c.UID);
        }

        public Entity Duplicate(int uid)
        {
            Entity e = NewEntity();
            foreach (string t in Components.Keys)
            {
                AComponent val;
                Components[t].TryGetValue(uid, out val);
                if (val != null)
                {
                    e.AddComponent(val.DeepClone<AComponent>());
                }

            }
            return e;
        }

        public void RefreshTypes()
        {
            foreach (Type type in
            Assembly.GetAssembly(typeof(AComponent)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AComponent))))
            {
                if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                {
                    Components[type.AssemblyQualifiedName] = new Dictionary<int, AComponent>();
                }
            }

            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in ass.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AComponent))))
                {
                    if (!Components.Keys.Contains(type.AssemblyQualifiedName))
                    {
                        Components[type.AssemblyQualifiedName] = new Dictionary<int, AComponent>();
                    }
                }
            }
        }


    }
}
