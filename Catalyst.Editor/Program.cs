using System;
using System.IO;
using System.Reflection;
using Catalyst.XNA;
using CatalystEditor;

namespace Catalyst.XNA
{
    public static class Program
    {

        [STAThread]
        public static void Main(string[] args)
        {
            using (var game = new CatalystEditor()) game.Run();
        }

        /**
        public static void Main(string[] args)
        {
            TexturePacker packer = new TexturePacker();
            packer.PackAtlas(Path.Combine(CatalystEditor.AssemblyDirectory, "Content", "Sprites"), Path.Combine(CatalystEditor.AssemblyDirectory, "Content", "Atlases"), "test");
        }
        */
    }
}