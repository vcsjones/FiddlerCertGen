namespace VCSJones.FiddlerCertGen

type PrivateKey(handle : NCryptKeyOrCryptProviderSafeHandle, keyProvider : KeyProviderBase, keySpec : KeySpec) = 
    static member CreateNew(keyProvider : KeyProviderBase) (keyName : string) (algorithm : Algorithm) (keyUsage : KeyUsage) (keySize : int option) (overwrite : bool) =
        let keySizeValue = keySize |? 2048
        let newKey = keyProvider.CreateKey keyName keySizeValue algorithm overwrite keyUsage
        new PrivateKey(fst newKey, keyProvider, snd newKey)

    static member OpenExisting(keyProvider : KeyProviderBase) (keyName : string) =
        match keyProvider.OpenExisting(keyName) with
        | None -> None
        | Some (handle, keySpec) -> Some(new PrivateKey(handle, keyProvider, keySpec))

    member this.KeySpec = keySpec
    member internal this.Handle = handle
    member this.Name = keyProvider.GetName(handle)
    member this.AlgorithmGroup = {name = keyProvider.GetAlgorithmGroup(handle)}
    member this.ProviderName = keyProvider.GetProviderName()

    member this.ToPublicKey() =
        let publicKeyObjId = if this.AlgorithmGroup = AlgorithmGroup.RSA then OIDs.RSA_PUBLIC_KEY else OIDs.ECC_PUBLIC_KEY
        let mutable infoSize = 0u
        if not <| ExternInterop.CryptExportPublicKeyInfoEx(handle, keySpec, EncodingType.X509_ASN_ENCODING ||| EncodingType.X509_ASN_ENCODING, publicKeyObjId, 0u, 0n, 0n, &infoSize) then
            invalidOp("Failed to get public key.")
        use buffer = GlobalBufferSafeHandle.Allocate (int infoSize)
        if not <| ExternInterop.CryptExportPublicKeyInfoEx(handle, keySpec, EncodingType.X509_ASN_ENCODING ||| EncodingType.X509_ASN_ENCODING, publicKeyObjId, 0u, 0n, buffer.DangerousGetHandle(), &infoSize) then
            invalidOp("Failed to get public key.")
        new PublicKeyInfo(buffer)

    interface System.IDisposable with
        member this.Dispose() = handle.Dispose()