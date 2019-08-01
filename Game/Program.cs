using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var game = new Chroma.Engine(640, 360, 640 * 3, 360 * 3, "Chroma", false))
                game.Run();
        }
    }
}
