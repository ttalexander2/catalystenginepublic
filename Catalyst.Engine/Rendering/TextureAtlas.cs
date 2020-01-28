using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public class TextureAtlas
    {
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

        public void LoadContent()
        {
            _texture = Engine.Instance.Content.Load<Texture2D>(Path);
        }
    }
}
