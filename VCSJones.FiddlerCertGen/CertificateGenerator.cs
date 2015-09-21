using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    public class CertificateGenerator
    {

        public static PrivateKey ExtractKey(X509Certificate2 cert)
        {
            if (!cert.HasPrivateKey)
            {
                throw new ArgumentException("Certificate does not contain a private key.", nameof(cert));
            }
            NCryptKeyOrCryptProviderSafeHandle handle;
            KeySpec keySpec;
            bool callerFree;
            if (!Crypt32.CryptAcquireCertificatePrivateKey(cert.Handle, AcquirePrivateKeyFlags.CRYPT_ACQUIRE_ALLOW_NCRYPT_KEY_FLAG, IntPtr.Zero, out handle, out keySpec, out callerFree))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return new PrivateKey(handle, keySpec == KeySpec.NCRYPT ? KeyProviders.CNG : KeyProviders.CAPI, keySpec);
        }

        private static byte[] SerializeCertificate(IntPtr pData, uint cbData)
        {
            var bytes = new byte[cbData];
            Marshal.Copy(pData, bytes, 0, bytes.Length);
            return bytes;
        }

        private static string HashAlgorithmToSignatureAlgorithm(PrivateKey privateKey, HashAlgorithm hashAlgorithm)
        {
            switch (hashAlgorithm)
            {
                case HashAlgorithm.SHA1:
                    if (privateKey.AlgorithmGroup == AlgorithmGroup.RSA) return OIDs.SHA1rsa;
                    if (privateKey.AlgorithmGroup == AlgorithmGroup.ECDSA) return OIDs.SHA1ecdsa;
                    goto default;
                case HashAlgorithm.SHA256:
                    if (privateKey.AlgorithmGroup == AlgorithmGroup.RSA) return OIDs.SHA256rsa;
                    if (privateKey.AlgorithmGroup == AlgorithmGroup.ECDSA) return OIDs.SHA256ecdsa;
                    goto default;
                case HashAlgorithm.SHA384:
                    if (privateKey.AlgorithmGroup == AlgorithmGroup.RSA) return OIDs.SHA384rsa;
                    if (privateKey.AlgorithmGroup == AlgorithmGroup.ECDSA) return OIDs.SHA384ecdsa;
                    goto default;
                default:
                    throw new NotSupportedException();
            }
        }

        private static List<X509AlternativeName> DnsAltNamesFromArray(string[] dnsNames, string[] ipAddresses)
        {
            var list = new List<X509AlternativeName>();
            foreach (var dnsName in dnsNames)
            {
                list.Add(new X509AlternativeName { Type = X509AlternativeNameType.DnsName, Value = dnsName });
            }
            foreach (var ipAddress in ipAddresses)
            {
                list.Add(new X509AlternativeName { Type = X509AlternativeNameType.IpAddress, Value = ipAddress });
            }
            return list;
        }

        public unsafe X509Certificate2 GenerateCertificate(X509Certificate2 issuingCertificate, PrivateKey privateKey, X500DistinguishedName dn, string[] dnsNames, string[] ipAddresses = null, DateTime? notBefore = null, DateTime? notAfter = null)
        {
            if (!issuingCertificate.HasPrivateKey)
            {
                throw new ArgumentException("Issuing certificate must have a private key.", nameof(issuingCertificate));
            }
            IntPtr basicEncodedDataPtr = IntPtr.Zero, certExtensionPtr = IntPtr.Zero;
            var serialNumber = new byte[16];
            var rng = RandomNumberGenerator.Create();
            rng.GetNonZeroBytes(serialNumber);
            fixed (byte* dnPtr = dn.RawData, issuerDnPtr = issuingCertificate.SubjectName.RawData, serialNumberPtr = serialNumber)
            {
                try
                {
                    var blob = new NATIVE_CRYPTOAPI_BLOB
                    {
                        cbData = (uint)dn.RawData.Length,
                        pbData = dnPtr
                    };
                    var signatureAlgorithm = new CRYPT_ALGORITHM_IDENTIFIER
                    {
                        pszObjId = issuingCertificate.SignatureAlgorithm.Value
                    };
                    using (var signingKey = ExtractKey(issuingCertificate))
                    {
                        using (PublicKeyInfo publicKey = privateKey.ToPublicKey(), signingPublicKey = signingKey.ToPublicKey())
                        {
                            using (var extensions = new MarshalX509ExtensionCollection())
                            {
                                using (extensions.Freeze())
                                {
                                    var usage = X509KeyUsageFlags.DigitalSignature;
                                    if (privateKey.AlgorithmGroup == AlgorithmGroup.RSA)
                                    {
                                        //Key encipherment is not valid for DSA/ECDSA
                                        usage |= X509KeyUsageFlags.KeyEncipherment;
                                    }
                                    extensions.Add(new X509BasicConstraintsExtension(false, false, 0, true));
                                    extensions.Add(new X509KeyUsageExtension(usage, true));
                                    extensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection {new Oid(OIDs.EKU_SERVER)}, false));
                                    extensions.Add(new X509SubjectAlternativeNameExtension(DnsAltNamesFromArray(dnsNames, ipAddresses ?? new string[0]), false));
                                    using (var sha1 = new SHA1CryptoServiceProvider())
                                    {
                                        var issuingKeyId = sha1.ComputeHash(signingPublicKey.Key);
                                        extensions.Add(new X509SubjectKeyIdentifierExtension(sha1.ComputeHash(publicKey.Key), false));
                                        extensions.Add(new X509AuthorityKeyIdentifierExtension(issuingKeyId, null));
                                    }
                                }
                                var certInfo = new CERT_INFO();
                                certInfo.Subject = blob;
                                certInfo.SerialNumber = new NATIVE_CRYPTOAPI_BLOB {cbData = (uint) serialNumber.Length, pbData = serialNumberPtr};
                                certInfo.SubjectPublicKeyInfo = publicKey.PublicKey;
                                certInfo.dwVersion = CertificateVersion.CERT_V3;
                                certInfo.Issuer = new NATIVE_CRYPTOAPI_BLOB {cbData = (uint) issuingCertificate.SubjectName.RawData.Length, pbData = issuerDnPtr};
                                certInfo.SignatureAlgorithm = signatureAlgorithm;
                                certInfo.NotAfter = FileTimeHelper.ToFileTimeStructureUtc(notAfter ?? DateTime.Now.AddHours(-1).AddYears(5));
                                certInfo.NotBefore = FileTimeHelper.ToFileTimeStructureUtc(notBefore ?? DateTime.Now.AddHours(-1));
                                certInfo.cExtension = extensions.Extensions.cExtension;
                                certInfo.rgExtension = extensions.Extensions.rgExtension;
                                var size = 0u;
                                var CERT_INFO_TYPE = (IntPtr) 2;
                                if (!Crypt32.CryptSignAndEncodeCertificate(signingKey.Handle, signingKey.KeySpec, EncodingType.X509_ASN_ENCODING, CERT_INFO_TYPE, ref certInfo, ref signatureAlgorithm, IntPtr.Zero, IntPtr.Zero, ref size))
                                {
                                    throw new Win32Exception(Marshal.GetLastWin32Error());
                                }
                                var buffer = Marshal.AllocHGlobal((int) size);
                                if (!Crypt32.CryptSignAndEncodeCertificate(signingKey.Handle, signingKey.KeySpec, EncodingType.X509_ASN_ENCODING, CERT_INFO_TYPE, ref certInfo, ref signatureAlgorithm, IntPtr.Zero, buffer, ref size))
                                {
                                    throw new Win32Exception(Marshal.GetLastWin32Error());
                                }
                                const int CERT_KEY_PROV_INFO_PROP_ID = 2;
                                var certificate = new X509Certificate2(SerializeCertificate(buffer, size));
                                var keyProvInfo = new CRYPT_KEY_PROV_INFO
                                {
                                    cProvParam = 0,
                                    dwKeySpec = privateKey.KeySpec,
                                    dwProvType = privateKey.Handle.IsNCryptKey ? ProviderType.CNG : ProviderType.PROV_RSA_AES,
                                    pwszProvName = privateKey.ProviderName,
                                    dwFlags = 0,
                                    pwszContainerName = privateKey.Name
                                };
                                if (!Crypt32.CertSetCertificateContextProperty(certificate.Handle, CERT_KEY_PROV_INFO_PROP_ID, 0u, ref keyProvInfo))
                                {
                                    throw new Win32Exception(Marshal.GetLastWin32Error());
                                }
                                return new X509Certificate2(certificate);
                            }
                        }
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(basicEncodedDataPtr);
                    Marshal.FreeHGlobal(certExtensionPtr);
                }
            }
        }

        public unsafe X509Certificate2 GenerateCertificateAuthority(PrivateKey privateKey, X500DistinguishedName dn, HashAlgorithm signatureAlgorithm, DateTime? notBefore = null, DateTime? notAfter = null)
        {
            {
                fixed (byte* dnPtr = dn.RawData)
                {
                    var blob = new NATIVE_CRYPTOAPI_BLOB
                    {
                        cbData = (uint)dn.RawData.Length,
                        pbData = dnPtr
                    };
                    var signatureAlgorithmIdentifier = new CRYPT_ALGORITHM_IDENTIFIER
                    {
                        pszObjId = HashAlgorithmToSignatureAlgorithm(privateKey, signatureAlgorithm)
                    };
                    using (var extensions = new MarshalX509ExtensionCollection())
                    {
                        using (extensions.Freeze())
                        {
                            extensions.Add(new X509BasicConstraintsExtension(true, true, 1, true));
                            extensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.CrlSign | X509KeyUsageFlags.KeyCertSign, true));
                            extensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid(OIDs.EKU_SERVER) }, false));
                            using (var publicKey = privateKey.ToPublicKey())
                            {
                                using (var sha1 = new SHA1CryptoServiceProvider())
                                {
                                    var pubKeyHash = sha1.ComputeHash(publicKey.Key);
                                    extensions.Add(new X509SubjectKeyIdentifierExtension(pubKeyHash, false));
                                    extensions.Add(new X509AuthorityKeyIdentifierExtension(pubKeyHash, null));
                                }
                            }
                        }
                        var certExtensions = extensions.Extensions;
                        var keyProvInfo = new CRYPT_KEY_PROV_INFO
                        {
                            cProvParam = 0,
                            dwKeySpec = privateKey.KeySpec,
                            dwProvType = privateKey.Handle.IsNCryptKey ? ProviderType.CNG : ProviderType.PROV_RSA_AES,
                            pwszProvName = privateKey.ProviderName,
                            dwFlags = 0,
                            pwszContainerName = privateKey.Name
                        };
                        var beginning = new SYSTEMTIME(notBefore ?? DateTime.UtcNow.AddHours(-1));
                        var expiration = new SYSTEMTIME(notAfter ?? DateTime.UtcNow.AddHours(-1).AddYears(30));
                        var certContext = Crypt32.CertCreateSelfSignCertificate(privateKey.Handle, ref blob, SelfSignFlags.NONE, ref keyProvInfo, ref signatureAlgorithmIdentifier, beginning, expiration, ref certExtensions);
                        if (certContext == IntPtr.Zero)
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                        return new X509Certificate2(certContext);
                    }
                }
            }
        }
    }
}
