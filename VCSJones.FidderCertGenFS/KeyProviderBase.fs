namespace VCSJones.FiddlerCertGen
open System.Runtime.InteropServices
open CapiConstants

[<StructuralComparisonAttribute; StructuralEqualityAttribute>]
type AlgorithmGroup =
    private {
        name : string
    }
    static member RSA = {name = "RSA"}
    static member ECDSA = {name = "ECDSA"}
    override this.ToString() = this.name

    static member op_Equality (left : AlgorithmGroup, right : AlgorithmGroup) = left = right
    static member op_Inequality (left : AlgorithmGroup, right : AlgorithmGroup) = left <> right

    static member TryParse value =
        match value with
        | "RSA" -> Success AlgorithmGroup.RSA
        | "ECDSA" -> Success AlgorithmGroup.ECDSA
        | _ -> Fail

[<AbstractClassAttribute>]
type KeyProviderBase() =
    abstract member GetProviderName : unit -> string
    abstract member CreateKey : string -> int32 -> Algorithm -> bool -> KeyUsage -> (NCryptKeyOrCryptProviderSafeHandle * KeySpec)
    abstract member GetName : NCryptKeyOrCryptProviderSafeHandle -> string
    abstract member GetAlgorithmGroup : NCryptKeyOrCryptProviderSafeHandle -> string
    abstract member OpenExisting : string -> (NCryptKeyOrCryptProviderSafeHandle * KeySpec) option
    abstract member Name : string

and CngKeyProvider() =
    inherit KeyProviderBase()

    let storageProvider = NCryptStorageProvider.MicrosoftSoftwareKeyStorageProvider
    static let instance = CngKeyProvider()
    static member Instance = instance
    override this.Name = "CNG"
    override this.GetProviderName() = storageProvider.Name
    override this.GetName handle = NCryptProperties.ReadStringUnicode handle CngConstants.NCRYPT_NAME_PROPERTY
    override this.GetAlgorithmGroup handle = NCryptProperties.ReadStringUnicode handle CngConstants.NCRYPT_ALGORITHM_GROUP_PROPERTY

    override this.CreateKey(keyName : string) (keySize : int) (algorithm : Algorithm) (overwrite : bool) (keyUsage : KeyUsage) =
        let mutable keyHandle = NCryptKeyOrCryptProviderSafeHandle.Null
        let flags = if overwrite then NCryptCreatePersistedKeyFlags.NCRYPT_OVERWRITE_KEY_FLAG else NCryptCreatePersistedKeyFlags.NONE
        let result = ExternInterop.NCryptCreatePersistedKey(storageProvider.Handle, &keyHandle, algorithm.name, keyName, KeySpec.NONE, flags)
        if result <> ERROR_SUCCESS then
            invalidOp("Failed to generate a key.")
        if algorithm = Algorithm.RSA then
            NCryptProperties.WriteUInt32 keyHandle CngConstants.NCRYPT_LENGTH_PROPERTY (uint32(keySize))
        NCryptProperties.WriteEnum keyHandle CngConstants.NCRYPT_EXPORT_POLICY_PROPERTY CngExportPolicy.NCRYPT_ALLOW_EXPORT_FLAG
        let finalizeResult = ExternInterop.NCryptFinalizeKey(keyHandle, 0u)
        if finalizeResult <> ERROR_SUCCESS then
            invalidOp("Failed to finalize key.")
        (keyHandle, KeySpec.NONE)

    override this.OpenExisting(keyName : string) =
        let mutable keyHandle = NCryptKeyOrCryptProviderSafeHandle.Null
        let openResult = ExternInterop.NCryptOpenKey(storageProvider.Handle, &keyHandle, keyName, KeySpec.NONE, 0u)
        match openResult with
        | v when v = ERROR_SUCCESS -> Some(keyHandle, KeySpec.NONE)
        | v when v = NTE_BAD_KEYSET -> None
        | _ -> invalidOp("Failed to open key.")

