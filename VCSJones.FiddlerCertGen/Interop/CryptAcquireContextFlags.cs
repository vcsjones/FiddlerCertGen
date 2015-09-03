namespace VCSJones.FiddlerCertGen.Interop
{
    internal enum CryptAcquireContextFlags : uint
    {
        CRYPT_VERIFYCONTEXT = 0xF0000000U,
        CRYPT_NEWKEYSET = 0x00000008U,
        CRYPT_DELETEKEYSET = 0x00000010U,
        CRYPT_MACHINE_KEYSET = 0x00000020U,
        CRYPT_SILENT = 0x00000040U,
    }
}