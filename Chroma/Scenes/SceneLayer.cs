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

        public SceneLayer(String name)
        {
            this.layerName = name;
        }

        public void AddEntity(Entity entity)
        {
            entityList.Add(entity);
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

        public virtual void Update(GameTime gameTime)
        {
            foreach (Entity entity in entityList)
            {
                entity.BeforeUpdate();
                entity.Update();
                entity.AfterUpdate();
            }
        }

        public virtual void Render(GameTime gameTime)
        {
            foreach (Entity entity in entityList)
            {
                entity.BeforeRender();
                entity.Render();
                entity.AfterRender();
            }
        }
    }
}
