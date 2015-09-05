namespace VCSJones.FiddlerCertGen
{
    public abstract class KeyProviderBase
    {
        internal abstract NCryptKeyOrCryptProviderSafeHandle CreateKey(string keyName, int keySize, Algorithm algorithm, bool overwrite);
        internal abstract string GetProviderName();
        internal abstract string GetName(NCryptKeyOrCryptProviderSafeHandle handle);
        internal abstract string GetAlgorithmGroup(NCryptKeyOrCryptProviderSafeHandle handle);
        internal abstract NCryptKeyOrCryptProviderSafeHandle OpenExisting(string keyName);
        public abstract string Name { get; }
    }
}