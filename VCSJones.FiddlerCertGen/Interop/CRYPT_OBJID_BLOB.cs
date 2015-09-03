using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Sequential)]
    internal struct CRYPT_OBJID_BLOB
    {
        public uint cbData;
        public IntPtr pbData;
    }
}