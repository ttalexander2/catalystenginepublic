using System;

namespace Catalyst.XNA
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (var game = new SampleGame()) game.Run();
        }
    }
}