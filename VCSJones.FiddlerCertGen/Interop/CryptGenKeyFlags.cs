using System;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: Flags]
    internal enum CryptGenKeyFlags : ushort
    {
        CRYPT_EXPORTABLE = 0x00000001,
        CRYPT_USER_PROTECTED = 0x00000002,
        CRYPT_CREATE_SALT = 0x00000004,
        CRYPT_UPDATE_KEY = 0x00000008,
        CRYPT_NO_SALT = 0x00000010,
        CRYPT_PREGEN = 0x00000040,
        CRYPT_RECIPIENT = 0x00000010,
        CRYPT_INITIATOR = 0x00000040,
        CRYPT_ONLINE = 0x00000080,
        CRYPT_SF = 0x00000100,
        CRYPT_CREATE_IV = 0x00000200,
        CRYPT_KEK = 0x00000400,
        CRYPT_DATA_KEY = 0x00000800,
        CRYPT_VOLATILE = 0x00001000,
        CRYPT_SGCKEY = 0x00002000,
        CRYPT_ARCHIVABLE = 0x00004000,
    }
}