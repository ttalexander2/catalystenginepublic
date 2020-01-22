using System;

namespace Chroma.Engine
{
    /// <summary>
    ///     The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var game = new ChromaGame(Global.Width, Global.Height, "Chroma", false))
            {
                game.Run();
            }
        }

        [STAThread]
        public static void Main(Scene scene)
        {
            using (var game = new ChromaGame(scene, Global.Width, Global.Height, "Chroma", false))
            {
                game.Run();
            }
        }
    }
}