namespace VCSJones.FiddlerCertGen

[<SealedAttribute>]
type NCryptStorageProvider =
    internal new : NCryptStorageProviderSafeHandle -> NCryptStorageProvider
    public new : string -> NCryptStorageProvider
    static member MicrosoftSoftwareKeyStorageProvider : NCryptStorageProvider
    static member MicrosoftSmartCardProvider : NCryptStorageProvider
    member internal Handle : NCryptStorageProviderSafeHandle
    member Name : string
    interface System.IDisposable