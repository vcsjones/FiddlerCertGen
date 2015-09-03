using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Sequential)]
    internal struct NATIVE_CRYPTOAPI_BLOB
    {
        public uint cbData;
        public unsafe byte* pbData;
    }
}