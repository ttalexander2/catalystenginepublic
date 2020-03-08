using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Catalyst.Engine.Utilities.Vector2;
using Color = Catalyst.Engine.Utilities.Color;
using Rectangle = Catalyst.Engine.Utilities.Rectangle;

namespace Catalyst.Engine.Physics
{
    /// <summary>
    /// Transform component representing physical location of an entity.
    /// <extends></extends>
    /// </summary>
    [Serializable]
    public class Position : Component
    {
        public static new string Name => "Transform";

        [GuiVector2]
        public Utilities.Vector2 Coordinates { get; set; }

       
        public Position(Entity entity) : base(entity)
        {
            Coordinates = new Utilities.Vector2();
        }


    }
}
