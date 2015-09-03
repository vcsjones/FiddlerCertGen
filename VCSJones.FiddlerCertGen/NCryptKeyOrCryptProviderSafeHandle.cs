using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    internal class NCryptKeyOrCryptProviderSafeHandle : NCryptHandleBase
    {
        public NCryptKeyOrCryptProviderSafeHandle(bool ownsHandle) : base(ownsHandle)
        {
        }

        public NCryptKeyOrCryptProviderSafeHandle() : base(true)
        {

        }

        public bool IsNCryptKey => NCrypt.NCryptIsKeyHandle(handle);

        protected override bool ReleaseHandle()
        {
            if (IsNCryptKey)
            {
                return NCrypt.NCryptFreeObject(handle) == SECURITY_STATUS.ERROR_SUCCESS;
            }
            else
            {
                return AdvApi32.CryptReleaseContext(handle, 0u);
            }
        }
    }
}