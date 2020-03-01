using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMOD;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

namespace Catalyst.Engine.Audio
{
    /// <summary>
    /// Manages FMOD audio, creates, loads, updates and unloads fmod system.
    /// Currently loads the master bank.
    /// </summary>
    public class AudioManager
    {
        /// <summary>
        /// System controlling game audio.
        /// </summary>
        public FMOD.Studio.System StudioSystem;
        /// <summary>
        /// Master bank holding all game audio.
        /// </summary>
        public FMOD.Studio.Bank Bank;
        /// <summary>
        /// Reference bank containing strings used for references.
        /// </summary>
        public FMOD.Studio.Bank Strings;

        /// <summary>
        /// Creates the FMOD System. Check Console.Error if audio does not work.
        /// </summary>
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

        /// <summary>
        /// Function to initialize the FMOD Studio system. Loads "master.bank" and "master.strings.bank"
        /// </summary>
        /// <param name="liveUpdate">Initialize with live update integration with FMOD Studio.</param>
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
                string FMODPath = Path.Combine(Engine.ContentDirectory, "FMOD", "Desktop");
                StudioSystem.loadBankFile(Path.GetFullPath(Path.Combine(FMODPath, "Master.strings.bank")), FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out Bank);
                StudioSystem.loadBankFile(Path.GetFullPath(Path.Combine(FMODPath, "Master.bank")), FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out Bank);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception occured when trying to load FMOD banks: ", e);
            }

            
        }

        /// <summary>
        /// Update call for FMOD. Called once per frame.
        /// </summary>
        public void Update()
        {
            StudioSystem.update();
        }

        /// <summary>
        /// Release FMOD studio system.
        /// </summary>
        public void Unload()
        {
            StudioSystem.release();
        }

    }
}

