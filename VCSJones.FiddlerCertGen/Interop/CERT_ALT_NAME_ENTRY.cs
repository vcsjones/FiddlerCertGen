using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Sequential)]
    internal struct CERT_ALT_NAME_ENTRY
    {
        public CertAltNameChoice dwAltNameChoice;
        public CERT_ALT_NAME_ENTRY_UNION Value;
    }
}