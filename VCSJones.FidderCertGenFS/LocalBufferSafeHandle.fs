namespace VCSJones.FiddlerCertGen
open Microsoft.Win32.SafeHandles
open InteropMemoryFree

[<SealedAttribute>]
type internal LocalBufferSafeHandle =
    inherit SafeHandleZeroOrMinusOneIsInvalid

    new() = {inherit SafeHandleZeroOrMinusOneIsInvalid(true)}
    new(ownsHandle) = {inherit SafeHandleZeroOrMinusOneIsInvalid(ownsHandle)}

    static member Null
        with get () =
            let result = new LocalBufferSafeHandle()
            result.SetHandle(0n)
            result

    override this.ReleaseHandle() =
        LocalFree(this.handle) = 0n


[<SealedAttribute>]
type internal GlobalBufferSafeHandle =
    inherit SafeHandleZeroOrMinusOneIsInvalid

    new() = {inherit SafeHandleZeroOrMinusOneIsInvalid(true)}
    new(ownsHandle) = {inherit SafeHandleZeroOrMinusOneIsInvalid(ownsHandle)}

    static member Null
        with get () =
            let result = new GlobalBufferSafeHandle()
            result.SetHandle(0n)
            result

    static member FromHandle(handle : nativeint) =
        let result = new GlobalBufferSafeHandle()
        result.SetHandle(handle)
        result

    override this.ReleaseHandle() =
        System.Runtime.InteropServices.Marshal.FreeHGlobal(this.handle)
        true

