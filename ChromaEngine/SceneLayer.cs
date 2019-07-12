using System;
using Microsoft.Xna.Framework;
namespace Chroma
{
    public class SceneLayer
    {
        public Boolean visible = true;
        public String layerName;

        public SceneLayer(String name)
        {
            layerName = name;
        }

        public void Update(GameTime gameTime)
        {
            //TODO: Update Logic
        }

        public void Draw(GameTime gameTime)
        {
            //TODO: Render Logic
        }
    }
}
