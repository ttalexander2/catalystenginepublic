using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma
{
    public class Entity
    {
        public bool active = true;

        public bool visible = true;
        public bool collidable = false;

        public int UID { get; private set; }

        public Entity()
        {
            UID = World.EntityIdNum;
            World.EntityIdNum++;
        }
    }
}
