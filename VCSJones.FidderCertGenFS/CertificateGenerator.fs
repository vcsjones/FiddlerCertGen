namespace VCSJones.FiddlerCertGen
open System
open System.Net
open System.Runtime.InteropServices
open System.Security.Cryptography
open System.Security.Cryptography.X509Certificates

type MarshalX509Extension(extension : X509Extension) as self =
    let blobPtr = Marshal.AllocHGlobal(extension.RawData.Length)
    do
        Marshal.Copy(extension.RawData, 0, blobPtr, extension.RawData.Length)
        let blob = CRYPT_OBJID_BLOB(cbData = uint32(extension.RawData.Length), pbData = blobPtr)
        let extension = CERT_EXTENSION(fCritical = extension.Critical, pszObjId = extension.Oid.Value, Value = blob)
        self.value <- extension

    member val private disposed = false with get, set
    member val private value = Unchecked.defaultof<CERT_EXTENSION> with get, set

    member internal this.Value
        with get() =
            if this.disposed then raise(ObjectDisposedException("blob"))
            this.value;
    member this.Extension = extension
    override this.Finalize() =  
        (this :> IDisposable).Dispose()
    interface IDisposable with
        member this.Dispose() =
            GC.SuppressFinalize(this)
            this.disposed <- true
            Marshal.FreeHGlobal(blobPtr)


type Freezer(collection : MarshalX509ExtensionCollection) = 
        do
            collection.frozen <- true
        interface System.IDisposable with
            member this.Dispose() =
                collection.frozen <- false
                collection.RebuildExtensions()
and MarshalX509ExtensionCollection() =
    member val internal frozen = false with get, set
    member this.Freeze() = new Freezer(this)
    member internal this.RebuildExtensions() = ()
    member this.Add(extension : X509Extension) = ()

type CertificateGenerator() =
    let hashAlgorithmToSignatureAlgorithm (hashAlgorithm : HashingAlgorithm) (algorithmGroup : AlgorithmGroup) =
        let toOID = function
        | (HashingAlgorithm.MD2, "RSA") -> OIDs.MD2rsa
        | (HashingAlgorithm.MD5, "RSA") -> OIDs.MD5rsa
        | (HashingAlgorithm.SHA1, "RSA") -> OIDs.SHA1rsa
        | (HashingAlgorithm.SHA256, "RSA") -> OIDs.SHA256rsa
        | (HashingAlgorithm.SHA384, "RSA") -> OIDs.SHA384rsa
        | (HashingAlgorithm.SHA1, "ECDSA") -> OIDs.SHA1ecdsa
        | (HashingAlgorithm.SHA256, "ECDSA") -> OIDs.SHA256ecdsa
        | (HashingAlgorithm.SHA384, "ECDSA") -> OIDs.SHA384ecdsa
        | _ -> raise(System.NotSupportedException())
        toOID (hashAlgorithm, algorithmGroup.name)

    let altNamesFromArrays (dnsNames : string list) (ips : IPAddress list) =
        dnsNames
        |> List.map(fun dns -> DNSName(dns))
        |> List.append(ips |> List.map(fun ip -> Address(ip)))

    member this.GenerateCertificateAuthority (privateKey : PrivateKey) (dn : X500DistinguishedName) (hashAlgorithm : HashingAlgorithm) (notBefore : DateTime option) (notAfter : DateTime option) =
        let dnFixed = GCHandle.Alloc(dn.RawData)
        let blob = CRYPTOAPI_BLOB(uint32(dn.RawData.Length), dnFixed.AddrOfPinnedObject())
        let signatureAlgorithm = CRYPT_ALGORITHM_IDENTIFIER(pszObjId = hashAlgorithmToSignatureAlgorithm hashAlgorithm privateKey.AlgorithmGroup)

        ()