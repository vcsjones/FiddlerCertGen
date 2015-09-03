namespace VCSJones.FiddlerCertGen
{
    internal static class OIDs
    {
        public const string EKU_SERVER = "1.3.6.1.5.5.7.3.1";
        public const string szOID_AUTHORITY_KEY_IDENTIFIER2 = "2.5.29.35";
        public const string szOID_SUBJECT_ALT_NAME2 = "2.5.29.17";
        public const string ECC_PUBLIC_KEY = "1.2.840.10045.2.1";
        public const string RSA_PUBLIC_KEY = "1.2.840.113549.1.1.1";
        public const string SHA1rsa = "1.2.840.113549.1.1.5";
        public const string SHA256rsa = "1.2.840.113549.1.1.11";
        public const string SHA384rsa = "1.2.840.113549.1.1.12";

        public const string SHA1ecdsa = "1.2.840.10045.4.1";
        public const string SHA256ecdsa = "1.2.840.10045.4.3.2";
        public const string SHA384ecdsa = "1.2.840.10045.4.3.3";
    }
}