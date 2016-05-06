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

#How do I use it?

[Download the latest installer](https://github.com/vcsjones/FiddlerCertGen/releases/latest) for your version of Fiddler. Most people use Fiddler 4, so if you aren't sure, use that version of the installer. Once installed,
restart Fiddler.

You need to reset your root certificate for Fiddler after installing. To do that, go to "Tools", "Fiddler Options...". Then on the HTTPS tab, click "Actions" then "Reset All Certificates". This will remove the existing
Fiddler root certificate and install one from Fiddler Certificate Generator.