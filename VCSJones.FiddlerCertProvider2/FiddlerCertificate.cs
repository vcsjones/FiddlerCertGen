using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using VCSJones.FiddlerCertProvider;

namespace VCSJones.FiddlerCertProvider2
{
    public class FiddlerCertificate : FiddlerCertificateBase
    {
        private readonly Dictionary<string, X509Certificate2> _certificateCache = new Dictionary<string, X509Certificate2>(StringComparer.InvariantCultureIgnoreCase);
        private readonly ReaderWriterLock _rwl = new ReaderWriterLock();
        private const int LOCK_TIMEOUT = 5000;

        public override X509Certificate2 GetCertificateForHostPlain(string sHostname)
        {
            _rwl.AcquireReaderLock(LOCK_TIMEOUT);
            try
            {
                var certExists = _certificateCache.ContainsKey(sHostname);
                if (certExists)
                {
                    return _certificateCache[sHostname];
                }
                else
                {
                    var signatureAlgorithm = CertificateConfiguration.EECertificateHashAlgorithm;
                    var cert = _generator.GenerateCertificate(GetRootCertificate(), EEPrivateKey, new X500DistinguishedName(FIDDLER_EE_DN), new[] { sHostname }, signatureAlgorithm: signatureAlgorithm);
                    var lockCookie = default(LockCookie);
                    try
                    {
                        lockCookie = _rwl.UpgradeToWriterLock(LOCK_TIMEOUT);
                        if (!_certificateCache.ContainsKey(sHostname))
                        {
                            _certificateCache.Add(sHostname, cert);
                        }
                        else
                        {
                            return _certificateCache[sHostname];
                        }
                    }
                    finally
                    {
                        _rwl.DowngradeFromWriterLock(ref lockCookie);
                    }
                    return cert;
                }

            }
            finally
            {
                _rwl.ReleaseReaderLock();
            }
        }

        public override X509Certificate2 GetCertificateForIPAddress(IPAddress address, string hostname)
        {
            var addressStr = address.ToString();
            _rwl.AcquireReaderLock(LOCK_TIMEOUT);
            try
            {
                var certExists = _certificateCache.ContainsKey(addressStr);
                if (certExists)
                {
                    return _certificateCache[addressStr];
                }
                else
                {
                    /*
                    Internet Explorer has weird handling of IPv6 addresses for certificates.
                    It appears to do a string match instead of a binary match on the address's octets.
                    It also seems to require the bracket notation in some circumstances when using dnsName.
                    */
                    var isIPv6 = address.AddressFamily == AddressFamily.InterNetworkV6;
                    var signatureAlgorithm = CertificateConfiguration.EECertificateHashAlgorithm;
                    var cert = _generator.GenerateCertificate(
                        issuingCertificate: GetRootCertificate(),
                        privateKey: EEPrivateKey,
                        dn: new X500DistinguishedName(FIDDLER_EE_DN),
                        dnsNames: isIPv6 ? new[] { addressStr, hostname } : new[] { addressStr },
                        signatureAlgorithm: signatureAlgorithm,
                        ipAddresses: new[] { address }
                    );
                    var lockCookie = default(LockCookie);
                    try
                    {
                        lockCookie = _rwl.UpgradeToWriterLock(LOCK_TIMEOUT);
                        if (!_certificateCache.ContainsKey(addressStr))
                        {
                            _certificateCache.Add(addressStr, cert);
                        }
                        else
                        {
                            return _certificateCache[addressStr];
                        }
                    }
                    finally
                    {
                        _rwl.DowngradeFromWriterLock(ref lockCookie);
                    }
                    return cert;
                }

            }
            finally
            {
                _rwl.ReleaseReaderLock();
            }
        }

        public override X509Certificate2 GetCertificateForHostWildCard(string sHostname)
        {
            string parentHostname;
            if (IsSubDomain(sHostname, out parentHostname))
            {
                sHostname = parentHostname;
            }
            _rwl.AcquireReaderLock(LOCK_TIMEOUT);
            try
            {
                var certExists = _certificateCache.ContainsKey(sHostname);
                if (certExists)
                {
                    return _certificateCache[sHostname];
                }
                else
                {
                    var signatureAlgorithm = CertificateConfiguration.EECertificateHashAlgorithm;
                    var cert = _generator.GenerateCertificate(GetRootCertificate(), EEPrivateKey, new X500DistinguishedName(FIDDLER_EE_DN), new[] { sHostname, "*." + sHostname }, signatureAlgorithm: signatureAlgorithm);
                    var lockCookie = default(LockCookie);
                    try
                    {
                        lockCookie = _rwl.UpgradeToWriterLock(LOCK_TIMEOUT);
                        if (!_certificateCache.ContainsKey(sHostname))
                        {
                            _certificateCache.Add(sHostname, cert);
                        }
                        else
                        {
                            return _certificateCache[sHostname];
                        }
                    }
                    finally
                    {
                        _rwl.DowngradeFromWriterLock(ref lockCookie);
                    }
                    return cert;
                }

            }
            finally
            {
                _rwl.ReleaseReaderLock();
            }
        }

        protected override void ClearCacheContainer()
        {
            _rwl.AcquireWriterLock(LOCK_TIMEOUT);
            _certificateCache.Clear();
            _rwl.ReleaseWriterLock();

        }

        public override bool CacheCertificateForHost(string sHost, X509Certificate2 oCert)
        {
            _rwl.AcquireWriterLock(LOCK_TIMEOUT);
            _certificateCache[sHost] = oCert;
            _rwl.ReleaseWriterLock();
            return true;
        }

        public override void ShowConfigurationUI(IntPtr hwndOwner)
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
