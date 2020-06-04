using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    [Serializable]
    public abstract class GameObject
    {
        public string Name { get; set; } = "";

        public bool Active { get; set; } = true;
        public bool Visible { get; set; } = true;

        public virtual void Rename(string name)
        {
            this.Name = name;
        }
    }


}
