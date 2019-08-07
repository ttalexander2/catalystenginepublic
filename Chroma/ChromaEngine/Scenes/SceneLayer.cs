using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace Chroma
{
    public class SceneLayer
    {
        public Boolean visible = true;
        public String layerName;

        private List<Entity> entityList = new List<Entity>();
        private Dictionary<int, Sprite> sprites = new Dictionary<int, Sprite>();

        public SceneLayer(String name)
        {
            this.layerName = name;
        }

        public void AddEntity(Entity entity)
        {
            entityList.Add(entity);
        }

        public void AddSpriteComponent(int entityID, Sprite sprite)
        {
            sprites.Add(entityID, sprite);
        }

        public Sprite GetSpriteComponent(int entityID)
        {
            return sprites[entityID];
        }

        public void AddEntityList(List<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                entityList.Add(entity);
            }
        }

        public void RemoveEntityList(List<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                entityList.Remove(entity);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            entityList.Remove(entity);
        }

        public List<Entity> GetEntityList()
        {
            return entityList;
        }

        public virtual void BeforeUpdate(GameTime gameTime)
        {
            foreach (Sprite sprite in sprites.Values)
            {
                sprite.BeforeUpdate(gameTime);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Sprite sprite in sprites.Values)
            {
                sprite.Update(gameTime);
            }
        }

        public virtual void AfterUpdate(GameTime gameTime)
        {
            foreach (Sprite sprite in sprites.Values)
            {
                sprite.AfterUpdate(gameTime);
            }
        }

        public virtual void BeforeRender(GameTime gameTime)
        {
            foreach (Sprite sprite in sprites.Values)
            {
                sprite.BeforeRender(gameTime);
            }
        }

        public virtual void Render(GameTime gameTime)
        {
            foreach(Sprite sprite in sprites.Values)
            {
                sprite.Render(gameTime);
            }
        }

        public virtual void AfterRender(GameTime gameTime)
        {
            foreach (Sprite sprite in sprites.Values)
            {
                sprite.AfterRender(gameTime);
            }
        }
    }
}
