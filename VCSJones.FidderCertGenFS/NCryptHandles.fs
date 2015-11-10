namespace VCSJones.FiddlerCertGen
open Microsoft.Win32.SafeHandles
open InteropMemoryFree
[<AbstractClassAttribute>]
type public NCryptHandleBase =
    inherit SafeHandleZeroOrMinusOneIsInvalid

    new() = {inherit SafeHandleZeroOrMinusOneIsInvalid(true)}
    new(ownsHandle) = {inherit SafeHandleZeroOrMinusOneIsInvalid(ownsHandle)}

    override this.ReleaseHandle() = NCryptFreeObject(this.handle) = 0u


[<SealedAttribute>]
type public NCryptStorageProviderSafeHandle(owner) =
    inherit NCryptHandleBase(owner)
    new() = new NCryptStorageProviderSafeHandle(true)
    static member Null
        with get () =
            let result = new NCryptStorageProviderSafeHandle()
            result.SetHandle(0n)
            result

[<SealedAttribute>]
type public NCryptKeyOrCryptProviderSafeHandle(owner) =
    inherit NCryptHandleBase(owner)
    let mutable callerFree : bool = true
    new() = new NCryptKeyOrCryptProviderSafeHandle(true)

    member this.IsNCryptKey = PlatformSupport.HasCngSupport && NCryptIsKeyHandle(this.handle)
    member this.SetCallerFree(free) = callerFree <- free

    static member Null
        with get () =
            let result = new NCryptKeyOrCryptProviderSafeHandle()
            result.SetHandle(0n)
            result

    override this.ReleaseHandle() = 
        match (callerFree, this.IsNCryptKey) with
        | (false, _) -> true
        | (true, true) -> NCryptFreeObject(this.handle) = ERROR_SUCCESS
        | (true, false) -> CryptReleaseContext(this.handle, 0u)

