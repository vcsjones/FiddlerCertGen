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

`VCSJones.FiddlerCertProvider4` and `VCSJones.FidderCertProvider2` use the certificate generation library and actually implements the Fiddler interface for generating certificates. This project targets the .NET 4.0 build of Fiddler. Use the one appropriate for your version of Fiddler.

#How do I use it?
Take the output assemblies `VCSJones.FidderCertGen` and one of the `CertProvider` assemblies and place it in `%HOMEPATH%\Documents\Fidder2\Scripts2` and restart Fiddler.

The certificate generation can be configured by navigating to Tools, Fiddler Options, HTTPS, then clicking the blue link to configure the certificate generator.

Changing the End Entity certificate configures has an immediate result, however and domains where a certificate was previously generated will use the cached certificates. Restarting Fiddler clears the certificate cache.

Changing the Root Certificate configuration requires disabling decryption of HTTPS traffic, selecting "Remove Interception Certificates" and re-enabling decryption of HTTPS traffic.

For information about each configuration option, use the built-in help.