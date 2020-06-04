using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Catalyst.Engine.Physics;
using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Catalyst.Engine.Utilities.Vector2;
using Color = Catalyst.Engine.Utilities.Color;
using Rectangle = Catalyst.Engine.Utilities.Rectangle;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public class AnimatedSprite : Sprite
    {
        private List<PackedTexture> _packedTextures = new List<PackedTexture>();
        [NonSerialized]
        private List<Texture2D> _textures = new List<Texture2D>();
        public override Texture2D Texture
        {
            get
            {
                if (Packed) return _packedTextures[CurrentTexture].Atlas.Texture;
                else return _textures[CurrentTexture];
            }
        }
        [GuiInteger(0, int.MaxValue)]
        public int CurrentTexture { get; set; }
        [GuiFloat(0,float.MaxValue)]
        public float AnimationSpeed { get; set; }

        [GuiLabel]
        public int Frame { get; set; }
        [GuiBoolean]
        public bool Loop { get; set; }
        [GuiBoolean]
        public bool Animating { get; set; }
        [GuiLabel("Number of textures: ")]
        public int Count
        {
            get
            {
                if (Packed) return _packedTextures.Count;
                else return _textures.Count;
            }
        }


        internal TimeSpan TimeChanged = new TimeSpan();

        public AnimatedSprite(Entity entity, Texture2D[] textures) : base(entity)
        {
            Packed = false;
            _textures.AddRange(textures);
            Layer = 0;
            Distance = 0;
            Active = true;
            Origin = RectangleOrigin.TopLeft;
        }

        public AnimatedSprite(Entity entity, PackedTexture[] textures) : base(entity)
        {
            Packed = true;
            _packedTextures.AddRange(textures);
            Layer = 0;
            Distance = 0;
            Active = true;
            Origin = RectangleOrigin.TopLeft;
        }



    }




}

