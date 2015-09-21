using System;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    internal class CngKeyProvider : KeyProviderBase
    {
        internal static CngKeyProvider Instance { get; } = new CngKeyProvider();
        private static readonly NCryptStorageProvider _storageProvider = NCryptStorageProvider.MicrosoftSoftwareKeyStorageProvider;
        public override string Name { get; } = "CNG";

        internal override NCryptKeyOrCryptProviderSafeHandle CreateKey(string keyName, int keySize, Algorithm algorithm, bool overwrite, out KeySpec keySpec)
        {
            NCryptKeyOrCryptProviderSafeHandle keyHandle;
            var flags = overwrite ? NCryptCreatePersistedKeyFlags.NCRYPT_OVERWRITE_KEY_FLAG : NCryptCreatePersistedKeyFlags.NONE;
            var result = NCrypt.NCryptCreatePersistedKey(_storageProvider.Handle, out keyHandle, algorithm.Name, keyName, KeySpec.NONE, flags);
            if (result != SECURITY_STATUS.ERROR_SUCCESS)
            {
                throw new InvalidOperationException("Failed to generate a key.");
            }
            if (algorithm == Algorithm.RSA)
            {
                NCryptPropertyWriter.WriteUInt32(keyHandle, CngProperties.NCRYPT_LENGTH_PROPERTY, (uint) keySize);
            }
            NCryptPropertyWriter.WriteEnum(keyHandle, CngProperties.NCRYPT_EXPORT_POLICY_PROPERTY, CngExportPolicy.NCRYPT_ALLOW_EXPORT_FLAG);
            var finalizeResult = NCrypt.NCryptFinalizeKey(keyHandle, 0u);
            if (finalizeResult != SECURITY_STATUS.ERROR_SUCCESS)
            {
                throw new InvalidOperationException("Failed to finalize key.");
            }
            keySpec = KeySpec.NONE;
            return keyHandle;
        }

        internal override string GetProviderName()
        {
            return _storageProvider.Name;
        }

        internal override string GetName(NCryptKeyOrCryptProviderSafeHandle handle)
        {
            return NCryptPropertyReader.ReadStringUni(handle, CngProperties.NCRYPT_NAME_PROPERTY);
        }

        internal override string GetAlgorithmGroup(NCryptKeyOrCryptProviderSafeHandle handle)
        {
            return NCryptPropertyReader.ReadStringUni(handle, CngProperties.NCRYPT_ALGORITHM_GROUP_PROPERTY);
        }

        internal override NCryptKeyOrCryptProviderSafeHandle OpenExisting(string keyName, out KeySpec keySpec)
        {
            keySpec = KeySpec.NONE;
            NCryptKeyOrCryptProviderSafeHandle keyHandle;
            var result = NCrypt.NCryptOpenKey(_storageProvider.Handle, out keyHandle, keyName, KeySpec.NONE, 0u);
            if (result == SECURITY_STATUS.ERROR_SUCCESS)
            {
                return keyHandle;
            }
            if (result == SECURITY_STATUS.NTE_BAD_KEYSET)
            {
                return null;
            }
            throw new InvalidOperationException("Failed to open key.");
        }
    }
}