using VCSJones.FiddlerCertGen;

namespace VCSJones.FiddlerCertProvider4
{
    public static class CertificateConfiguration
    {
        public static string RootCertificateAlgorithm
        {
            get
            {
                var defaultAlgorithm = PlatformSupport.HasCngSupport ? Algorithm.ECDSA_P256 : Algorithm.RSA;
                return Fiddler.FiddlerApplication.Prefs.GetStringPref("fiddler.cng.root-algorithm", defaultAlgorithm.ToString());
            }
            set
            {

            }
        }
    }
}
