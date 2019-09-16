using Chroma.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine
{
    public class ECManager
    {
        private Scene scene;
        private int id;
        internal Dictionary<int, Entity> entityDict = new Dictionary<int, Entity>();
        internal Dictionary<Type, Dictionary<int, AComponent>> components = new Dictionary<Type, Dictionary<int, AComponent>>();

        public ECManager(Scene scene)
        {
            this.scene = scene;
            id = 0;
        }

        public Entity NewEntity()
        {
            Entity e = new Entity(this);
            entityDict.Add(e.UID, e);
            return e;
        }

        public int NewId()
        {
            id++;
            return id - 1;
        }

        public Dictionary<int, Entity> GetEntities()
        {
            return entityDict;
        }
        public T GetComponent<T>(int UID) where T : AComponent
        {
            Type t = typeof(T);
            return (T)components[t][UID];
        }

        public Dictionary<int, AComponent> GetComponents<T>() where T : AComponent
        {
            Type t = typeof(T);
            return components[t];
        }

    }
}
