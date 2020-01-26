using System;
using System.IO;
using System.Reflection;
using Catalyst.XNA;

namespace Catalyst.XNA
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (var game = new CatalystEditor()) game.Run();
        }
    }
}