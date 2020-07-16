using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CatalystEditor
{
    public static class LoadManager
    {
        public static void Initialize()
        {
            /**
            using (var watcher = new ObservableFileSystemWatcher(c => { c.Path = ProjectManager.ProjectPath; }))
            {
                var changes = watcher.Changed;

                changes.Subscribe(filepath => { CompileAndLoad(filepath);});

                watcher.Start();
            }
            */
        }
        /**
        private static void CompileAndLoad(string filepath)
        {

            if (_reference != null && _reference.IsAlive)
            {
                _loader.Unload();
            }
            _loader = new AssemblyContextLoader();

                var assembly = _loader.LoadFromStream(asm);

            for (var i = 0; i < 8 && _reference.IsAlive; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            _reference = new WeakReference(_loader);
            

        }

        */
    }
}
