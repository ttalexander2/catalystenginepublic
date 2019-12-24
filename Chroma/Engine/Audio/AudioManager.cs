using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMOD;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

namespace Chroma.Engine.Audio
{
    public class AudioManager
    {
        public FMOD.Studio.System StudioSystem;
        public FMOD.Studio.Bank Bank;
        public FMOD.Studio.Bank Strings;

        public AudioManager()
        {
            FMOD.Studio.System.create(out StudioSystem);
           
        }

        public void Initialize()
        {
            StudioSystem.initialize(16, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, (IntPtr)null);
#if DEBUG
            FMOD.Debug.Initialize(DEBUG_FLAGS.LOG, DEBUG_MODE.FILE, null, "chroma_fmod_log.txt");
#endif
            string FMODPath = Path.Combine(ChromaGame.ContentDirectory, "FMOD", "Desktop"); 
            StudioSystem.loadBankFile(Path.Combine(FMODPath, "Master.strings.bank"), FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out Bank);
            StudioSystem.loadBankFile(Path.Combine(FMODPath, "Master.bank"), FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out Bank);
            
        }

        public void Update()
        {
            StudioSystem.update();
        }

        public void Unload()
        {
            StudioSystem.release();
        }

    }
}
