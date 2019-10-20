using Chroma.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine
{
    [Serializable]
    public class ECManager
    {
        private Scene scene;
        private int id;
        internal Dictionary<int, Entity> entityDict = new Dictionary<int, Entity>();
        internal Dictionary<Type, Dictionary<int, AComponent>> components = new Dictionary<Type, Dictionary<int, AComponent>>();

        internal ECManager(Scene scene)
        {
            this.scene = scene;
            id = 0;
            foreach (Type type in
            Assembly.GetAssembly(typeof(AComponent)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AComponent))))
            {
                components[type] = new Dictionary<int, AComponent>();
            }
            
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
            components[t].TryGetValue(UID, out val);
            return val != null ? (T)val : null;
        }

        public Dictionary<int, AComponent> GetComponents<T>() where T : AComponent
        {
            Type t = typeof(T);
            return components[t];
        }

    }
}
