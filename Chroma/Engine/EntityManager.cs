using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine
{
    public class EntityManager
    {
        private int id = -1;

        public Entity Create()
        {
            return new Entity();
        }

        public int NewId()
        {
            id++;
            return id;
        }

    }
}
