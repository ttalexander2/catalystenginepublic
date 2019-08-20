using System;
using System.Collections.Generic;
using Chroma.Engine.Graphics;
using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Scenes
{
    public class SceneLayer
    {
        public readonly List<Entity> EntityList = new List<Entity>();
        public string layerName;
        public readonly Dictionary<int, Sprite> Sprites = new Dictionary<int, Sprite>();
        public readonly Dictionary<int, Alarm> Alarms = new Dictionary<int, Alarm>();
        public bool visible = true;
        public bool hasCollisions = true;

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

        public void AddTimerComponent(int entityId, Alarm alarm)
        {
            Alarms.Add(entityId, alarm);
        }

        public void AddEntityList(List<Entity> entities)
        {
            EntityList.AddRange(entities);
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
            for(int i = 0; i < Sprites.Count; i++) Sprites[i].BeforeUpdate(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < Sprites.Count; i++) Sprites[i].Update(gameTime);
            for (int i = 0; i < Alarms.Count; i++) Alarms[i].Update(gameTime);
        }

        public virtual void AfterUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Sprites.Count; i++) Sprites[i].AfterUpdate(gameTime);
        }

        public virtual void BeforeRender(GameTime gameTime)
        {
            for (int i = 0; i < Sprites.Count; i++) Sprites[i].BeforeRender(gameTime);
        }

        public virtual void Render(GameTime gameTime)
        {
            for (int i = 0; i < Sprites.Count; i++) Sprites[i].Render(gameTime);
        }

        public virtual void AfterRender(GameTime gameTime)
        {
            for (int i = 0; i < Sprites.Count; i++) Sprites[i].AfterRender(gameTime);
        }

        public virtual void End()
        {
        }
    }
}