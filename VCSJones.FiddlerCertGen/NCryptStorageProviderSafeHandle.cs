using Microsoft.Win32.SafeHandles;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    internal class NCryptHandleBase : SafeHandleZeroOrMinusOneIsInvalid
    {
        public NCryptHandleBase(bool ownsHandle) : base(ownsHandle)
        {
        }

        public NCryptHandleBase() : base(true)
        {

        }

        protected override bool ReleaseHandle()
        {
            return NCrypt.NCryptFreeObject(handle) == SECURITY_STATUS.ERROR_SUCCESS;
        }
    }

    internal class NCryptStorageProviderSafeHandle : NCryptHandleBase
    {
        public NCryptStorageProviderSafeHandle(bool ownsHandle) : base(ownsHandle)
        {
        }

        public NCryptStorageProviderSafeHandle()
        {
        }
    }
}