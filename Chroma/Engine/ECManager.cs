using Chroma.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine
{
[Serializable]
    public class ECManager
    {
        
        private int id;
        
        internal Dictionary<int, Entity> entityDict = new Dictionary<int, Entity>();
        
        internal Dictionary<string, Dictionary<int, AComponent>> components = new Dictionary<string, Dictionary<int, AComponent>>();

        internal ECManager()
        {
            id = 0;
            RefreshTypes();
            
        }

        public Entity NewEntity()
        {
            Entity e = new Entity(this);
            entityDict.Add(e.UID, e);
            return e;
        }

        internal int NewId()
        {
            id++;
            return id - 1;
        }

        public Dictionary<int, Entity> GetEntities()
        {
            return entityDict;
        }

        public Entity GetEntity(int UID)
        {
            Entity val;
            entityDict.TryGetValue(UID, out val);
            return val;
        }
        public T GetComponent<T>(int UID) where T : AComponent
        {
            Type t = typeof(T);

            AComponent val;
            components[t.AssemblyQualifiedName].TryGetValue(UID, out val);
            return val != null ? (T)val : null;
        }

        public T GetComponent<T>(Entity e) where T : AComponent
        {
            Type t = typeof(T);

            AComponent val;
            components[t.AssemblyQualifiedName].TryGetValue(e.UID, out val);
            return val != null ? (T)val : null;
        }

        public Dictionary<int, AComponent> GetComponents<T>() where T : AComponent
        {
            Type t = typeof(T);
            return components[t.AssemblyQualifiedName];
        }

        public Dictionary<string, Dictionary<int, AComponent>> GetComponentDictionary()
        {
            return components;
        }

        public void RefreshTypes()
        {
            foreach (Type type in
            Assembly.GetAssembly(typeof(AComponent)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AComponent))))
            {
                if (!components.Keys.Contains(type.AssemblyQualifiedName))
                {
                    components[type.AssemblyQualifiedName] = new Dictionary<int, AComponent>();
                }

            }
        }


    }
}
