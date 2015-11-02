namespace VCSJones.FiddlerCertGen

open System.Runtime.InteropServices
open System.Security.Cryptography
open System.Security.Cryptography.X509Certificates

module internal X509AuthorityKeyIdentifierEncoder = 
    let encodeExtensions(publicKey : byte array option, serialNumber : byte array option) =
        use serialNumberPtr = match serialNumber with
                                | None -> GlobalBufferSafeHandle.Null
                                | Some serial -> 
                                    let ptr = GlobalBufferSafeHandle.FromHandle(Marshal.AllocHGlobal(serial.Length))
                                    Marshal.Copy(serial, 0, ptr.DangerousGetHandle(), serial.Length)
                                    ptr
        use publicKeyPtr = match publicKey with
                                | None -> GlobalBufferSafeHandle.Null
                                | Some key -> 
                                    let ptr = GlobalBufferSafeHandle.FromHandle(Marshal.AllocHGlobal(key.Length))
                                    Marshal.Copy(key, 0, ptr.DangerousGetHandle(), key.Length)
                                    ptr
        let mutable identifier =
            CERT_AUTHORITY_KEY_ID2_INFO(
                CRYPTOAPI_BLOB((match publicKey with | None -> 0u | Some s -> uint32(s.Length)), publicKeyPtr.DangerousGetHandle()),
                CERT_ALT_NAME_INFO(cAltEntry = 0u),
                CRYPTOAPI_BLOB((match serialNumber with | None -> 0u | Some s -> uint32(s.Length)), serialNumberPtr.DangerousGetHandle())
            )
        let mutable buffer : LocalBufferSafeHandle = LocalBufferSafeHandle.Null
        let mutable dataSize : uint32 = 0u
        if not <| Crypto32.CryptEncodeObjectExAuthority(EncodingType.X509_ASN_ENCODING, OIDs.szOID_AUTHORITY_KEY_IDENTIFIER2, &identifier, 0x8000u, 0n, &buffer, &dataSize) then
            invalidOp("Failed to encode object.")
        using(buffer) (fun _ ->
            let dataBuffer = Array.zeroCreate<byte>(int dataSize)
            Marshal.Copy(buffer.DangerousGetHandle(), dataBuffer, 0, int dataSize)
            dataBuffer
        )

type X509AuthorityKeyIdentifierExtension(publicKey : byte array option, serialNumber : byte array option) = 
    inherit X509Extension(new Oid(OIDs.szOID_AUTHORITY_KEY_IDENTIFIER2), X509AuthorityKeyIdentifierEncoder.encodeExtensions(publicKey, serialNumber), false)