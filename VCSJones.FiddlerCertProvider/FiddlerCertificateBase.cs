using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Fiddler;
using VCSJones.FiddlerCertGen;

namespace VCSJones.FiddlerCertProvider
{
    public abstract class FiddlerCertificateBase : ICertificateProvider3, ICertificateProviderInfo
    {
        protected static readonly KeyProviderBase _keyProviderEngine;
        protected const string FIDDLER_ROOT_DN = "CN=DO_NOT_TRUST_FiddlerRoot, O=DO_NOT_TRUST, OU=Created by http://www.fiddler2.com";
        protected const string FIDDLER_EE_DN = "CN=DO_NOT_TRUST_Fiddler, O=DO_NOT_TRUST, OU=Created by http://www.fiddler2.com";
        protected const string FIDDLER_EE_PRIVATE_KEY_NAME = "FIDDLER_EE_KEY";
        protected const string FIDDLER_ROOT_PRIVATE_KEY_NAME = "FIDDLER_ROOT_KEY_2";
        protected readonly CertificateGenerator _generator = new CertificateGenerator();
        protected X509Certificate2 _root;
        protected PrivateKey _eePrivateKey;
        private static readonly object _privateKeyLock = new object();

        static FiddlerCertificateBase()
        {
            _keyProviderEngine = PlatformSupport.HasCngSupport ? KeyProviders.CNG : KeyProviders.CAPI;
        }

        protected PrivateKey EEPrivateKey
        {
            get
            {
                if (_eePrivateKey == null)
                {
                    lock (_privateKeyLock)
                    {
                        if (_eePrivateKey == null)
                        {
                            lock (typeof(CertificateConfiguration))
                            {
                                var algorithm = CertificateConfiguration.EECertificateAlgorithm;
                                var keySize = CertificateConfiguration.EERsaKeySize;
                                var keyName = $"{FIDDLER_EE_PRIVATE_KEY_NAME}_{algorithm}_{keySize}_{_keyProviderEngine.Name}_4";
                                var key = PrivateKey.OpenExisting(_keyProviderEngine, keyName) ?? PrivateKey.CreateNew(_keyProviderEngine, keyName, algorithm, KeyUsage.KeyExchange, keySize: keySize);
                                _eePrivateKey = key;
                            }
                        }
                    }
                }
                return _eePrivateKey;
            }
        }

        internal void ForceEECertificateClear()
        {
            _eePrivateKey = null;
        }


        public X509Certificate2 GetCertificateForHost(string sHostname)
        {
            IPAddress addr;
            if (IPAddress.TryParse(sHostname, out addr))
            {
                return GetCertificateForIPAddress(addr, sHostname);
            }
            var wildCard = FiddlerApplication.Prefs.GetBoolPref("fiddler.certmaker.UseWildcards", true);
            return wildCard ? GetCertificateForHostWildCard(sHostname) : GetCertificateForHostPlain(sHostname);
        }
        public List<X509Policy> GetEEPolicies()
        {
            var list = new List<X509Policy>();
            foreach (var model in PolicyConfiguration.Instance.GetAllPolicies())
            {
                list.Add(new X509Policy
                {
                    PolicyIdentifier = model.Oid
                });
            }
            return list;
        }

        public abstract X509Certificate2 GetCertificateForHostPlain(string sHostname);

        public abstract X509Certificate2 GetCertificateForIPAddress(IPAddress address, string hostname);

        public abstract X509Certificate2 GetCertificateForHostWildCard(string sHostname);

        public static bool IsSubDomain(string hostname, out string parentHostname)
        {
            var components = hostname.Split('.');
            if (components.Length < 3)
            {
                parentHostname = null;
                return false;
            }
            var tld = components[components.Length - 1];
            var domain = new string[components.Length - 2];
            Array.Copy(components, 1, domain, 0, components.Length - 2);
            if (Array.TrueForAll(domain, p => p.Length <= 2))
            {
                parentHostname = null;
                return false;
            }
            parentHostname = string.Join(".", domain) + "." + tld;
            return true;
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
                    if (certs.Count == 0)
                    {
                        if (!CreateRootCertificate())
                        {
                            throw new InvalidOperationException("Failed to generated a temporary root.");
                        }
                    }
                    else
                    {
                        _root = certs[0];
                    }
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
                lock (typeof(CertificateConfiguration))
                {
                    var algorithm = CertificateConfiguration.RootCertificateAlgorithm;
                    var signatureAlgorithm = CertificateConfiguration.RootCertificateHashAlgorithm;
                    var keySize = CertificateConfiguration.RootRsaKeySize;
                    var keyName = $"{FIDDLER_ROOT_PRIVATE_KEY_NAME}_{algorithm}_{keySize}_{_keyProviderEngine.Name};";
                    using (var key = PrivateKey.CreateNew(_keyProviderEngine, keyName, algorithm, KeyUsage.Signature, overwrite: true, keySize: keySize))
                    {
                        _root = _generator.GenerateCertificateAuthority(key, new X500DistinguishedName(FIDDLER_ROOT_DN), signatureAlgorithm);
                        return true;
                    }
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
            ClearCacheContainer();
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

        protected abstract void ClearCacheContainer();

        public abstract bool CacheCertificateForHost(string sHost, X509Certificate2 oCert);

        public string GetConfigurationString()
        {
            //This doesn't seem to be used by Fiddler. It uses its own implementation only in the 
            //ShowConfigurationUI implementation that we are going to re-implement anyway.
            return string.Empty;
        }

        public abstract void ShowConfigurationUI(IntPtr hwndOwner);
    }
}
