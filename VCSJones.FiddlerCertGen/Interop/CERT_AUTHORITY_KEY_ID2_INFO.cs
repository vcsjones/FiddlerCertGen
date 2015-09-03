using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Sequential)]
    internal struct CERT_AUTHORITY_KEY_ID2_INFO
    {
        public CRYPTOAPI_BLOB KeyId;
        public CERT_ALT_NAME_INFO AuthorityCertIssuer;
        public CRYPTOAPI_BLOB AuthorityCertSerialNumber;
    }
}