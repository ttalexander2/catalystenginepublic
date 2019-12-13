using System;
using System.Collections.Generic;
using Chroma.Engine.Physics;
using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Chroma.Engine.Utilities.Utility;

namespace Chroma.Engine.Graphics
{
    [Serializable]
    public class CSprite : AComponent
    {


        public string name;
        public float layer;

        private int _textureHeight;
        private int _textureWidth;
        public List<Texture2D> Textures = new List<Texture2D>();
        public Texture2D Texture
        {
            get
            {
                return Textures[CurrentTexture];
            }
        }
        public int CurrentTexture { get; set; }
        public float animationSpeed = 2.0f; //Frames per second

        public bool visible = true;

        public int frame { get; set; }
        public bool loop = true;
        public bool animating = true;
        public SpriteEffects spriteEffects = new SpriteEffects();
        public Color debugColor = Color.Red * 0.4f; //Float = transparency

        internal TimeSpan TimeChanged = new TimeSpan();

        public CSprite(Entity entity) : base(entity)
        {
            CTransform transform = new CTransform(entity);
            Vector2 dims = GetDims();
            transform.Origin = Utility.OriginToVectorOffset(Origin.TopLeft, dims);
            transform.CollisionOffset = new Vector2();
            transform.CollisionDims = transform.Dimensions;
            animating = Textures.Count > 1;
            entity.AddComponent<CTransform>(transform);
        }

        public CSprite(Entity entity, string name, int x, int y, Texture2D[] textures, Origin origin) : base(entity)
        {
            CTransform transform = new CTransform(entity);
            this.name = name;
            this.Textures.AddRange(textures);
            Vector2 dims = GetDims();
            transform.Dimensions = dims;
            transform.Position = new Vector2(x, y);
            transform.Origin = Utility.OriginToVectorOffset(origin, dims);
            transform.CollisionOffset = new Vector2();
            transform.CollisionDims = transform.Dimensions;
            animating = Textures.Count > 1;
            entity.AddComponent<CTransform>(transform);
        }

        public CSprite(Entity entity, string name, int x, int y, float scale, float rotation, Texture2D[] textures, Origin origin) : base(entity)
        {
            CTransform transform = new CTransform(entity);
            this.name = name;
            this.Textures.AddRange(textures);
            Vector2 dims = GetDims();
            transform.Dimensions = dims;
            transform.Position = new Vector2(x, y);
            transform.Origin = Utility.OriginToVectorOffset(origin, dims);
            transform.Scale = scale;
            transform.Rotation = rotation;
            transform.CollisionOffset = new Vector2();
            transform.CollisionDims = transform.Dimensions;
            animating = Textures.Count > 1;
            entity.AddComponent<CTransform>(transform);
        }

        public CSprite(Entity entity, string name, int x, int y, Texture2D[] textures, int xOrigin, int yOrigin) : base(entity)
        {
            CTransform transform = new CTransform(entity);
            this.name = name;
            this.Textures.AddRange(textures);
            Vector2 dims = GetDims();
            transform.Dimensions = dims;
            transform.Position = new Vector2(x, y);
            transform.Origin = new Vector2(xOrigin, yOrigin);
            transform.CollisionOffset = new Vector2();
            transform.CollisionDims = transform.Dimensions;
            animating = Textures.Count > 1;
            entity.AddComponent<CTransform>(transform);
        }

        public CSprite(Entity entity, string name, int x, int y, float scale, float rotation, Texture2D[] textures, int xOrigin, int yOrigin) : base(entity)
        {
            CTransform transform = new CTransform(entity);
            this.name = name;
            this.Textures.AddRange(textures);
            Vector2 dims = GetDims();
            transform.Dimensions = dims;
            transform.Position = new Vector2(x, y);
            transform.Origin = new Vector2(xOrigin, yOrigin);
            transform.Scale = scale;
            transform.Rotation = rotation;
            transform.CollisionOffset = new Vector2();
            transform.CollisionDims = transform.Dimensions;
            animating = Textures.Count > 1;
            entity.AddComponent<CTransform>(transform);
        }


        private Vector2 GetDims()
        {
            for (int i = 0; i < Textures.Count; i++)
            {
                if (Textures[i].Width > _textureWidth) _textureWidth = Textures[i].Width;
                if (Textures[i].Height > _textureHeight) _textureHeight = Textures[i].Height;
            }
            return new Vector2(_textureWidth, _textureHeight);
        }



    }




}

