using Microsoft.Win32.SafeHandles;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    internal class LibrarySafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public LibrarySafeHandle(bool ownsHandle) : base(ownsHandle)
        {
        }

        public LibrarySafeHandle() : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return Kernel32.FreeLibrary(handle);
        }
    }
}
