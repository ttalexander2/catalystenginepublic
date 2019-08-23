﻿using System;

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
            using (var game = new ChromaGame(Global.NativeWidth * (int) Global.PixelScale,
                Global.NativeHeight * (int) Global.PixelScale, 640, 360, "Chroma", false))
            {
                game.Run();
            }
        }
    }
}