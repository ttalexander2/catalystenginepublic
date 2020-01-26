using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Utilities
{
    internal static class LibraryLoader
    {
        [DllImport("libdl")]
        static extern IntPtr dlopen(String fileName, int flags);

        [DllImport("libdl")]
        static extern IntPtr dlerror();

        [DllImport("libdl")]
        static extern IntPtr dlsym(IntPtr handle, String symbol);

        internal static void LoadPosixLibrary(string path)
        {
            const int RTLD_NOW = 2;
                if (File.Exists(path))
                {
                    var addr = dlopen(path, RTLD_NOW);
                    if (addr == IntPtr.Zero)
                    {
                        var error = Marshal.PtrToStringAnsi(dlerror());
                        throw new Exception("dlopen failed: " + path + " : " + error);
                    }
                    return;
                }
            throw new Exception("dlopen failed: unable to locate library " + path);
        }
    }


}
