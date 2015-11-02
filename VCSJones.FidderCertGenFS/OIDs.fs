namespace VCSJones.FiddlerCertGen
open System.Security.Cryptography

module OIDs =
    let EKU_SERVER = "1.3.6.1.5.5.7.3.1"
    let szOID_AUTHORITY_KEY_IDENTIFIER2 = "2.5.29.35"
    let szOID_SUBJECT_ALT_NAME2 = "2.5.29.17"
    let ECC_PUBLIC_KEY = "1.2.840.10045.2.1"
    let RSA_PUBLIC_KEY = "1.2.840.113549.1.1.1"
    let MD2rsa = "1.2.840.113549.1.1.2"
    let MD5rsa = "1.2.840.113549.1.1.4"
    let SHA1rsa = "1.2.840.113549.1.1.5"
    let SHA256rsa = "1.2.840.113549.1.1.11"
    let SHA384rsa = "1.2.840.113549.1.1.12"
    let SHA1ecdsa = "1.2.840.10045.4.1"
    let SHA256ecdsa = "1.2.840.10045.4.3.2"
    let SHA384ecdsa = "1.2.840.10045.4.3.3"