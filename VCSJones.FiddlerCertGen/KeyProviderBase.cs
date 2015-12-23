using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    public abstract class KeyProviderBase
    {
        internal abstract NCryptKeyOrCryptProviderSafeHandle CreateKey(string keyName, int keySize, Algorithm algorithm, bool overwrite, KeyUsage keyUsage, out KeySpec keySpec);
        internal abstract string GetProviderName();
        internal abstract string GetName(NCryptKeyOrCryptProviderSafeHandle handle);
        internal abstract string GetAlgorithmGroup(NCryptKeyOrCryptProviderSafeHandle handle);
        internal abstract NCryptKeyOrCryptProviderSafeHandle OpenExisting(string keyName, out KeySpec keySpec);
        public abstract string Name { get; }
        internal abstract byte[] Export(NCryptKeyOrCryptProviderSafeHandle handle);
    }
}