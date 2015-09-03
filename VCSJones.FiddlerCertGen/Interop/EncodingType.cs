using System;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: Flags]
    public enum EncodingType : uint
    {
        PKCS_7_ASN_ENCODING = 65536,
        X509_ASN_ENCODING = 1
    }
}