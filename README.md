Fiddler Certificate Generator
=========

#What is it?
Fiddler is web debugging proxy developed by Eric Lawrence and Telerik. Fiddler supports HTTPS interception so that it can inspect and modify HTTPS traffic.

This project is an alternative certificate generation library. Fiddler must generate an HTTPS certificate for every site that it intercepts. This one provides some functionality that none of the built-in ones do.

1. Support for ECDSA certificates.
1. Support for Windows XP / Windows Server 2003 with RSA keys greater than 1024.
1. End-Entity certificates do not need to be stored in the My certificate store, only the root must be installed in the Root store.
1. Support for RSA CNG in addition to ECDSA CNG.

#What are the defaults?
The signature algorithm default is SHA256, SHA384 and SHA1 are also supported. SHA1 should not be used since browsers are starting to sunset it, but is left there for users of Windows XP that aren't using Service Pack 3.

The algorithm depends on the operating system. ECDSA will be used for Windows Vista or greater, RSA will be used for Windows XP.


#How does it work?
There are two libraries, the VCSJones.FiddlerCertGen project, which is a .NET 2.0 project. This does the bulk of the certificate generation but knows nothing about Fiddler, nor does it have to be used in Fiddler. It can be used as a general purpose test certificate library.

`VCSJones.FiddlerCertProvider4` uses the certificate generation library and actually implements the Fiddler interface for generating certificates. This project targets the .NET 4.0 build of Fiddler, but should be source-compatible with the .NET Framework 2.0 and .NET 2.0 Fiddler, the reference to Fiddler needs to be updated to a Fiddler .NET 2 path.

#How do I use it?
The easiest way is to take the output of both projects and put them beside the `Fiddler.exe` executable, such as `C:\Program Files (x86)\Fiddler2`. The assemblies are called `CertMaker.dll` and `VCSJones.FiddlerCertGen.dll`.
Inclusing the PDBs isn't strictly required, but makes debugging possible.

#How do key sizes work?
For RSA it's fairly simple, you specify you want to use the RSA algorithm and use the `keySize` parameter when creating a private key. If it isn't specifed, 2048 is the default. Practically most browsers expect to receive a key size of 1024, 2048, or 4096.

For ECDSA each key size, 256, 384, and 521 are their own algorithm. This is how the CNG layer in Windows works. The `keySize` parameter is ignored when using ECDSA and is instead determined by the algorithm used.

Not all browsers support ECDSA, but the more modern ones do. SSLLabs has a good breakdown of which browsers don't. In practice, a key will always be 256-bit for an end-entity certificate, in fact Safari on OS X or iOS does not support 384-bit end-entity certificates. It does support 384-bit roots and intermediates. 521-bit certificates are never encountered in the wild.
