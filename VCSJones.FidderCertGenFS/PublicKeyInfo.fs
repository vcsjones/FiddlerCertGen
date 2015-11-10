namespace VCSJones.FiddlerCertGen
open System
open System.Runtime.InteropServices

type PublicKeyInfo(buffer : GlobalBufferSafeHandle) =
    let publicKey = Marshal.PtrToStructure(buffer.DangerousGetHandle(), typeof<CERT_PUBLIC_KEY_INFO>) :?> CERT_PUBLIC_KEY_INFO

    member internal this.PublicKey = publicKey

    member this.Key = 
        let key = Array.zeroCreate<byte> (int publicKey.PubicKey.cbData)
        Marshal.Copy(publicKey.PubicKey.pbData, key, 0, key.Length)
        key

    override this.Finalize() = 
        (this :> IDisposable).Dispose()

    interface IDisposable with
        member this.Dispose() =
            GC.SuppressFinalize(this)
            buffer.Dispose()