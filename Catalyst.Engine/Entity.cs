using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Net;

namespace Catalyst.Engine
{
    [Serializable]
    public class Entity : GameObject
    {
        public Scene Scene { get; private set; }
        public int UID { get; private set; }

        public HashSet<string> ComponentTypes;

        public List<int> Children = new List<int>();

        public Collider2D Collider;

        [GuiVector2]
        public Vector2 Position = new Vector2();
        public float X 
        { 
            get
            {
                return Position.X;
            }
            set
            {
                Position.X = value;
            }
        }
        public float Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position.Y = value;
            }
        }

        internal Entity(Scene scene)
        {
            this.Scene = scene;
            this.UID = Scene.Manager.NewId();
            this.Name = "entity_"+ UID;
            this.ComponentTypes = new HashSet<string>();
            Position = Vector2.Zero;
            this.Scene.Manager.Entities.Add(UID, this);
            this.Scene.HierarchyTree.AddElement(this, this.Name);
            this.Initialize();
        }

        internal Entity() { }

        public Entity CreateChild()
        {
            Entity e = Scene.Manager.NewEntity();
            Children.Add(e.UID);
            return e;
        }

        public virtual void Initialize() { }

        public override void Rename(string name)
        {
            this.Name = name;
            this.Scene.Manager.CurrentScene.HierarchyTree.RenameFile(this, name);
        }

        #region adders
        public T AddComponent<T>() where T : Component
        {
            Type t = typeof(T);
            Scene.Manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            ComponentTypes.Add(t.AssemblyQualifiedName);
            return (T)Scene.Manager.Components[t.AssemblyQualifiedName][UID];
        }

        public Component AddComponent(Type t)
        {
            Component c = (Component)Activator.CreateInstance(t, new Object[] { this });
            ComponentTypes.Add(t.AssemblyQualifiedName);
            return c;
        }

        public void AddComponents<T, U>() where T : Component where U : Component
        {
            Type t = typeof(T);
            Scene.Manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            ComponentTypes.Add(t.AssemblyQualifiedName);
            AddComponent<U>();
        }

        public void AddComponents<T, U, V>() where T : Component where U : Component where V : Component
        {
            Type t = typeof(T);
            Scene.Manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            ComponentTypes.Add(t.AssemblyQualifiedName);
            AddComponents<U, V>();
        }

        public void AddComponents<T, U, V, W>() where T : Component where U : Component where V : Component where W : Component
        {
            Type t = typeof(T);
            Scene.Manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            ComponentTypes.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W>();
        }

        public void AddComponents<T, U, V, W, X>() where T : Component where U : Component where V : Component where W : Component where X : Component
        {
            Type t = typeof(T);
            Scene.Manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            ComponentTypes.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W, X>();
        }

        public void AddComponents<T, U, V, W, X, Y>() where T : Component where U : Component where V : Component where W : Component where X : Component where Y : Component
        {
            Type t = typeof(T);
            Scene.Manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            ComponentTypes.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W, X, Y>();
        }

        public void AddComponents<T, U, V, W, X, Y, Z>() where T : Component where U : Component where V : Component where W : Component where X : Component where Y : Component where Z : Component
        {
            Type t = typeof(T);
            Scene.Manager.Components[t.AssemblyQualifiedName][UID] = (T)Activator.CreateInstance(typeof(T), new Object[] { this });
            ComponentTypes.Add(t.AssemblyQualifiedName);
            AddComponents<U, V, W, X, Y, Z>();
        }


        public void AddChild<P, C>() where P : Component where C : P
        {
            Type t = typeof(P);
            Scene.Manager.Components[t.AssemblyQualifiedName][UID] = (C)Activator.CreateInstance(typeof(C), new Object[] { this });
            ComponentTypes.Add(t.AssemblyQualifiedName);
        }

        public void AddComponent<T>(Component c) where T : Component
        {

            if (c.UID != UID || !(c is T)) { return; }
            Type t = typeof(T);
            Scene.Manager.Components[t.AssemblyQualifiedName][UID] = c;
            ComponentTypes.Add(t.AssemblyQualifiedName);
        }

        internal void AddComponent(Component c)
        {
            if (c.UID != UID)
            {
                c.UID = UID;
            }
            Scene.Manager.Components[c.GetType().AssemblyQualifiedName][UID] = c;
            ComponentTypes.Add(c.GetType().AssemblyQualifiedName);
        }

        #endregion

        public T GetComponent<T>() where T : Component
        {
            return Scene.Manager.GetComponent<T>(UID);
        }

        public void RemoveComponent<T>() where T : Component
        {
            Type t = typeof(T);
            Scene.Manager.Components[t.AssemblyQualifiedName].Remove(UID);
            ComponentTypes.Remove(t.AssemblyQualifiedName);
        }

        public void RemoveComponent(Component component)
        {
            Scene.Manager.Components[component.GetType().AssemblyQualifiedName].Remove(UID);
            ComponentTypes.Remove(component.GetType().AssemblyQualifiedName);
        }

        public void DestroyEntity()
        {
            foreach (string t in Scene.Manager.Components.Keys)
            {
                Scene.Manager.Components[t].Remove(UID);
                ComponentTypes.Remove(Type.GetType(t).AssemblyQualifiedName);
            }
            Scene.Manager.Entities.Remove(UID);
        }

        public bool HasComponent<T>() where T : Component
        {
            Type t = typeof(T);
            return ComponentTypes.Contains(t.AssemblyQualifiedName);
        }

        public bool HasComponent(Component component)
        {
            return ComponentTypes.Contains(component.GetType().AssemblyQualifiedName);
        }
    }
}