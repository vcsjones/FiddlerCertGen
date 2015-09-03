using System.Security.Cryptography.X509Certificates;
using VCSJones.FiddlerCertGen;

namespace ScratchGui
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = KeyProviders.CAPI;
            var algorithmRoot = Algorithm.RSA;
            var algorithm = Algorithm.RSA;
            using (PrivateKey rootKey = PrivateKey.CreateNew(provider, "SNARF", algorithmRoot, overwrite:true), eeKey = PrivateKey.CreateNew(provider, "SNARF2", algorithm, overwrite: true))
            {
                var generator = new CertificateGenerator();
                var root = generator.GenerateCertificateAuthority(rootKey, new X500DistinguishedName("CN=test"), HashAlgorithm.SHA256);
                X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Add(root);
                var generateCert = generator.GenerateCertificate(root, eeKey, new X500DistinguishedName("CN=EE"), new []{"test.com"});
                X509Certificate2UI.DisplayCertificate(generateCert);
            }
        }
    }
}
