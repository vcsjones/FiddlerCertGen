using System;
using System.Runtime.InteropServices;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Sequential)]
    internal struct CERT_INFO
    {
        public CertificateVersion dwVersion;
        public NATIVE_CRYPTOAPI_BLOB SerialNumber;
        public CRYPT_ALGORITHM_IDENTIFIER SignatureAlgorithm;
        public NATIVE_CRYPTOAPI_BLOB Issuer;
        public FILETIME NotBefore;
        public FILETIME NotAfter;
        public NATIVE_CRYPTOAPI_BLOB Subject;
        public CERT_PUBLIC_KEY_INFO SubjectPublicKeyInfo;
        public CRYPT_BIT_BLOB IssuerUniqueId;
        public CRYPT_BIT_BLOB SubjectUniqueId;
        public uint cExtension;
        public IntPtr rgExtension;
    }
}