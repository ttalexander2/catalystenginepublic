using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework;

namespace Chroma
{
    public class Scene
    {
        private List<SceneLayer> layers = new List<SceneLayer>();

        public Scene(int width, int height)
        {

        }



        public SceneLayer CreateLayer()
        {
            layers.Add(new SceneLayer("untitled" + layers.Count));
            return layers[layers.Count-1];
        }

        public List<SceneLayer> GetLayerList()
        {
            return layers;
        }

        public void Update(GameTime gameTime)
        {
            foreach (SceneLayer layer in layers)
            {
                layer.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (SceneLayer layer in layers)
            {
                layer.Draw(gameTime);
            }
        }
    }
}
