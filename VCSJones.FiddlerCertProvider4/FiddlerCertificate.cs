using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Fiddler;
using VCSJones.FiddlerCertGen;
using System.Windows.Forms;

namespace VCSJones.FiddlerCertProvider4
{
    public class FiddlerCertificate : ICertificateProvider3, ICertificateProviderInfo
    {
        private static readonly KeyProviderBase _keyProviderEngine;
        private const string FIDDLER_ROOT_DN = "CN=DO_NOT_TRUST_FiddlerRoot, O=DO_NOT_TRUST, OU=Created by http://www.fiddler2.com";
        private const string FIDDLER_EE_DN = "CN=DO_NOT_TRUST_Fiddler, O=DO_NOT_TRUST, OU=Created by http://www.fiddler2.com";
        private const string FIDDLER_EE_PRIVATE_KEY_NAME = "FIDDLER_EE_KEY";
        private const string FIDDLER_ROOT_PRIVATE_KEY_NAME = "FIDDLER_ROOT_KEY_2";
        private readonly ConcurrentDictionary<string, X509Certificate2> _certificateCache = new ConcurrentDictionary<string, X509Certificate2>(StringComparer.InvariantCultureIgnoreCase);
        private readonly CertificateGenerator _generator = new CertificateGenerator();
        private X509Certificate2 _root;
        private PrivateKey _eePrivateKey;
        private static readonly object _privateKeyLock = new object();

        static FiddlerCertificate()
        {
            _keyProviderEngine = PlatformSupport.HasCngSupport ? KeyProviders.CNG : KeyProviders.CAPI;
        }

        private PrivateKey EEPrivateKey
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

        public X509Certificate2 GetCertificateForHostPlain(string sHostname)
        {
            return _certificateCache.GetOrAdd(sHostname, hostname =>
            {
                var signatureAlgorithm = CertificateConfiguration.EECertificateHashAlgorithm;
                return _generator.GenerateCertificate(GetRootCertificate(), EEPrivateKey, new X500DistinguishedName(FIDDLER_EE_DN), new[] { hostname }, signatureAlgorithm: signatureAlgorithm);
            });
        }

        public X509Certificate2 GetCertificateForIPAddress(IPAddress address, string hostname)
        {
            return _certificateCache.GetOrAdd(address.ToString(), addressStr =>
            {
                /*
                Internet Explorer has weird handling of IPv6 addresses for certificates.
                It appears to do a string match instead of a binary match on the address's octets.
                It also seems to require the bracket notation in some circumstances when using dnsName.
                */
                var isIPv6 = address.AddressFamily == AddressFamily.InterNetworkV6;
                var signatureAlgorithm = CertificateConfiguration.EECertificateHashAlgorithm;
                return _generator.GenerateCertificate(
                    issuingCertificate: GetRootCertificate(),
                    privateKey: EEPrivateKey,
                    dn: new X500DistinguishedName(FIDDLER_EE_DN),
                    dnsNames : isIPv6 ? new [] {addressStr, hostname} : new[] {addressStr},
                    signatureAlgorithm: signatureAlgorithm,
                    ipAddresses: new[] {address}
                );
            });
        }

        public X509Certificate2 GetCertificateForHostWildCard(string sHostname)
        {
            string parentHostname;
            bool isSubDomain = IsSubDomain(sHostname, out parentHostname);
            return _certificateCache.GetOrAdd(isSubDomain ? parentHostname : sHostname, hostname =>
            {
                var signatureAlgorithm = CertificateConfiguration.EECertificateHashAlgorithm;
                return _generator.GenerateCertificate(GetRootCertificate(), EEPrivateKey, new X500DistinguishedName(FIDDLER_EE_DN), new[] { hostname, "*." + hostname }, signatureAlgorithm: signatureAlgorithm);
            });
        }

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
                lock(typeof(CertificateConfiguration))
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

        public string GetConfigurationString()
        {
            //This doesn't seem to be used by Fiddler. It uses its own implementation only in the 
            //ShowConfigurationUI implementation that we are going to re-implement anyway.
            return string.Empty;
        }

        public void ShowConfigurationUI(IntPtr hwndOwner)
        {
            var owner = NativeWindow.FromHandle(hwndOwner);
            using (var dialog = new ConfigurationDialog(this))
            {
                dialog.Owner = Control.FromHandle(hwndOwner) as Form;
                dialog.ShowDialog(owner);
            }
        }
    }
}
