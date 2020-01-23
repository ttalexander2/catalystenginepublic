using System;
using System.IO;
using System.Reflection;

namespace Catalyst.XNA
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine(Crunch.Start(3, Path.Combine("Content", "Atlases", "Atlas"), Path.Combine("Content", "Sprites", "Player"), "-d"));
            //using (var game = new SampleGame()) game.Run();
        }
    }
}