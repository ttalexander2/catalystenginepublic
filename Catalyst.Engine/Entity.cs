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
    public class Entity : GameObject
    {
        public int UID { get; private set; }

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

        public override void Rename(string name)
        {
            this.Name = name;
            this._manager.CurrentScene.HierarchyTree.RenameFile(this, name);
        }

        #region adders
        public Component AddComponent<T>() where T : Component
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            return _manager.Components[t.AssemblyQualifiedName][UID];
        }

        public Component AddComponent(Type t)
        {
            _manager.Components[t.AssemblyQualifiedName][UID] = (Component)Activator.CreateInstance(t, new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            return _manager.Components[t.AssemblyQualifiedName][UID];
        }

        public void AddComponents<T, U>() where T : Component where U : Component
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponent<U>();
        }

        public void AddComponents<T, U, V>() where T : Component where U : Component where V : Component
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponents<U, V>();
        }

        public void AddComponents<T, U, V, W>() where T : Component where U : Component where V : Component where W : Component
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W>();
        }

        public void AddComponents<T, U, V, W, X>() where T : Component where U : Component where V : Component where W : Component where X : Component
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W, X>();
        }

        public void AddComponents<T, U, V, W, X, Y>() where T : Component where U : Component where V : Component where W : Component where X : Component where Y : Component
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W, X, Y>();
        }

        public void AddComponents<T, U, V, W, X, Y, Z>() where T : Component where U : Component where V : Component where W : Component where X : Component where Y : Component where Z : Component
        {
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W, X, Y, Z>();
        }


        public void AddChild<P, C>() where P : Component where C : P
        {
            Type t = typeof(P);
            _manager.Components[t.AssemblyQualifiedName][UID] = (C)Activator.CreateInstance(typeof(C), new Object[] { this });
            _typeSet.Add(t.AssemblyQualifiedName);
        }

        public void AddComponent<T>(Component c) where T : Component
        {

            if (c.UID != UID || !(c is T)) { return; }
            Type t = typeof(T);
            _manager.Components[t.AssemblyQualifiedName][UID] = c;
            _typeSet.Add(t.AssemblyQualifiedName);
        }

        internal void AddComponent(Component c)
        {
            if (c.UID != UID)
            {
                c.UID = UID;
            }
            _manager.Components[c.GetType().AssemblyQualifiedName][UID] = c;
            _typeSet.Add(c.GetType().AssemblyQualifiedName);

        }

        #endregion

        public T GetComponent<T>() where T : Component
        {
            return _manager.GetComponent<T>(UID);
        }

        public void RemoveComponent<T>() where T : Component
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

        public bool HasComponent<T>() where T : Component
        {
            Type t = typeof(T);
            return _typeSet.Contains(t.AssemblyQualifiedName);
        }
    }
}