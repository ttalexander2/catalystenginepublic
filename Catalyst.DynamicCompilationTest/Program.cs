using System;
using System.IO;

namespace Catalyst.DynamicCompilationTest
{
    class Program
    {
        private static string sourcesPath;
        static void Main(string[] args)
        {
            sourcesPath = @"C:\Users\Thomas\Desktop\test\Test.Source\bin\Debug";

            AssemblyReloader.WatchProjectDirectory(sourcesPath, "Test.Source.dll");

        }
    }
}
