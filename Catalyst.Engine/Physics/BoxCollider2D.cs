using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Catalyst.Engine
{
    [Serializable]
    public class BoxCollider2D : Collider2D
    {
        [GuiVector2]
        public Vector2 Offset;
        [GuiVector2]
        public Vector2 Dimensions
        {
            get { return _dimensions; }
            set
            {
                Bounds.Width = (int)value.X;
                Bounds.Height = (int)value.Y;
                _dimensions = value;
            }
        }

        private Vector2 _dimensions;
        public int X
        {
            get
            {
                return (int)Dimensions.X;
            }
            set
            {
                _dimensions.X = value;
            }
        }
        public int Y
        {
            get
            {
                return (int)Dimensions.Y;
            }
            set
            {
                _dimensions.Y = value;
            }
        }
        [GuiEnum]
        public RectangleOrigin Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                _origin = value;
                switch (value)
                {
                    case (RectangleOrigin.TopLeft):
                        _originVector = new Vector2(0, 0);
                        break;
                    case (RectangleOrigin.TopRight):
                        _originVector = new Vector2(Dimensions.X, 0);
                        break;
                    case (RectangleOrigin.TopCenter):
                        _originVector = new Vector2((int)(Dimensions.X / 2), 0);
                        break;
                    case (RectangleOrigin.CenterLeft):
                        _originVector = new Vector2(0, (int)(Dimensions.Y / 2));
                        break;
                    case (RectangleOrigin.CenterRight):
                        _originVector = new Vector2(Dimensions.X, (int)(Dimensions.Y / 2));
                        break;
                    case (RectangleOrigin.Center):
                        _originVector = new Vector2((int)(Dimensions.X / 2), (int)(Dimensions.Y / 2));
                        break;
                    case (RectangleOrigin.BottomLeft):
                        _originVector = new Vector2(0, Dimensions.Y);
                        break;
                    case (RectangleOrigin.BottomRight):
                        _originVector = new Vector2(Dimensions.X, Dimensions.Y);
                        break;
                    case (RectangleOrigin.BottomCenter):
                        _originVector = new Vector2((int)(Dimensions.X / 2), Dimensions.Y);
                        break;
                    default:
                        value = RectangleOrigin.Custom;
                        break;
                }
            }
        }
        private RectangleOrigin _origin;
        [GuiVector2]
        public Vector2 OriginVector
        {
            get
            {
                return _originVector;
            }
            set
            {
                if (value != _originVector)
                {
                    _originVector = value;
                    _origin = RectangleOrigin.Custom;
                }

            }
        }
        private Vector2 _originVector = Vector2.Zero;

        [NonSerialized]
        private Texture2D _dummy;

        private Texture2D _dummyTexture
        {
            get
            {
                if (_dummy == null)
                {
                    _dummy = new Texture2D(Graphics.DeviceManager.GraphicsDevice, 1, 1);
                    _dummy.SetData(new Color[] { Color.White });
                }
                return _dummy;
            }
        }

        public BoxCollider2D(Entity entity) : base(entity)
        {
            //_dummy = new Texture2D(Graphics.GraphicsDevice.GraphicsDevice, 1, 1);
            //_dummy.SetData(new Color[] { Color.White });
        }


        public override void DebugRender()
        {
            if (Colliding)
            {
                Graphics.SpriteBatch.Draw(_dummyTexture, new Vector2(Entity.Position.X, Entity.Position.Y) + OriginVector + Offset, null, Color.Green, 0, Vector2.Zero, new Vector2(Dimensions.X, 1), new SpriteEffects(), 0);
                Graphics.SpriteBatch.Draw(_dummyTexture, new Vector2(Entity.Position.X, Entity.Position.Y) + OriginVector + Offset, null, Color.Green, 0, Vector2.Zero, new Vector2(1, Dimensions.Y), new SpriteEffects(), 0);
                Graphics.SpriteBatch.Draw(_dummyTexture, new Vector2(Entity.Position.X, Entity.Position.Y + Dimensions.Y) + OriginVector + Offset, null, Color.Green, 0, Vector2.Zero, new Vector2(Dimensions.X + 1, 1), new SpriteEffects(), 0);
                Graphics.SpriteBatch.Draw(_dummyTexture, new Vector2(Entity.Position.X + Dimensions.X, Entity.Position.Y) + OriginVector + Offset, null, Color.Green, 0, Vector2.Zero, new Vector2(1, Dimensions.Y + 1), new SpriteEffects(), 0);
            }
            else
            {
                Graphics.SpriteBatch.Draw(_dummyTexture, new Vector2(Entity.Position.X, Entity.Position.Y) + OriginVector + Offset, null, Color.Red, 0, Vector2.Zero, new Vector2(Dimensions.X, 1), new SpriteEffects(), 0);
                Graphics.SpriteBatch.Draw(_dummyTexture, new Vector2(Entity.Position.X, Entity.Position.Y) + OriginVector + Offset, null, Color.Red, 0, Vector2.Zero, new Vector2(1, Dimensions.Y), new SpriteEffects(), 0);
                Graphics.SpriteBatch.Draw(_dummyTexture, new Vector2(Entity.Position.X, Entity.Position.Y + Dimensions.Y) + OriginVector + Offset, null, Color.Red, 0, Vector2.Zero, new Vector2(Dimensions.X + 1, 1), new SpriteEffects(), 0);
                Graphics.SpriteBatch.Draw(_dummyTexture, new Vector2(Entity.Position.X + Dimensions.X, Entity.Position.Y) + OriginVector + Offset, null, Color.Red, 0, Vector2.Zero, new Vector2(1, Dimensions.Y + 1), new SpriteEffects(), 0);
            }
           
        }
    }
}
