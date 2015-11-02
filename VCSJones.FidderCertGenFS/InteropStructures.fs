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