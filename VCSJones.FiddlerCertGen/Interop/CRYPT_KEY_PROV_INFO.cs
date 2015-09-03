using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct CRYPT_KEY_PROV_INFO
    {
        public string pwszContainerName;
        public string pwszProvName;
        public ProviderType dwProvType;
        public CryptKeyProvInfoFlags dwFlags;
        public uint cProvParam;
        public IntPtr rgProvParam;
        public KeySpec dwKeySpec;
    }
}