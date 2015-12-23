using VCSJones.FiddlerCertGen;

namespace VCSJones.FiddlerCertProvider
{
    public static class CertificateConfiguration
    {
        private const string RootCertificateHashAlgorithmPreferenceKey = "fiddler.cng.root-hash-algorithm";
        private const string RootCertificateAlgorithmPreferenceKey = "fiddler.cng.root-algorithm";
        private const string RootCertificateRsaKeySizePreferenceKey = "fiddler.cng.root-rsa-key-size";
        private const string EECertificateHashAlgorithmPreferenceKey = "fiddler.cng.ee-hash-algorithm";
        private const string EECertificateAlgorithmPreferenceKey = "fiddler.cng.ee-algorithm";
        private const string EECertificateRsaKeySizePreferenceKey = "fiddler.cng.ee-rsa-key-size";

        public static Algorithm RootCertificateAlgorithm
        {
            get
            {
                var defaultAlgorithm = PlatformSupport.HasCngSupport ? Algorithm.ECDSA_P384 : Algorithm.RSA;
                Algorithm result;
                return Algorithm.TryParse(Fiddler.FiddlerApplication.Prefs.GetStringPref(RootCertificateAlgorithmPreferenceKey, defaultAlgorithm.ToString()), out result) ? result : defaultAlgorithm;
            }
            set
            {
                Fiddler.FiddlerApplication.Prefs.SetStringPref(RootCertificateAlgorithmPreferenceKey, value.ToString());
            }
        }

        public static HashAlgorithm RootCertificateHashAlgorithm
        {
            get
            {
                var defaultAlgorithm = PlatformSupport.HasCngSupport ? HashAlgorithm.SHA384 : HashAlgorithm.SHA1;
                return (HashAlgorithm)Fiddler.FiddlerApplication.Prefs.GetInt32Pref(RootCertificateHashAlgorithmPreferenceKey, (int)defaultAlgorithm);
            }
            set
            {
                Fiddler.FiddlerApplication.Prefs.SetInt32Pref(RootCertificateHashAlgorithmPreferenceKey, (int)value);
            }
        }

        public static HashAlgorithm EECertificateHashAlgorithm
        {
            get
            {
                var defaultAlgorithm = PlatformSupport.HasCngSupport ? HashAlgorithm.SHA256 : HashAlgorithm.SHA1;
                return (HashAlgorithm)Fiddler.FiddlerApplication.Prefs.GetInt32Pref(EECertificateHashAlgorithmPreferenceKey, (int)defaultAlgorithm);
            }
            set
            {
                Fiddler.FiddlerApplication.Prefs.SetInt32Pref(EECertificateHashAlgorithmPreferenceKey, (int)value);
            }
        }

        public static Algorithm EECertificateAlgorithm
        {
            get
            {
                var defaultAlgorithm = PlatformSupport.HasCngSupport ? Algorithm.ECDSA_P256 : Algorithm.RSA;
                Algorithm result;
                return Algorithm.TryParse(Fiddler.FiddlerApplication.Prefs.GetStringPref(EECertificateAlgorithmPreferenceKey, defaultAlgorithm.ToString()), out result) ? result : defaultAlgorithm;
            }
            set
            {
                Fiddler.FiddlerApplication.Prefs.SetStringPref(EECertificateAlgorithmPreferenceKey, value.ToString());
            }
        }

        public static int RootRsaKeySize
        {
            get
            {
                const int defaultKeySize = 2048;
                return Fiddler.FiddlerApplication.Prefs.GetInt32Pref(RootCertificateRsaKeySizePreferenceKey, defaultKeySize);
            }
            set
            {
                Fiddler.FiddlerApplication.Prefs.SetInt32Pref(RootCertificateRsaKeySizePreferenceKey, value);
            }
        }

        public static int EERsaKeySize
        {
            get
            {
                const int defaultKeySize = 2048;
                return Fiddler.FiddlerApplication.Prefs.GetInt32Pref(EECertificateRsaKeySizePreferenceKey, defaultKeySize);
            }
            set
            {
                Fiddler.FiddlerApplication.Prefs.SetInt32Pref(EECertificateRsaKeySizePreferenceKey, value);
            }
        }
    }
}
