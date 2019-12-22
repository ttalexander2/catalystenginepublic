using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMOD;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Chroma.Engine.Audio
{
    public class AudioManager
    {
        public FMOD.Studio.System StudioSystem;

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        public AudioManager()
        {
            if (Environment.Is64BitProcess)
                LoadLibrary(System.IO.Path.GetFullPath("x64\\fmodstudio.dll"));
            else
                LoadLibrary(System.IO.Path.GetFullPath("x86\\fmodstudio.dll"));

            FMOD.Studio.System.create(out StudioSystem);
        }

        public void Initialize()
        {
            
            StudioSystem.initialize(10, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, (IntPtr)null);
        }

    }
}
