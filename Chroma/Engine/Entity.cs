using Chroma.Engine.Graphics;
using Chroma.Engine.Physics;
using System;
using System.Collections.Generic;

namespace Chroma.Engine
{
    [Serializable]
    public class Entity
    {
        public bool active = true;

        public int UID { get; private set; }

        public string Name { get; private set; }

        private HashSet<Type> typeSet;

        private ECManager manager;

        internal Entity(ECManager manager)
        {
            this.UID = manager.NewId();
            this.manager = manager;
            this.Name = "entity_"+ UID;
            this.typeSet = new HashSet<Type>();
        }

        #region adders
        public void AddComponent<T>() where T : AComponent
        {
            Type t = typeof(T);
            manager.components[t][this] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            typeSet.Add(t);
        }

        public void AddComponents<T, U>() where T : AComponent where U : AComponent
        {
            Type t = typeof(T);
            manager.components[t][this] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            typeSet.Add(t);
            AddComponent<U>();
        }

        public void AddComponents<T, U, V>() where T : AComponent where U : AComponent where V : AComponent
        {
            Type t = typeof(T);
            manager.components[t][this] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            typeSet.Add(t);
            AddComponents<U, V>();
        }

        public void AddComponents<T, U, V, W>() where T : AComponent where U : AComponent where V : AComponent where W : AComponent
        {
            Type t = typeof(T);
            manager.components[t][this] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            typeSet.Add(t);
            AddComponents<U, V, W>();
        }

        public void AddComponents<T, U, V, W, X>() where T : AComponent where U : AComponent where V : AComponent where W : AComponent where X : AComponent
        {
            Type t = typeof(T);
            manager.components[t][this] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            typeSet.Add(t);
            AddComponents<U, V, W, X>();
        }

        public void AddComponents<T, U, V, W, X, Y>() where T : AComponent where U : AComponent where V : AComponent where W : AComponent where X : AComponent where Y : AComponent
        {
            Type t = typeof(T);
            manager.components[t][this] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            typeSet.Add(t);
            AddComponents<U, V, W, X, Y>();
        }

        public void AddComponents<T, U, V, W, X, Y, Z>() where T : AComponent where U : AComponent where V : AComponent where W : AComponent where X : AComponent where Y : AComponent where Z : AComponent
        {
            Type t = typeof(T);
            manager.components[t][this] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            typeSet.Add(t);
            AddComponents<U, V, W, X, Y, Z>();
        }


        public void AddChild<P, C>() where P : AComponent where C : P
        {
            Type t = typeof(P);
            manager.components[t][this] = (C)Activator.CreateInstance(typeof(C), new Object[] { this });
            typeSet.Add(t);
        }

        public void AddComponent<T>(AComponent c) where T : AComponent
        {

            if (c.UID != UID || !(c is T)) { return; }
            Type t = typeof(T);
            manager.components[t][this] = c;
            typeSet.Add(t);
        }

        #endregion

        public T GetComponent<T>() where T : AComponent
        {
            return manager.GetComponent<T>(UID);
        }

        public void RemoveComponent<T>() where T : AComponent
        {
            Type t = typeof(T);
            manager.components[t].Remove(this);
            typeSet.Remove(t);
        }

        public void DestroyEntity()
        {
            foreach (Type t in manager.components.Keys)
            {
                manager.components[t].Remove(this);
                typeSet.Remove(t);
            }
            manager.entityDict.Remove(UID);
        }

        public bool HasComponent<T>() where T : AComponent
        {
            Type t = typeof(T);
            return typeSet.Contains(t);
        }


    }
}