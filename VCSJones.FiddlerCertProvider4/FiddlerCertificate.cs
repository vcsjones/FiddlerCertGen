using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using VCSJones.FiddlerCertProvider;

namespace VCSJones.FiddlerCertProvider4
{
    public class FiddlerCertificate : FiddlerCertificateBase
    {
        private readonly ConcurrentDictionary<string, X509Certificate2> _certificateCache = new ConcurrentDictionary<string, X509Certificate2>(StringComparer.InvariantCultureIgnoreCase);

        public override X509Certificate2 GetCertificateForHostPlain(string sHostname)
        {
            return _certificateCache.GetOrAdd(sHostname, hostname =>
            {
                var signatureAlgorithm = CertificateConfiguration.EECertificateHashAlgorithm;
                return _generator.GenerateCertificate(GetRootCertificate(), EEPrivateKey, new X500DistinguishedName(FIDDLER_EE_DN), new[] { hostname }, signatureAlgorithm: signatureAlgorithm, policies: GetEEPolicies());
            });
        }

        public override X509Certificate2 GetCertificateForIPAddress(IPAddress address, string hostname)
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
                    ipAddresses: new[] {address},
                    policies: GetEEPolicies()
                );
            });
        }

        public override X509Certificate2 GetCertificateForHostWildCard(string sHostname)
        {
            string parentHostname;
            bool isSubDomain = IsSubDomain(sHostname, out parentHostname);
            return _certificateCache.GetOrAdd(isSubDomain ? parentHostname : sHostname, hostname =>
            {
                var signatureAlgorithm = CertificateConfiguration.EECertificateHashAlgorithm;
                return _generator.GenerateCertificate(GetRootCertificate(), EEPrivateKey, new X500DistinguishedName(FIDDLER_EE_DN), new[] { hostname, "*." + hostname }, signatureAlgorithm: signatureAlgorithm, policies: GetEEPolicies());
            });
        }

        protected override void ClearCacheContainer()
        {
            _certificateCache.Clear();
        }

        public override bool CacheCertificateForHost(string sHost, X509Certificate2 oCert)
        {
            _certificateCache.AddOrUpdate(sHost, oCert, delegate { return oCert; });
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
