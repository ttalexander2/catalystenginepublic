using System.Collections.Generic;
using Chroma.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Scenes
{
    public class SceneLayer
    {
        private readonly List<Entity> _entityList = new List<Entity>();
        public string layerName;
        private readonly Dictionary<int, Sprite> _sprites = new Dictionary<int, Sprite>();
        public bool visible = true;

        public SceneLayer(string name)
        {
            layerName = name;
        }

        public void AddEntity(Entity entity)
        {
            _entityList.Add(entity);
        }

        public void AddSpriteComponent(int entityId, Sprite sprite)
        {
            _sprites.Add(entityId, sprite);
        }

        public Sprite GetSpriteComponent(int entityId)
        {
            return _sprites[entityId];
        }

        public void AddEntityList(List<Entity> entities)
        {
            foreach (var entity in entities) _entityList.Add(entity);
        }

        public void RemoveEntityList(List<Entity> entities)
        {
            foreach (var entity in entities) _entityList.Remove(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entityList.Remove(entity);
        }

        public List<Entity> GetEntityList()
        {
            return _entityList;
        }

        public virtual void Start()
        {
        }

        public virtual void BeforeUpdate(GameTime gameTime)
        {
            foreach (var sprite in _sprites.Values) sprite.BeforeUpdate(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var sprite in _sprites.Values) sprite.Update(gameTime);
        }

        public virtual void AfterUpdate(GameTime gameTime)
        {
            foreach (var sprite in _sprites.Values) sprite.AfterUpdate(gameTime);
        }

        public virtual void BeforeRender(GameTime gameTime)
        {
            foreach (var sprite in _sprites.Values) sprite.BeforeRender(gameTime);
        }

        public virtual void Render(GameTime gameTime)
        {
            foreach (var sprite in _sprites.Values) sprite.Render(gameTime);
        }

        public virtual void AfterRender(GameTime gameTime)
        {
            foreach (var sprite in _sprites.Values) sprite.AfterRender(gameTime);
        }

        public virtual void End()
        {
        }
    }
}