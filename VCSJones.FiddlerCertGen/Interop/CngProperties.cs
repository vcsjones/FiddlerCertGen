using System;

namespace VCSJones.FiddlerCertGen.Interop
{
    internal static class CngProperties
    {
        public const string NCRYPT_ALGORITHM_GROUP_PROPERTY = "Algorithm Group";
        public const string NCRYPT_LENGTH_PROPERTY = "Length";
        public const string NCRYPT_NAME_PROPERTY = "Name";
        public const string NCRYPT_EXPORT_POLICY_PROPERTY = "Export Policy";
    }

    [type: Flags]
    public enum CngExportPolicy
    {
        NCRYPT_ALLOW_EXPORT_FLAG = 0x00000001,
        NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG = 0x00000002,
        NCRYPT_ALLOW_ARCHIVING_FLAG = 0x00000004,
        NCRYPT_ALLOW_PLAINTEXT_ARCHIVING_FLAG = 0x00000008,
    }
}