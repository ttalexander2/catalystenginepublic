using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Physics
{
    [Serializable]
    public class CTransform : AComponent
    {
        public static new string Name => "Transform";

        
        public Utilities.Vector2 Position { get; set; }
        
        public Utilities.Vector2 Dimensions { get; set; }
        
        public float Rotation { get; set; }
        
        public float Scale { get; set; }
        
        public Utilities.Vector2 Origin { get; set; }
        
        public Utilities.Vector2 CollisionOffset { get; set; }
        
        public Utilities.Vector2 CollisionDims { get; set; }
        
        public Action CollisionAction { get; set; }

        public CTransform(Entity entity) : base(entity)
        {
            Position = new Utilities.Vector2();
            Dimensions = new Utilities.Vector2();
            CollisionOffset = new Utilities.Vector2();
            CollisionDims = new Utilities.Vector2();
            Rotation = 0.0f;
            Scale = 1.0f;
            Origin = new Utilities.Vector2(0, 0);
            CollisionAction = null;
        }


    }
}
