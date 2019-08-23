using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine
{
    public static class EntityManager
    {
        private static int id = -1;

        public static Entity Create()
        {
            return new Entity();
        }

        public static int NewId()
        {
            id++;
            return id;
        }

    }
}
