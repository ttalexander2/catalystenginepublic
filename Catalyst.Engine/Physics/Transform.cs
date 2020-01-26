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
    [Serializable]
    public class Transform : AComponent
    {
        public static new string Name => "Transform";

        [ImmediateVector2]
        public Utilities.Vector2 Position { get; set; }
        [ImmediateVector2]
        public Utilities.Vector2 Dimensions { get; set; }
        [ImmediateFloat(ImmediateFloatMode.Slider, -2.0f*(float)Math.PI, 2.0f * (float)Math.PI)]
        public float Rotation { get; set; }
        [ImmediateFloat(0.01f, 9999.0f)]
        public float Scale { get; set; }
        
        public Utilities.Vector2 Origin { get; set; }
        [ImmediateVector2]
        public Utilities.Vector2 CollisionOffset { get; set; }
        [ImmediateLabel]
        public Utilities.Vector2 CollisionDims { get; set; }
        
        public Action CollisionAction { get; set; }

        public Transform(Entity entity) : base(entity)
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
