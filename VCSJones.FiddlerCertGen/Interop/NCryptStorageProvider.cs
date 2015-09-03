using System;
using System.ComponentModel;

namespace VCSJones.FiddlerCertGen.Interop
{
    public sealed class NCryptStorageProvider : IDisposable
    {
        private readonly NCryptStorageProviderSafeHandle _handle;

        public static NCryptStorageProvider MicrosoftSoftwareKeyStorageProvider { get; } = new NCryptStorageProvider("Microsoft Software Key Storage Provider");
        public static NCryptStorageProvider MicrosoftSmartCardProvider { get; } = new NCryptStorageProvider("Microsoft Smart Card Key Storage Provider");

        public string Name => NCryptPropertyReader.ReadStringUni(_handle, CngProperties.NCRYPT_NAME_PROPERTY);

        private NCryptStorageProvider(string name)
        {
            var result = NCrypt.NCryptOpenStorageProvider(out _handle, name, 0u);
            if (result != SECURITY_STATUS.ERROR_SUCCESS)
            {
                throw new Win32Exception(unchecked((int) result), "Failed to open the CNG storage provider.");
            }
        }



        internal NCryptStorageProviderSafeHandle Handle => _handle;

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}
