﻿namespace VCSJones.FiddlerCertGen
{
    public static class KeyProviders
    {
        public static KeyProviderBase CNG => CngKeyProvider.Instance;
        public static KeyProviderBase CAPI => CapiKeyProvider.Instance;
    }
}