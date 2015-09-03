namespace VCSJones.FiddlerCertGen
{
    public enum CryptKeyProvInfoFlags : uint
    {
        CERT_SET_KEY_PROV_HANDLE_PROP_ID = 0x00000001,
        CERT_SET_KEY_CONTEXT_PROP_ID = 0x00000001,
        NCRYPT_MACHINE_KEY_FLAG = 0x00000020,
        NCRYPT_SILENT_FLAG = 0x00000040
    }
}