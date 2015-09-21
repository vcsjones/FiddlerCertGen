using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{


    internal class CapiKeyProvider : KeyProviderBase
    {
        internal static CapiKeyProvider Instance { get; } = new CapiKeyProvider();
        public override string Name { get; } = "CAPI";


        private static readonly string _providerName;

        static CapiKeyProvider()
        {
            //Windows XP has a goofy name for the provider.
            _providerName = PlatformSupport.UseLegacyKeyStoreName ? "Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype)" : "Microsoft Enhanced RSA and AES Cryptographic Provider";
        }


        internal override NCryptKeyOrCryptProviderSafeHandle CreateKey(string keyName, int keySize, Algorithm algorithm, bool overwrite, KeyUsage keyUsage, out KeySpec keySpec)
        {
            const int ALREADY_EXISTS = unchecked((int)0x8009000f);
            if (algorithm != Algorithm.RSA)
            {
                throw new ArgumentException("CAPI does not support algorithms other than RSA.", nameof(algorithm));
            }
            NCryptKeyOrCryptProviderSafeHandle provider;
            if (!AdvApi32.CryptAcquireContext(out provider, keyName, _providerName, ProviderType.PROV_RSA_AES, CryptAcquireContextFlags.CRYPT_NEWKEYSET))
            {
                var lastError = Marshal.GetLastWin32Error();
                if (lastError == ALREADY_EXISTS && overwrite)
                {
                    if (!AdvApi32.CryptAcquireContext(out provider, keyName, _providerName, ProviderType.PROV_RSA_AES, CryptAcquireContextFlags.CRYPT_DELETEKEYSET))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                    if (!AdvApi32.CryptAcquireContext(out provider, keyName, _providerName, ProviderType.PROV_RSA_AES, CryptAcquireContextFlags.CRYPT_NEWKEYSET))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            var flags = CryptGenKeyFlags.CRYPT_EXPORTABLE;
            var keySizeFlags = ((uint)keySize & 0xFFFFU) << 16;
            var genKeyFlags = ((ushort)flags) | keySizeFlags;
            CryptKeySafeHandle key;
            KeySpec algorithmKeySpec;
            switch (keyUsage)
            {
                case KeyUsage.KeyExchange:
                    algorithmKeySpec = KeySpec.AT_KEYEXCHANGE;
                    break;
                case KeyUsage.Signature:
                    algorithmKeySpec = KeySpec.AT_SIGNATURE;
                    break;
                default:
                    throw new ArgumentException(nameof(keyUsage));
            }
            if (!AdvApi32.CryptGenKey(provider, algorithmKeySpec, genKeyFlags, out key))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            key.Close();
            keySpec = algorithmKeySpec;
            return provider;
        }

        internal override string GetProviderName()
        {
            return _providerName;
        }

        internal override string GetName(NCryptKeyOrCryptProviderSafeHandle handle)
        {
            const int PP_NAME = 0x6;
            uint size = 0;
            if (!AdvApi32.CryptGetProvParam(handle, PP_NAME, IntPtr.Zero, ref size, 0u))
            {
                throw new InvalidOperationException("Failed to get property.");
            }
            var buffer = Marshal.AllocHGlobal((int)size);
            try
            {
                if (!AdvApi32.CryptGetProvParam(handle, PP_NAME, buffer, ref size, 0u))
                {
                    throw new InvalidOperationException("Failed to get property.");
                }
                return Marshal.PtrToStringAnsi(buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        internal override string GetAlgorithmGroup(NCryptKeyOrCryptProviderSafeHandle handle)
        {
            //Hack, but we only support RSA right now.
            return AlgorithmGroup.RSA.Name;
        }

        internal override unsafe NCryptKeyOrCryptProviderSafeHandle OpenExisting(string keyName, out KeySpec keySpec)
        {
            keySpec = KeySpec.NONE;
            const int DOES_NOT_EXIST = unchecked((int)0x80090016);
            NCryptKeyOrCryptProviderSafeHandle provider;
            if (!AdvApi32.CryptAcquireContext(out provider, keyName, _providerName, ProviderType.PROV_RSA_AES, 0u))
            {
                var result = Marshal.GetLastWin32Error();
                if (result == DOES_NOT_EXIST)
                {
                    return null;
                }
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            const uint PP_KEYSPEC = 0x27;
            uint* keySpecBuffer = stackalloc uint[1];
            var dataLength = (uint)Marshal.SizeOf(typeof(uint));
            if (!AdvApi32.CryptGetProvParam(provider, PP_KEYSPEC, keySpecBuffer, ref dataLength, 0u))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            keySpec = (KeySpec)(*keySpecBuffer);
            return provider;
        }
    }
}