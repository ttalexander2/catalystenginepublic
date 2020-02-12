using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public class TextureAtlas
    {
        public string Name
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(Path);
            }
        }
        public string Path { get; private set; }
        public Dictionary<int, MTexture> Textures = new Dictionary<int, MTexture>();
        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
        }
        [NonSerialized]
        private Texture2D _texture;

        public TextureAtlas(string path)
        {
            Path = path;
        }

        public void LoadContent()
        {
            _texture = Graphics.Content.Load<Texture2D>(System.IO.Path.Combine("Content", "Atlases", System.IO.Path.GetFileNameWithoutExtension(Path)));
        }
    }
}
