using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Physics
{
    [Serializable]
    public class CTransform : AComponent
    {
        public static new string Name => "Transform";

        public Vector2 Position { get; set; }
        public Vector2 Dimensions { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public Vector2 Origin { get; set; }

        public Vector2 CollisionOffset { get; set; }
        public Vector2 CollisionDims { get; set; }
        public Action CollisionAction { get; set; }

        public CTransform(Entity entity) : base(entity)
        {
            Position = new Vector2();
            Dimensions = new Vector2();
            CollisionOffset = new Vector2();
            CollisionDims = new Vector2();
            Rotation = 0.0f;
            Scale = 1.0f;
            Origin = new Vector2(0, 0);
            CollisionAction = null;
        }


    }
}