and CapiKeyProvider() =
    inherit KeyProviderBase()

    let providerName =
        if PlatformSupport.UseLegacyKeyStoreName then
            "Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype)"
        else
            "Microsoft Enhanced RSA and AES Cryptographic Provider"

    static let instance = CapiKeyProvider()
    static member Instance = instance
    override this.Name = "CAPI"
    override this.GetProviderName() = providerName
    override this.GetAlgorithmGroup(_) = AlgorithmGroup.RSA.name

    override this.CreateKey(keyName : string) (keySize : int) (algorithm : Algorithm) (overwrite : bool) (keyUsage : KeyUsage) =
        if algorithm <> Algorithm.RSA then
            invalidArg "algorithm" "CAPI does not support algorithms other than RSA."
        let mutable provider = NCryptKeyOrCryptProviderSafeHandle.Null
        let result = ExternInterop.CryptAcquireContext(&provider, keyName, providerName, ProviderType.PROV_RSA_AES, CryptAcquireContextFlags.CRYPT_NEWKEYSET)
        if not result then
            let lastError = Marshal.GetLastWin32Error()
            if lastError = ALREADY_EXISTS && overwrite then
                if not <| ExternInterop.CryptAcquireContext(&provider, keyName, providerName, ProviderType.PROV_RSA_AES, CryptAcquireContextFlags.CRYPT_DELETEKEYSET) then
                    invalidOp("Failed to delete existing key set.")
                if not <| ExternInterop.CryptAcquireContext(&provider, keyName, providerName, ProviderType.PROV_RSA_AES, CryptAcquireContextFlags.CRYPT_NEWKEYSET) then
                    invalidOp("Failed to create key set.")
            else
                invalidOp("Failed to create key set.")
        let flags = CryptGenKeyFlags.CRYPT_EXPORTABLE
        let keySizeFlags = (uint32(keySize) &&& 0xFFFFu) <<< 16
        let genKeyFlags = uint32(flags) ||| keySizeFlags
        let keySpec =
            match keyUsage with
            | KeyUsage.KeyExchange -> KeySpec.AT_KEYEXCHANGE
            | KeyUsage.Signature -> KeySpec.AT_SIGNATURE
            | _ -> invalidArg "keyUsage" "Invalid key usage."

        use mutable key = CryptKeySafeHandle.Null
        if not <| ExternInterop.CryptGenKey(provider, keySpec, genKeyFlags, &key) then
            invalidOp("Failed to generate key.")
        (provider, keySpec)

    override this.GetName handle =
        let PP_NAME = 0x6u
        let mutable size = 0u
        if not <| ExternInterop.CryptGetProvParam(handle, PP_NAME, 0n, &size, 0u) then
            invalidOp("Failed to get property.")
        use buffer = GlobalBufferSafeHandle.Allocate(int size)
        if not <| ExternInterop.CryptGetProvParam(handle, PP_NAME, buffer.DangerousGetHandle(), &size, 0u) then
            invalidOp("Failed to get property.")
        Marshal.PtrToStringAnsi(buffer.DangerousGetHandle())

    override this.OpenExisting(keyName : string) =
        let mutable provider = NCryptKeyOrCryptProviderSafeHandle.Null
        if not <| ExternInterop.CryptAcquireContext(&provider, keyName, providerName, ProviderType.PROV_RSA_AES, CryptAcquireContextFlags.NONE) then
            let result = Marshal.GetLastWin32Error()
            if result = DOES_NOT_EXIST then None
            else invalidOp "Failed to open the key."
        else
            let PP_KEYSPEC = 0x27u
            let mutable length = uint32 sizeof<uint32>
            use buffer = GlobalBufferSafeHandle.Allocate sizeof<uint32>
            if not <| ExternInterop.CryptGetProvParam(provider, PP_KEYSPEC, buffer.DangerousGetHandle(), &length, 0u) then
                invalidOp "Failed to get keySpec"
            else
                let keySpec = LanguagePrimitives.EnumOfValue<uint32, KeySpec>(uint32(Marshal.ReadInt32(buffer.DangerousGetHandle())))
                Some (provider, keySpec)