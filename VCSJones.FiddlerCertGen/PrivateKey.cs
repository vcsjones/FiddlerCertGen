using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    /// <summary>
    /// Represents a private key. This may either be a CNG key or a CAPI key.
    /// </summary>
    public class PrivateKey : IDisposable
    {
        private readonly NCryptKeyOrCryptProviderSafeHandle _handle;
        private readonly KeyProviderBase _keyProvider;
        private readonly KeySpec _keySpec;

        internal PrivateKey(NCryptKeyOrCryptProviderSafeHandle handle, KeyProviderBase keyProvider, KeySpec keySpec)
        {
            _handle = handle;
            _keyProvider = keyProvider;
            _keySpec = keySpec;
        }

        /// <summary>
        /// Creates a new private key.
        /// </summary>
        /// <param name="keyProvider">The provider for the key. <see cref="KeyProviders"/> contains different types of providers.</param>
        /// <param name="keyName">The name of the key to persist. Use <c>null</c> to create an ephemeral key.</param>
        /// <param name="algorithm">The algorithm of the key. Not all providers support all algorithms.</param>
        /// <param name="keyUsage">Specifies the purpose of the key. This is not appicable to CNG algorithms.</param>
        /// <param name="keySize">The size of the key. Only valid for RSA keys.</param>
        /// <param name="overwrite">True to overwrite the provider's exisint private key, otherwise false.</param>
        /// <returns>A new private key.</returns>
        public static PrivateKey CreateNew(KeyProviderBase keyProvider, string keyName, Algorithm algorithm, KeyUsage keyUsage, int? keySize = null, bool overwrite = false)
        {
            var keySizeValue = keySize ?? 2048;
            KeySpec keySpec;
            var handle = keyProvider.CreateKey(keyName, keySizeValue, algorithm, overwrite, keyUsage, out keySpec);
            return new PrivateKey(handle, keyProvider, keySpec);
        }

        public static PrivateKey OpenExisting(KeyProviderBase keyProvider, string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException(nameof(keyName));
            }
            KeySpec keySpec;
            var handle = keyProvider.OpenExisting(keyName, out keySpec);
            if (handle == null)
            {
                return null;
            }
            return new PrivateKey(handle, keyProvider, keySpec);
        }

        internal NCryptKeyOrCryptProviderSafeHandle Handle => _handle;
        public string ProviderName => _keyProvider.GetProviderName();
        public string Name => _keyProvider.GetName(_handle);
        public AlgorithmGroup AlgorithmGroup => new AlgorithmGroup(_keyProvider.GetAlgorithmGroup(_handle));

        public KeySpec KeySpec => _keySpec;

        public PublicKeyInfo ToPublicKey()
        {
            uint infoSize = 0;
            var publicKeyObjId = AlgorithmGroup == AlgorithmGroup.RSA ? OIDs.RSA_PUBLIC_KEY : OIDs.ECC_PUBLIC_KEY;
            if (Crypt32.CryptExportPublicKeyInfoEx(_handle, _keySpec, EncodingType.X509_ASN_ENCODING | EncodingType.PKCS_7_ASN_ENCODING, publicKeyObjId, 0, IntPtr.Zero, IntPtr.Zero, ref infoSize))
            {
                var buffer = Marshal.AllocHGlobal((int)infoSize);
                if (Crypt32.CryptExportPublicKeyInfoEx(_handle, _keySpec, EncodingType.X509_ASN_ENCODING | EncodingType.PKCS_7_ASN_ENCODING, publicKeyObjId, 0, IntPtr.Zero, buffer, ref infoSize))
                {
                    return new PublicKeyInfo(buffer);
                }
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public byte[] Export() => _keyProvider.Export(_handle);

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}
