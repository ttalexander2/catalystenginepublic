using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Chroma
{
    public class Scene
    {
        private List<SceneLayer> scenes = new List<SceneLayer>();

        public Scene(int width, int height)
        {

        }



        public SceneLayer CreateLayer()
        {
            scenes.Add(new SceneLayer("untitled" + scenes.Count));
            return scenes[scenes.Count-1];
        }

        public List<SceneLayer> GetLayerList()
        {
            return scenes;
        }
    }
}
