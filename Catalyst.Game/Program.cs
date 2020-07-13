using Catalyst.Engine;
using Catalyst.Engine.Utilities;
using System;
using System.IO;

namespace Catalyst
{
    /// <summary>
    ///     The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        public static void Main()
        {
#if OSX
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "libfmod.dylib"));
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "libfmodstudio.dylib"));
#elif LINUX32
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "x86", "libfmod.so"));
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "x86", "libfmodstudio.so"));
#elif LINUX64
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "x64", "libfmod.so"));
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "x64", "libfmodstudio.so"));
#endif
            using (var game = new Catalyst.Engine.Engine(Graphics.Width, Graphics.Height, "Catalyst", false))
            {
                game.Run();
            }
        }
    }
}