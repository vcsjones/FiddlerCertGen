using System;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    /// <summary>
    /// Determines the support for features by platform.
    /// </summary>
    public static class PlatformSupport
    {
        private static bool? _hasCngSupport, _legacyKeyStoreName;

        /// <summary>
        /// True if the operating environment supports CNG, otherwise false.
        /// </summary>
        public static bool HasCngSupport
        {
            get
            {
                //Don't particularly care if execution and publication isn't locked, avoid locking here.
                if (_hasCngSupport == null)
                {
                    var handle = Kernel32.LoadLibraryEx("ncrypt.dll", IntPtr.Zero, LoadLibraryFlags.NONE);
                    //Check for an entry point that is well-known just incase we encounter a similarly named library
                    //Which isn't the one we want.
                    _hasCngSupport = !handle.IsInvalid && Kernel32.GetProcAddress(handle, "NCryptOpenKey") != IntPtr.Zero;
                    handle.Close();
                }
                return _hasCngSupport.Value;
            }
        }

        public static bool UseLegacyKeyStoreName
        {
            get
            {
                if (_legacyKeyStoreName == null)
                {
                    _legacyKeyStoreName = Environment.OSVersion.Version < new Version(5, 2);
                }
                return _legacyKeyStoreName.Value;
            }
        }
    }
}