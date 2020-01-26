using Catalyst.Engine.Rendering;
using Catalyst.Engine.Physics;
using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Catalyst.Engine
{
    [Serializable]
    public class Entity
    {
        
        public bool active = true;
        
        public int UID { get; private set; }
        
        public string Name { get; set; }

        private HashSet<string> _typeSet;
        
        private ECManager _manager;

        internal Entity(ECManager manager)
        {
            this.UID = manager.NewId();
            this._manager = manager;
            this.Name = "entity_"+ UID;
            this._typeSet = new HashSet<string>();
        }

        internal Entity(ECManager manager, int id)
        {
            this.UID = id;
            this._manager = manager;
            this._typeSet = new HashSet<string>();
        }

        private Entity() { }

        #region adders
        public AComponent AddComponent<T>() where T : AComponent
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            return _manager.Components[t.AssemblyQualifiedName][UID];
        }

        public AComponent AddComponent(Type t)
        {
            _manager.Components[t.AssemblyQualifiedName][UID] = (AComponent)Activator.CreateInstance(t, new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            return _manager.Components[t.AssemblyQualifiedName][UID];
        }

        public void AddComponents<T, U>() where T : AComponent where U : AComponent
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponent<U>();
        }

        public void AddComponents<T, U, V>() where T : AComponent where U : AComponent where V : AComponent
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponents<U, V>();
        }

        public void AddComponents<T, U, V, W>() where T : AComponent where U : AComponent where V : AComponent where W : AComponent
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W>();
        }

        public void AddComponents<T, U, V, W, X>() where T : AComponent where U : AComponent where V : AComponent where W : AComponent where X : AComponent
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W, X>();
        }

        public void AddComponents<T, U, V, W, X, Y>() where T : AComponent where U : AComponent where V : AComponent where W : AComponent where X : AComponent where Y : AComponent
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W, X, Y>();
        }

        public void AddComponents<T, U, V, W, X, Y, Z>() where T : AComponent where U : AComponent where V : AComponent where W : AComponent where X : AComponent where Y : AComponent where Z : AComponent
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W, X, Y, Z>();
        }


        public void AddChild<P, C>() where P : AComponent where C : P
        {
            Type t = typeof(P);
            _manager.Components[t.AssemblyQualifiedName][UID] = (C)Activator.CreateInstance(typeof(C), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
        }

        public void AddComponent<T>(AComponent c) where T : AComponent
        {

            if (c.UID != UID || !(c is T)) { return; }
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = c;
            _typeSet.Add(t.AssemblyQualifiedName);
        }

        internal void AddComponent(AComponent c)
        {
            if (c.UID != UID)
            {
                c.UID = UID;
            }
            _manager.Components[c.GetType().AssemblyQualifiedName][UID] = c;
            _typeSet.Add(c.GetType().AssemblyQualifiedName);

        }

        #endregion

        public T GetComponent<T>() where T : AComponent
        {
            return _manager.GetComponent<T>(UID);
        }

        public void RemoveComponent<T>() where T : AComponent
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName].Remove(UID);
            _typeSet.Remove(t.AssemblyQualifiedName);
        }

        public void DestroyEntity()
        {
            foreach (string t in _manager.Components.Keys)
            {
                _manager.Components[t].Remove(UID);
                _typeSet.Remove(Type.GetType(t).AssemblyQualifiedName);
            }
            _manager.EntityDict.Remove(UID);
        }

        public bool HasComponent<T>() where T : AComponent
        {
            Type t = typeof(T);
            return _typeSet.Contains(t.AssemblyQualifiedName);
        }
    }
}