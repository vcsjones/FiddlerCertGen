using Microsoft.Win32.SafeHandles;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    internal class CryptKeySafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public CryptKeySafeHandle(bool ownsHandle) : base(ownsHandle)
        {
        }

        public CryptKeySafeHandle() : base(true)
        {
            
        }

        protected override bool ReleaseHandle()
        {
            return AdvApi32.CryptDestroyKey(handle);
        }
    }
}