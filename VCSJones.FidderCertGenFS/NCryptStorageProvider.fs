namespace VCSJones.FiddlerCertGen

[<SealedAttribute>]
type NCryptStorageProvider(handle : NCryptStorageProviderSafeHandle) =
    static let softwareKeyStorageProvider = new NCryptStorageProvider("Microsoft Software Key Storage Provider")
    static let smartcardStorageProvider = new NCryptStorageProvider("Microsoft Smart Card Key Storage Provider")

    new(name : string) = 
        let mutable handle : NCryptStorageProviderSafeHandle = NCryptStorageProviderSafeHandle.Null
        let result = ExternInterop.NCryptOpenStorageProvider(&handle, name, 0u)
        if result <> 0u then
            invalidOp("Failed to open storage provider.")
        new NCryptStorageProvider(handle)

    static member MicrosoftSoftwareKeyStorageProvider = softwareKeyStorageProvider
    static member MicrosoftSmartCardProvider = smartcardStorageProvider

    member this.Handle = handle
    member this.Name = NCryptProperties.ReadStringUnicode handle CngConstants.NCRYPT_NAME_PROPERTY
        

    interface System.IDisposable with
        member this.Dispose() = handle.Dispose()