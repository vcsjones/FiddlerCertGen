namespace VCSJones.FiddlerCertGen
[<AutoOpenAttribute>]
module internal SecurityStatuses =
    let ERROR_SUCCESS = 0u
    let NTE_BAD_KEYSET = 0x80090016u

module internal CngConstants =
    let NCRYPT_LENGTH_PROPERTY = "Length"
    let NCRYPT_NAME_PROPERTY = "Name"
    let NCRYPT_ALGORITHM_GROUP_PROPERTY = "Algorithm Group"
    let NCRYPT_EXPORT_POLICY_PROPERTY = "Export Policy"

module internal CapiConstants =
    let ALREADY_EXISTS = 0x8009000f
    let DOES_NOT_EXIST = 0x80090016