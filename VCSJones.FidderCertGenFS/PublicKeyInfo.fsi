namespace VCSJones.FiddlerCertGen
open System
open System.Runtime.InteropServices

type PublicKeyInfo =
    internal new : GlobalBufferSafeHandle -> PublicKeyInfo
    member internal PublicKey : CERT_PUBLIC_KEY_INFO
    member Key  : byte array
    interface IDisposable