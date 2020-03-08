using Catalyst.Engine.Physics;
using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public class Sprite : Component, Loadable
    {
        public Entity Entity { get; private set; }
        [GuiString]
        public String Name { get; set; }
        public bool Packed { get; internal set; }

        private Rectangle _textureRect;
        public Rectangle TextureRect
        {
            get
            {
                if (Packed) return PackedTexture.Bounds;
                else return _textureRect;
            }
        }

        public PackedTexture PackedTexture { get; private set; }

        #region Sprite
        [GuiFloat(GuiFloatMode.Slider, 0, 1)]
        public float Layer { get; set; }

        [GuiFloat(GuiFloatMode.Slider, 0, 1)]
        public float Distance { get; set; }

        [NonSerialized]
        private Texture2D _texture;
        private string _texturePath;

        public virtual Texture2D Texture
        {
            get
            {
                if (Packed) return PackedTexture.Atlas.Texture;
                else return _texture;
            }
        }

        [GuiBoolean]
        public bool Active { get; set; }

        [GuiEnum]
        public SpriteOrigin Origin { get; set; }

        private Vector2 _originVec2;
        public Vector2 OriginVec2
        {
            get
            {
                if (_originVec2 == null)
                {
                    _originVec2 = OriginToVectorOffset(Origin, TextureRect);
                }
                return _originVec2;
            }
        }

        [GuiFloat(GuiFloatMode.Slider, -2.0f * (float)Math.PI, 2.0f * (float)Math.PI)]
        public float Rotation { get; set; }
        [GuiFloat(0.01f, 9999.0f)]
        public float Scale { get; set; }

        public SpriteEffects spriteEffects = new SpriteEffects();
        #endregion

        public Sprite(Entity entity) : base(entity) { }


        public Sprite(Entity entity, PackedTexture packedTextureInfo) : base(entity)
        {
            Entity = entity;
            PackedTexture = packedTextureInfo;
            Packed = true;
            Layer = 0;
            Distance = 0;
            Active = true;
            Origin = SpriteOrigin.TopLeft;
            Rotation = 0;
            Scale = 1;
        }

        public Sprite(Entity entity, Texture2D texture) : base(entity)
        {
            Packed = false;
            _texture = texture;
            _textureRect = new Rectangle(texture.Bounds);
            Layer = 0;
            Distance = 0;
            Active = true;
            Origin = SpriteOrigin.TopLeft;
            Rotation = 0;
            Scale = 1;
            _texturePath = texture.Name;
        }

        [Serializable]
        public enum SpriteOrigin
        {
            TopLeft,
            BottomLeft,
            TopRight,
            BottomRight,
            Center,
            TopCenter,
            BottomCenter,
            CenterLeft,
            CenterRight
        }

        public void LoadContent()
        {
            if (Packed)
            {
                PackedTexture.Atlas.LoadContent();
            }
            else
            {
                _texture = Graphics.Content.Load<Texture2D>(_texturePath);
            }
        }

        public static Utilities.Vector2 OriginToVectorOffset(SpriteOrigin origin, Utilities.Rectangle dimensions)
        {
            switch (origin)
            {
                case (SpriteOrigin.TopLeft):
                    return new Utilities.Vector2(0, 0);
                case (SpriteOrigin.TopRight):
                    return new Utilities.Vector2(dimensions.X, 0);
                case (SpriteOrigin.TopCenter):
                    return new Utilities.Vector2((int)(dimensions.X / 2), 0);
                case (SpriteOrigin.CenterLeft):
                    return new Utilities.Vector2(0, (int)(dimensions.Y / 2));
                case (SpriteOrigin.CenterRight):
                    return new Utilities.Vector2(dimensions.X, (int)(dimensions.Y / 2));
                case (SpriteOrigin.Center):
                    return new Utilities.Vector2((int)(dimensions.X / 2), (int)(dimensions.Y / 2));
                case (SpriteOrigin.BottomLeft):
                    return new Utilities.Vector2(0, dimensions.Y);
                case (SpriteOrigin.BottomRight):
                    return new Utilities.Vector2(dimensions.X, dimensions.Y);
                case (SpriteOrigin.BottomCenter):
                    return new Utilities.Vector2((int)(dimensions.X / 2), dimensions.Y);
                default:
                    return new Utilities.Vector2(0, 0);
            }

        }
    }
}
