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
    public class Atlas : ILoad
    {
        public string Name
        {
            get
            {
                return global::System.IO.Path.GetFileNameWithoutExtension(Path);
            }
        }
        public string Path { get; private set; }

        public Dictionary<int, PackedTexture> Textures = new Dictionary<int, PackedTexture>();
        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
        }
        [NonSerialized]
        private Texture2D _texture;

        public Atlas(string path)
        {
            Path = path;
        }

        public void LoadContent()
        {
            _texture = Graphics.Content.Load<Texture2D>(global::System.IO.Path.Combine("Content", "Atlases", global::System.IO.Path.GetFileName(Path)));
        }

        public void UnloadContent() { }
    }
}
