using System;

namespace Chroma
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var game = new Engine(640*(int)(Global.Scale), 360*(int)(Global.Scale), 640, 360, "Chroma", false))
                game.Run();
        }
    }
}
