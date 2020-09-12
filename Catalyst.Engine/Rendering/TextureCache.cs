using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Catalyst.Engine.Rendering
{
    public static class TextureCache
    {

        private static Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

        internal static bool LoadTexture(string path)
        {
            if (!_textures.ContainsKey(path))
            {
                if (!File.Exists(path))
                    return false;
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    _textures[path] = Texture2D.FromStream(Graphics.DeviceManager.GraphicsDevice, fs);
                }
            }
            return true;
        }

        internal static Texture2D GetTexture(string path)
        {
            if (!_textures.ContainsKey(path))
                return null;
            return _textures[path];
        }

        internal static bool HasTexture(string path)
        {
            return _textures.ContainsKey(path);
        }

        internal static void UnloadTexture(string path)
        {
            if (!_textures.ContainsKey(path))
            {
                _textures[path].Dispose();
                _ = _textures.Remove(path);
            }
        }

        internal static bool ReloadTexture(string path)
        {
            
            if (!File.Exists(path))
                return false;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                _textures[path].Reload(fs);
            }
            return true;
        }

        internal static void UnloadAll()
        {
            foreach (string texture in _textures.Keys)
            {
                UnloadTexture(texture);
            }
        }
    }
}
