using System;
using System.Collections.Generic;

namespace Chroma.Engine
{
    public class Entity
    {
        public bool active = true;

        public int UID { get; private set; }

        public int test { get; private set; }
        public string Name { get; private set; }

        private HashSet<Type> typeSet;

        private ECManager manager;

        internal Entity(ECManager manager)
        {
            this.UID = manager.NewId();
            this.manager = manager;
            this.Name = "entity_"+ UID;
            
        }

        public void AddComponent<T>() where T : AComponent
        {
            manager.entityDict[UID].AddType<T>();
            Type t = typeof(T);
            if (!manager.components.ContainsKey(t))
            {
                manager.components[t] = new Dictionary<int, AComponent>();
            }
            manager.components[t][UID] = (T)Activator.CreateInstance(typeof(T), UID);
        }

        public void RemoveComponent<T>() where T : AComponent
        {
            manager.entityDict[UID].RemoveType<T>();
            Type t = typeof(T);
            manager.components[t].Remove(UID);
        }

        public void DestroyEntity()
        {
            foreach (Type t in manager.components.Keys)
            {
                manager.components[t].Remove(UID);
            }
            manager.entityDict.Remove(UID);
        }

        public bool HasComponent<T>() where T : AComponent
        {
            Type t = typeof(T);
            return typeSet.Contains(t);
        }

        internal void AddType<T>() where T: AComponent
        {
            Type t = typeof(T);
            _ = typeSet.Add(t);
        }

        internal void RemoveType<T>() where T: AComponent
        {
            Type t =typeof(T);
            _ = typeSet.Remove(t);
        }


    }
}