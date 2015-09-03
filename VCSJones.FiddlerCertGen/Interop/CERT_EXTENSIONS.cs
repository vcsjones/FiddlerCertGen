using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Sequential)]
    internal struct CERT_EXTENSIONS
    {
        public uint cExtension;
        public IntPtr rgExtension;
    }
}