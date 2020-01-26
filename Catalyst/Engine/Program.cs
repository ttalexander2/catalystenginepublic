using System;

namespace Catalyst.Engine
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
            using (var game = new CatalystEngine(Graphics.Width, Graphics.Height, "Catalyst", false))
            {
                game.Run();
            }
        }

        [STAThread]
        public static void Main(Scene scene)
        {
            using (var game = new CatalystEngine(scene, Graphics.Width, Graphics.Height, "Catalyst", false))
            {
                game.Run();
            }
        }
    }
}