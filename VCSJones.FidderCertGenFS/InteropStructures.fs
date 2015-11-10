namespace VCSJones.FiddlerCertGen
#nowarn "9"
open System.Runtime.InteropServices

[<type: StructLayoutAttribute(LayoutKind.Sequential); StructAttribute>]
type internal CRYPTOAPI_BLOB =
    val cbData : uint32
    val pbData : nativeint
    new(cbDataVal, pbDataVal) = {cbData = cbDataVal; pbData = pbDataVal}

[<type: StructLayoutAttribute(LayoutKind.Explicit, CharSet = CharSet.Unicode); StructAttribute>]
type internal CERT_ALT_NAME_ENTRY_UNION =
    [<field: FieldOffsetAttribute(0); DefaultValueAttribute>]
    val mutable pOtherName : nativeint
    [<field: FieldOffsetAttribute(0); DefaultValueAttribute>]
    val mutable pwszRfc822Name : nativeint
    [<field: FieldOffsetAttribute(0); DefaultValueAttribute>]
    val mutable pwszDNSName : nativeint
    [<field: FieldOffsetAttribute(0); DefaultValueAttribute>]
    val mutable DirectoryName : CRYPTOAPI_BLOB
    [<field: FieldOffsetAttribute(0); DefaultValueAttribute>]
    val mutable pwszURL : nativeint
    [<field: FieldOffsetAttribute(0); DefaultValueAttribute>]
    val mutable IPAddress : CRYPTOAPI_BLOB
    [<field: FieldOffsetAttribute(0); DefaultValueAttribute>]
    val mutable pszRegisteredID : nativeint

[<type: StructLayoutAttribute(LayoutKind.Sequential); StructAttribute>]
type internal CERT_ALT_NAME_ENTRY = 
    [<field: DefaultValueAttribute>]val mutable dwAltNameChoice : CertAltNameChoice
    [<field: DefaultValueAttribute>]val mutable value : CERT_ALT_NAME_ENTRY_UNION

[<type: StructLayoutAttribute(LayoutKind.Sequential); StructAttribute>]
type internal CERT_ALT_NAME_INFO =
    [<field: DefaultValueAttribute>]val mutable cAltEntry : uint32
    [<field: DefaultValueAttribute>]val mutable rgAltEntry : nativeint

[<type: StructLayoutAttribute(LayoutKind.Sequential); StructAttribute>]
type internal CERT_AUTHORITY_KEY_ID2_INFO =
    val KeyId : CRYPTOAPI_BLOB
    val AuthorityCertIssuer : CERT_ALT_NAME_INFO
    val AuthorityCertSerialNumber : CRYPTOAPI_BLOB

    new(keyId, authorityCertIssuer, authorityCertSerialNumber) = {
            KeyId = keyId
            AuthorityCertIssuer = authorityCertIssuer
            AuthorityCertSerialNumber = authorityCertSerialNumber
        }

[<type: StructLayoutAttribute(LayoutKind.Sequential); StructAttribute>]
type internal CRYPT_OBJID_BLOB =
    val mutable cbData : uint32
    val mutable pbData : nativeint

[<type: StructLayoutAttribute(LayoutKind.Sequential); StructAttribute>]
type internal CRYPT_BIT_BLOB =
    val mutable cbData : uint32
    val mutable pbData : nativeint
    val mutable cUnusedBits : uint32

[<type: StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi); StructAttribute>]
type internal CERT_EXTENSION =
    val mutable pszObjId : string
    val mutable fCritical : bool
    val mutable Value : CRYPT_OBJID_BLOB

[<type: StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi); StructAttribute>]
type internal CRYPT_ALGORITHM_IDENTIFIER =
    val mutable pszObjId : string
    val mutable Parameters : CRYPT_OBJID_BLOB

[<type: StructLayoutAttribute(LayoutKind.Sequential); StructAttribute>]
type internal CERT_PUBLIC_KEY_INFO =
    val mutable Algorithm : CRYPT_ALGORITHM_IDENTIFIER
    val mutable PubicKey : CRYPT_BIT_BLOB


[<type: StructLayoutAttribute(LayoutKind.Sequential); StructAttribute>]
type internal CERT_INFO =
    val mutable dwVersion : CertificateVersion
    val mutable SerialNumber : CRYPTOAPI_BLOB
    val mutable SignatureAlgorithm : CRYPT_ALGORITHM_IDENTIFIER
    val mutable Issuer : CRYPTOAPI_BLOB
    val mutable NotBefore: System.Runtime.InteropServices.ComTypes.FILETIME
    val mutable NotAfter : System.Runtime.InteropServices.ComTypes.FILETIME
    val mutable Subject : CRYPTOAPI_BLOB
    val mutable SubjectPublicKeyInfo : CERT_PUBLIC_KEY_INFO
    val mutable IssuerUniqueId : CRYPT_BIT_BLOB
    val mutable SubjectUniqueId : CRYPT_BIT_BLOB
    val mutable cExtension : uint32
    val mutable rgExtension : nativeint