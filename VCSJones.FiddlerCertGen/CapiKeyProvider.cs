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
            _providerName = "Microsoft Enhanced Cryptographic Provider v1.0";
        }


        internal override NCryptKeyOrCryptProviderSafeHandle CreateKey(string keyName, int keySize, Algorithm algorithm, bool overwrite, KeyUsage keyUsage, out KeySpec keySpec)
        {
            const int ALREADY_EXISTS = unchecked((int)0x8009000f);
            if (algorithm != Algorithm.RSA)
            {
                throw new ArgumentException("CAPI does not support algorithms other than RSA.", nameof(algorithm));
            }
            NCryptKeyOrCryptProviderSafeHandle provider;
            if (!AdvApi32.CryptAcquireContext(out provider, keyName, _providerName, ProviderType.PROV_RSA_FULL, CryptAcquireContextFlags.CRYPT_NEWKEYSET))
            {
                var lastError = Marshal.GetLastWin32Error();
                if (lastError == ALREADY_EXISTS && overwrite)
                {
                    if (!AdvApi32.CryptAcquireContext(out provider, keyName, _providerName, ProviderType.PROV_RSA_FULL, CryptAcquireContextFlags.CRYPT_DELETEKEYSET))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                    if (!AdvApi32.CryptAcquireContext(out provider, keyName, _providerName, ProviderType.PROV_RSA_FULL, CryptAcquireContextFlags.CRYPT_NEWKEYSET))
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
            return ReadProvParam(handle, ProvParam.PP_NAME, Marshal.PtrToStringAnsi);
        }

        internal override string GetAlgorithmGroup(NCryptKeyOrCryptProviderSafeHandle handle)
        {
            //Hack, but we only support RSA right now.
            return AlgorithmGroup.RSA.Name;
        }

        internal override NCryptKeyOrCryptProviderSafeHandle OpenExisting(string keyName)
        {
            const int DOES_NOT_EXIST = unchecked((int)0x80090016);
            NCryptKeyOrCryptProviderSafeHandle provider;
            if (!AdvApi32.CryptAcquireContext(out provider, keyName, _providerName, ProviderType.PROV_RSA_FULL, 0u))
            {
                var result = Marshal.GetLastWin32Error();
                if (result == DOES_NOT_EXIST)
                {
                    return null;
                }
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return provider;
        }

        private static T ReadProvParam<T>(NCryptKeyOrCryptProviderSafeHandle hProv, ProvParam property, Func<IntPtr, T> convert)
        {
            uint size = 0;
            if (!AdvApi32.CryptGetProvParam(hProv, property, IntPtr.Zero, ref size, 0u))
            {
                throw new InvalidOperationException("Failed to get property.");
            }
            var buffer = Marshal.AllocHGlobal((int)size);
            try
            {
                if (!AdvApi32.CryptGetProvParam(hProv, property, buffer, ref size, 0u))
                {
                    throw new InvalidOperationException("Failed to get property.");
                }
                return convert(buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}