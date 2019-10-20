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
            using (var game = new Engine(Global.Width, Global.Height, "Chroma", false))
            {
                game.Run();
            }
        }
    }
}