namespace VCSJones.FiddlerCertGen

open System.Runtime.InteropServices
open System.Security.Cryptography
open System.Security.Cryptography.X509Certificates

type X509AlternativeName =
    | DNSName of string
    | IPAddress of System.Net.IPAddress
    | Url of string


module internal X509AlternativeNameEncoder =
    let encodeExtensions(altNames : X509AlternativeName list) : byte array =
        let mutable altName = CERT_ALT_NAME_INFO()
        altName.cAltEntry <- altNames |> List.length |> uint32
        let rec altNameEntry (altNames : X509AlternativeName list) (entries : (CERT_ALT_NAME_ENTRY * nativeint) list) =
            match altNames with
            | head :: tail ->
                                let mutable entry = CERT_ALT_NAME_ENTRY()
                                let mutable value = CERT_ALT_NAME_ENTRY_UNION()
                                match head with
                                | DNSName(dnsName) ->
                                    value.pwszDNSName <- Marshal.StringToHGlobalUni(dnsName)
                                    entry.dwAltNameChoice <- CertAltNameChoice.CERT_ALT_NAME_DNS_NAME
                                    entry.value <- value
                                    altNameEntry tail (entries |> List.append [(entry, value.pwszDNSName)])
                                | IPAddress(ipAddress) ->
                                    let addressBytes = ipAddress.GetAddressBytes()
                                    let nativeBytes = Marshal.AllocHGlobal(addressBytes.Length)
                                    Marshal.Copy(addressBytes, 0, nativeBytes, addressBytes.Length)
                                    value.IPAddress <- CRYPTOAPI_BLOB(addressBytes.Length |> uint32, nativeBytes)
                                    entry.dwAltNameChoice <- CertAltNameChoice.CERT_ALT_NAME_IP_ADDRESS
                                    entry.value <- value
                                    altNameEntry tail (entries |> List.append [(entry, nativeBytes)])
                                | Url(url) ->
                                    value.pwszURL <- Marshal.StringToHGlobalUni(url)
                                    entry.dwAltNameChoice <- CertAltNameChoice.CERT_ALT_NAME_URL
                                    entry.value <- value
                                    altNameEntry tail (entries |> List.append [(entry, value.pwszURL)])
            | [] -> entries
        let altNameList = altNameEntry altNames []
        let entries = altNameList |> List.map fst |> Array.ofList
        let handle = GCHandle.Alloc(entries, GCHandleType.Pinned)
        altName.rgAltEntry <- handle.AddrOfPinnedObject()
        let mutable buffer : LocalBufferSafeHandle = LocalBufferSafeHandle.Null
        let mutable dataSize : uint32 = 0u
        if not <| Crypto32.CryptEncodeObjectExAltnerateName(EncodingType.X509_ASN_ENCODING, OIDs.szOID_SUBJECT_ALT_NAME2, &altName, 0x8000u, 0n, &buffer, &dataSize) then
            invalidOp("Failed to encode object.")
        try
            using(buffer) (fun _ ->
                let dataBuffer = Array.zeroCreate<byte>(int dataSize)
                Marshal.Copy(buffer.DangerousGetHandle(), dataBuffer, 0, int dataSize)
                dataBuffer
            )
        finally
            handle.Free()
            altNameList |> Seq.map snd |> Seq.iter(Marshal.FreeHGlobal)

type X509SubjectAlternativeNameExtension(altNames : X509AlternativeName seq, critical : bool) = 
    inherit X509Extension(new Oid(OIDs.szOID_SUBJECT_ALT_NAME2), X509AlternativeNameEncoder.encodeExtensions(altNames |> Seq.toList), critical)