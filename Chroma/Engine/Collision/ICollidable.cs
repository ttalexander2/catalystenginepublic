using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Collision
{
    public interface ICollidable
    {
        void HandleCollision(object collider);
    }
}
