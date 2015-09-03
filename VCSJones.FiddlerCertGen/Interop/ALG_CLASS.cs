namespace VCSJones.FiddlerCertGen.Interop
{
    internal enum ALG_CLASS : uint
    {
        ALG_CLASS_ANY = (0),
        ALG_CLASS_SIGNATURE = (1 << 13),
        ALG_CLASS_MSG_ENCRYPT = (2 << 13),
        ALG_CLASS_DATA_ENCRYPT = (3 << 13),
        ALG_CLASS_HASH = (4 << 13),
        ALG_CLASS_KEY_EXCHANGE = (5 << 13),
        ALG_CLASS_ALL = (7 << 13),
    }
}