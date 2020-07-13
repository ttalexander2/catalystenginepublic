using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Utilities;

namespace Catalyst.DynamicCompilationTest
{
    public static class AssemblyReloader
    {
        private static string _directory;
        private static string _filename;
        private static WeakReference _hostAlcWeakRef;
        private static SimpleUnloadableAssemblyLoadContext _context;


        [STAThread]
        public static void WatchProjectDirectory(string directory, string filename)
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                _directory = directory;
                _filename = filename;
                Console.WriteLine($"Running from: {Environment.CurrentDirectory}");
                Console.WriteLine($"Sources from: {Path.Combine(directory, filename)}");

                watcher.Path = directory;
                watcher.Changed += OnChanged;
                watcher.EnableRaisingEvents = true;

                Console.WriteLine("Press 'q' to exit");

                while (Console.Read() != 'q')
                {
                    
                }
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            ReloadAssembly(_directory, _filename);
        }

        public static void ReloadAssembly(string directory, string filename)
        {
            string filepath = Path.Combine(directory, filename);
            Console.WriteLine(filepath);
            if (!File.Exists(filepath))
            {
                Console.WriteLine(string.Format("Failed to find file: {0}", filepath));
                return;
            }


            if (_hostAlcWeakRef != null)
            {
                _context.Unload();
                // Poll and run GC until the AssemblyLoadContext is unloaded.
                // You don't need to do that unless you want to know when the context
                // got unloaded. You can just leave it to the regular GC.

                for (int i = 0; _hostAlcWeakRef.IsAlive && (i < 10); i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }


            _context = new SimpleUnloadableAssemblyLoadContext();
            _ = _context.LoadFromAssemblyPath(filepath);

            _hostAlcWeakRef = new WeakReference(_context, trackResurrection: true);
        }
    }

    internal class SimpleUnloadableAssemblyLoadContext : AssemblyLoadContext
    {
        public SimpleUnloadableAssemblyLoadContext()
            : base(true)
        {
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
