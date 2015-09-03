using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Sequential)]
    internal struct CERT_ALT_NAME_INFO
    {
        public uint cAltEntry;
        public IntPtr rgAltEntry;
    }
}