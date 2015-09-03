namespace VCSJones.FiddlerCertGen.Interop
{
    internal enum SelfSignFlags : uint
    {
        NONE = 0,
        CERT_CREATE_SELFSIGN_NO_SIGN = 1,
        CERT_CREATE_SELFSIGN_NO_KEY_INFO = 2
    }
}