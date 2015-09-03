using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Sequential)]
    internal struct CRYPTOAPI_BLOB
    {
        public uint cbData;
        public IntPtr pbData;
    }
}