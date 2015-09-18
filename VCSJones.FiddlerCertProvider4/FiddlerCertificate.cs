using System;
using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using Fiddler;
using VCSJones.FiddlerCertGen;

namespace VCSJones.FiddlerCertProvider4
{
    public class FiddlerCertificate : ICertificateProvider3
    {
        private static readonly KeyProviderBase _keyProviderEngine;
        private static readonly Algorithm _algorithm;
        private static readonly HashAlgorithm _signatureAlgorithm;
        private const string FIDDLER_ROOT_DN = "CN=DO_NOT_TRUST_FiddlerRoot, O=DO_NOT_TRUST, OU=Created by http://www.fiddler2.com";
        private const string FIDDLER_EE_DN = "CN=DO_NOT_TRUST_Fiddler, O=DO_NOT_TRUST, OU=Created by http://www.fiddler2.com";
        private const string FIDDLER_EE_PRIVATE_KEY_NAME = "FIDDLER_EE_KEY";
        private const string FIDDLER_ROOT_PRIVATE_KEY_NAME = "FIDDLER_ROOT_KEY";
        private readonly ConcurrentDictionary<string, X509Certificate2> _certificateCache = new ConcurrentDictionary<string, X509Certificate2>(StringComparer.InvariantCultureIgnoreCase);
        private readonly CertificateGenerator _generator = new CertificateGenerator();
        private PrivateKey _eePrivateKey;
        private static readonly object _keyGenLock = new object();
        private X509Certificate2 _root;

        static FiddlerCertificate()
        {
            _algorithm = PlatformSupport.HasCngSupport ? Algorithm.ECDSA256 : Algorithm.RSA;
            _keyProviderEngine = PlatformSupport.HasCngSupport ? KeyProviders.CNG : KeyProviders.CAPI;
            _signatureAlgorithm = HashAlgorithm.SHA256;
        }


        private PrivateKey GetEEPrivateKey()
        {
            if (_eePrivateKey == null)
            {
                lock (_keyGenLock)
                {
                    if (_eePrivateKey == null)
                    {
                        var fiddlerEePrivateKeyName = $"{FIDDLER_EE_PRIVATE_KEY_NAME}_${_algorithm}_${_keyProviderEngine.Name}";
                        var key = PrivateKey.OpenExisting(_keyProviderEngine, fiddlerEePrivateKeyName);
                        if (key == null)
                        {
                            key = PrivateKey.CreateNew(_keyProviderEngine, fiddlerEePrivateKeyName, _algorithm);
                        }
                        _eePrivateKey = key;
                    }
                }
            }
            return _eePrivateKey;
        }

        public X509Certificate2 GetCertificateForHost(string sHostname)
        {
            return _certificateCache.GetOrAdd(sHostname, hostname =>
            {
                return _generator.GenerateCertificate(GetRootCertificate(), GetEEPrivateKey(), new X500DistinguishedName(FIDDLER_EE_DN), new[] { hostname });
            });
        }

        public X509Certificate2 GetRootCertificate()
        {
            if (_root == null)
            {

                var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                try
                {
                    store.Open(OpenFlags.ReadOnly);
                    var certs = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, FIDDLER_ROOT_DN, true);
                    _root = certs[0];
                }
                finally
                {
                    store.Close();
                }
            }
            return _root;
        }

        public bool CreateRootCertificate()
        {
            try
            {
                using (var key = PrivateKey.CreateNew(_keyProviderEngine, FIDDLER_ROOT_PRIVATE_KEY_NAME, _algorithm, overwrite:true))
                {
                    _root = _generator.GenerateCertificateAuthority(key, new X500DistinguishedName(FIDDLER_ROOT_DN), _signatureAlgorithm);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool TrustRootCertificate()
        {
            var root = _root;
            if (root == null)
            {
                return false;
            }
            try
            {
                var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                try
                {
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(root);
                }
                finally
                {
                    store.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ClearCertificateCache()
        {
            return ClearCertificateCache(true);
        }

        public bool rootCertIsTrusted(out bool bUserTrusted, out bool bMachineTrusted)
        {
            var userStore = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            var machineStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            try
            {
                userStore.Open(OpenFlags.ReadOnly);
                machineStore.Open(OpenFlags.ReadOnly);
                bUserTrusted = userStore.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, FIDDLER_ROOT_DN, true).Count > 0;
                bMachineTrusted = machineStore.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, FIDDLER_ROOT_DN, true).Count > 0;
                return bUserTrusted || bMachineTrusted;
            }
            catch
            {
                bUserTrusted = false;
                bMachineTrusted = false;
                return false;
            }
            finally
            {
                userStore.Close();
                machineStore.Close();
            }
        }

        public bool ClearCertificateCache(bool bClearRoot)
        {
            _certificateCache.Clear();
            if (bClearRoot)
            {
                _root = null;
                var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                try
                {
                    store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
                    var roots = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, FIDDLER_ROOT_DN, true);
                    store.RemoveRange(roots);
                }
                catch
                {
                    return false;
                }
                finally
                {
                    store.Close();
                }
            }
            return true;
        }

        public bool CacheCertificateForHost(string sHost, X509Certificate2 oCert)
        {
            _certificateCache.AddOrUpdate(sHost, oCert, delegate { return oCert; });
            return true;
        }
    }
}
