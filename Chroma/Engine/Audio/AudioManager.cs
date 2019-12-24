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
            try
            {
                FMOD.Studio.System.create(out StudioSystem);
            } 
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception occured when trying to initialize FMOD: ", e);
            }

        }

        public void Initialize(bool liveUpdate)
        {
            if (liveUpdate)
            {
                StudioSystem.initialize(16, FMOD.Studio.INITFLAGS.LIVEUPDATE, FMOD.INITFLAGS.NORMAL, (IntPtr)null);
            }
            else
            {
                StudioSystem.initialize(16, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, (IntPtr)null);
            }

#if DEBUG
            FMOD.Debug.Initialize(DEBUG_FLAGS.LOG, DEBUG_MODE.FILE, null, "chroma_fmod_log.txt");
#endif
            try
            {
                string FMODPath = Path.Combine(ChromaGame.ContentDirectory, "FMOD", "Desktop");
                StudioSystem.loadBankFile(Path.GetFullPath(Path.Combine(FMODPath, "Master.strings.bank")), FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out Bank);
                StudioSystem.loadBankFile(Path.GetFullPath(Path.Combine(FMODPath, "Master.bank")), FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out Bank);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception occured when trying to load FMOD banks: ", e);
            }

            
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
