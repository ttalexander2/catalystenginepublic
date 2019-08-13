using System;
using System.Collections.Generic;
using Chroma.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Scenes
{
    public class SceneLayer
    {
        public readonly List<Entity> EntityList = new List<Entity>();
        public string layerName;
        public readonly Dictionary<int, Sprite> Sprites = new Dictionary<int, Sprite>();
        public bool visible = true;

        public SceneLayer(string name)
        {
            layerName = name;
        }

        public void AddEntity(Entity entity)
        {
            EntityList.Add(entity);
        }

        public void AddSpriteComponent(int entityId, Sprite sprite)
        {
            Sprites.Add(entityId, sprite);
        }

        public Sprite GetSpriteComponent(int entityId)
        {
            return Sprites[entityId];
        }

        public void AddEntityList(List<Entity> entities)
        {
            foreach (var entity in entities) EntityList.Add(entity);
        }

        public void RemoveEntityList(List<Entity> entities)
        {
            foreach (var entity in entities) EntityList.Remove(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            EntityList.Remove(entity);
        }

        public List<Entity> GetEntityList()
        {
            return EntityList;
        }

        public virtual void Start()
        {
        }

        public virtual void BeforeUpdate(GameTime gameTime)
        {
            foreach (var sprite in Sprites.Values) sprite.BeforeUpdate(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var sprite in Sprites.Values) sprite.Update(gameTime);
        }

        public virtual void AfterUpdate(GameTime gameTime)
        {
            foreach (var sprite in Sprites.Values) sprite.AfterUpdate(gameTime);
        }

        public virtual void BeforeRender(GameTime gameTime)
        {
            foreach (var sprite in Sprites.Values) sprite.BeforeRender(gameTime);
        }

        public virtual void Render(GameTime gameTime)
        {
            foreach (var sprite in Sprites.Values) sprite.Render(gameTime);
        }

        public virtual void AfterRender(GameTime gameTime)
        {
            foreach (var sprite in Sprites.Values) sprite.AfterRender(gameTime);
        }

        public virtual void End()
        {
        }
    }
}