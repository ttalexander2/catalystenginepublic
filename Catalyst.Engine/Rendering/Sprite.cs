﻿using System;
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
    public class Sprite : AComponent
    {

        public new string Name { get; set; }

        [ImmediateFloat(ImmediateFloatMode.Slider, 0, 1)]
        public float Layer { get; set; }

        private int _textureHeight;
        
        private int _textureWidth;

        public List<MTexture> Textures = new List<MTexture>();
        public MTexture Texture
        {
            get
            {
                return Textures[CurrentTexture];
            }
        }
        public int CurrentTexture { get; set; }
        [ImmediateFloat(0,1000)]
        public float AnimationSpeed { get; set; }
        [ImmediateBoolean]
        public bool Visible { get; set; }
        [ImmediateLabel]
        public int Frame { get; set; }
        [ImmediateBoolean]
        public bool Loop { get; set; }
        [ImmediateBoolean]
        public bool Animating { get; set; }

        public SpriteEffects spriteEffects = new SpriteEffects();
        
        public Color debugColor = Color.Red * 0.4f; //Float = transparency

        internal TimeSpan TimeChanged = new TimeSpan();

        public Sprite(Entity entity) : base(entity)
        {
            Transform transform = new Transform(entity);
            Vector2 dims = GetDims();
            transform.Origin = Utility.OriginToVectorOffset(Origin.TopLeft, dims);
            transform.CollisionOffset = new Vector2();
            transform.CollisionDims = transform.Dimensions;
            Animating = Textures.Count > 1;
            entity.AddComponent<Transform>(transform);
            Visible = true;
            Loop = true;
            AnimationSpeed = 2.0f;
        }

        public Sprite(Entity entity, string name, int x, int y, MTexture[] textures, Origin origin) : base(entity)
        {
            Transform transform = new Transform(entity);
            this.Name = name;
            this.Textures.AddRange(textures);
            Vector2 dims = GetDims();
            transform.Dimensions = dims;
            transform.Position = new Vector2(x, y);
            transform.Origin = Utility.OriginToVectorOffset(origin, dims);
            transform.CollisionOffset = new Vector2();
            transform.CollisionDims = transform.Dimensions;
            Animating = Textures.Count > 1;
            entity.AddComponent<Transform>(transform);
            Visible = true;
            Loop = true;
            AnimationSpeed = 2.0f;
        }

        public Sprite(Entity entity, string name, int x, int y, float scale, float rotation, MTexture[] textures, Origin origin) : base(entity)
        {
            Transform transform = new Transform(entity);
            this.Name = name;
            this.Textures.AddRange(textures);
            Vector2 dims = GetDims();
            transform.Dimensions = dims;
            transform.Position = new Vector2(x, y);
            transform.Origin = Utility.OriginToVectorOffset(origin, dims);
            transform.Scale = scale;
            transform.Rotation = rotation;
            transform.CollisionOffset = new Vector2();
            transform.CollisionDims = transform.Dimensions;
            Animating = Textures.Count > 1;
            entity.AddComponent<Transform>(transform);
            Visible = true;
            Loop = true;
            AnimationSpeed = 2.0f;
        }

        public Sprite(Entity entity, string name, int x, int y, MTexture[] textures, int xOrigin, int yOrigin) : base(entity)
        {
            Transform transform = new Transform(entity);
            this.Name = name;
            this.Textures.AddRange(textures);
            Vector2 dims = GetDims();
            transform.Dimensions = dims;
            transform.Position = new Vector2(x, y);
            transform.Origin = new Vector2(xOrigin, yOrigin);
            transform.CollisionOffset = new Vector2();
            transform.CollisionDims = transform.Dimensions;
            Animating = Textures.Count > 1;
            entity.AddComponent<Transform>(transform);
            Visible = true;
            Loop = true;
            AnimationSpeed = 2.0f;
        }

        public Sprite(Entity entity, string name, int x, int y, float scale, float rotation, MTexture[] textures, int xOrigin, int yOrigin) : base(entity)
        {
            Transform transform = new Transform(entity);
            this.Name = name;
            this.Textures.AddRange(textures);
            Vector2 dims = GetDims();
            transform.Dimensions = dims;
            transform.Position = new Vector2(x, y);
            transform.Origin = new Vector2(xOrigin, yOrigin);
            transform.Scale = scale;
            transform.Rotation = rotation;
            transform.CollisionOffset = new Vector2();
            transform.CollisionDims = transform.Dimensions;
            Animating = Textures.Count > 1;
            entity.AddComponent<Transform>(transform);
            Visible = true;
            Loop = true;
            AnimationSpeed = 2.0f;
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
