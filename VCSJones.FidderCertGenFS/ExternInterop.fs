namespace VCSJones.FiddlerCertGen
open System.Runtime.InteropServices

module internal ExternInterop =
    [<DllImportAttribute("crypt32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "CryptEncodeObjectEx", SetLastError = true)>]
    extern [<return: MarshalAsAttribute(UnmanagedType.Bool)>]bool CryptEncodeObjectExAltnerateName(
            [<InAttribute; MarshalAsAttribute(UnmanagedType.U4)>] EncodingType dwCertEncodingType,
            [<InAttribute; MarshalAsAttribute(UnmanagedType.LPStr)>] string lpszStructType,
            [<InAttribute; MarshalAsAttribute(UnmanagedType.Struct)>] CERT_ALT_NAME_INFO& pvStructInfo,
            [<InAttribute; MarshalAsAttribute(UnmanagedType.U4)>] uint32 dwFlags,
            [<InAttribute; MarshalAsAttribute(UnmanagedType.SysInt)>] nativeint pEncodePara,
            [<OutAttribute>] LocalBufferSafeHandle& pvEncoded,
            [<InAttribute; OutAttribute; MarshalAs(UnmanagedType.U4)>] uint32& pvbEncoded
        );
    [<DllImportAttribute("crypt32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "CryptEncodeObjectEx", SetLastError = true)>]
    extern [<return: MarshalAsAttribute(UnmanagedType.Bool)>]bool CryptEncodeObjectExAuthority(
            [<InAttribute; MarshalAsAttribute(UnmanagedType.U4)>] EncodingType dwCertEncodingType,
            [<InAttribute; MarshalAsAttribute(UnmanagedType.LPStr)>] string lpszStructType,
            [<InAttribute; MarshalAsAttribute(UnmanagedType.Struct)>] CERT_AUTHORITY_KEY_ID2_INFO& pvStructInfo,
            [<InAttribute; MarshalAsAttribute(UnmanagedType.U4)>] uint32 dwFlags,
            [<InAttribute; MarshalAsAttribute(UnmanagedType.SysInt)>] nativeint pEncodePara,
            [<OutAttribute>] LocalBufferSafeHandle& pvEncoded,
            [<InAttribute; OutAttribute; MarshalAs(UnmanagedType.U4)>] uint32& pvbEncoded
        );

        [<DllImport("ncrypt.dll", EntryPoint = "NCryptOpenStorageProvider", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)>]
        extern [<return: MarshalAs(UnmanagedType.Error)>]uint32 NCryptOpenStorageProvider(
                [<OutAttribute>] NCryptStorageProviderSafeHandle& phProvider,
                [<InAttribute; MarshalAs(UnmanagedType.LPWStr)>] string pszProviderName,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>]uint32 dwFlags
            );

        [<DllImport("ncrypt.dll", EntryPoint = "NCryptGetProperty", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)>]
        extern [<return: MarshalAs(UnmanagedType.Error)>]uint32 NCryptGetProperty(
                [<InAttribute>]NCryptHandleBase hObject,
                [<InAttribute; MarshalAs(UnmanagedType.LPWStr)>]string pszProperty,
                [<InAttribute; MarshalAs(UnmanagedType.SysInt)>]nativeint pbOutput,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>]uint32 cbOutput,
                [<OutAttribute; MarshalAs(UnmanagedType.U4)>]uint32& pbcResult,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>]uint32 dwFlags
            );

        [<DllImport("ncrypt.dll", EntryPoint = "NCryptSetProperty", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)>]
        extern [<return: MarshalAs(UnmanagedType.Error)>]uint32 NCryptSetProperty(
                [<InAttribute>]NCryptHandleBase hObject,
                [<InAttribute; MarshalAs(UnmanagedType.LPWStr)>]string pszProperty,
                [<InAttribute; MarshalAs(UnmanagedType.SysInt)>]nativeint pbInput,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>]uint32 cbInput,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>]uint32 dwFlags
            );

        [<DllImport("ncrypt.dll", EntryPoint = "NCryptCreatePersistedKey", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)>]
        extern [<return: MarshalAs(UnmanagedType.Error)>]uint32 NCryptCreatePersistedKey(
                [<InAttribute>]NCryptStorageProviderSafeHandle hProvider,
                [<OutAttribute>]NCryptKeyOrCryptProviderSafeHandle& phKey,
                [<InAttribute; MarshalAs(UnmanagedType.LPWStr)>]string pszAlgId,
                [<InAttribute; MarshalAs(UnmanagedType.LPWStr)>]string pszKeyName,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>]KeySpec dwLegacyKeySpec,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>]NCryptCreatePersistedKeyFlags dwFlags
            );

        [<DllImport("ncrypt.dll", EntryPoint = "NCryptFinalizeKey", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)>]
        extern [<return: MarshalAs(UnmanagedType.Error)>]uint32 NCryptFinalizeKey(
                [<InAttribute>]NCryptKeyOrCryptProviderSafeHandle hKey,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>]uint32 dwFlags
            );

        [<DllImport("ncrypt.dll", EntryPoint = "NCryptOpenKey", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)>]
        extern [<return: MarshalAs(UnmanagedType.Error)>]uint32 NCryptOpenKey(
                [<InAttribute>]NCryptStorageProviderSafeHandle hProvider,
                [<OutAttribute>]NCryptKeyOrCryptProviderSafeHandle& phKey,
                [<InAttribute; MarshalAs(UnmanagedType.LPWStr)>]string pszKeyName,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>]KeySpec dwLegacyKeySpec,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>]uint32 dwFlags
            );

        [<DllImport("advapi32.dll", EntryPoint = "CryptAcquireContext", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)>]
        extern [<return: MarshalAs(UnmanagedType.Bool)>]bool CryptAcquireContext(
                [<OutAttribute>] NCryptKeyOrCryptProviderSafeHandle& phProv,
                [<InAttribute; MarshalAs(UnmanagedType.LPTStr)>] string pszContainer,
                [<InAttribute; MarshalAs(UnmanagedType.LPTStr)>] string pszProvider,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>] ProviderType dwProvType,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>] CryptAcquireContextFlags dwFlags
            );

        [<DllImport("advapi32.dll", EntryPoint = "CryptGenKey", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)>]
        extern [<return: MarshalAs(UnmanagedType.Bool)>]bool CryptGenKey(
                [<InAttribute;>] NCryptKeyOrCryptProviderSafeHandle hProv,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>] KeySpec Algid,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>] uint32 dwFlags,
                [<OutAttribute;>] CryptKeySafeHandle& phKey
            );

        [<DllImport("advapi32.dll", EntryPoint = "CryptGetProvParam", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)>]
        extern [<return: MarshalAs(UnmanagedType.Bool)>]bool CryptGetProvParam(
                [<InAttribute;>] NCryptKeyOrCryptProviderSafeHandle hProv,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>] uint32 dwParam,
                [<InAttribute; MarshalAs(UnmanagedType.SysInt)>] nativeint pbData,
                [<InAttribute; OutAttribute; MarshalAs(UnmanagedType.SysInt)>] uint32& pdwDataLen,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>] uint32 dwFlags
            );

        [<DllImport("crypt32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "CryptExportPublicKeyInfoEx", SetLastError = true)>]
        extern [<return: MarshalAs(UnmanagedType.Bool)>]bool CryptExportPublicKeyInfoEx(
                [<InAttribute>] NCryptKeyOrCryptProviderSafeHandle hCryptProvOrNCryptKey,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>] KeySpec dwKeySpec,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>] EncodingType dwCertEncodingType,
                [<InAttribute; MarshalAs(UnmanagedType.LPStr)>] string pszPublicKeyObjId,
                [<InAttribute; MarshalAs(UnmanagedType.U4)>] uint32 dwFlags,
                [<InAttribute; MarshalAs(UnmanagedType.SysInt)>] nativeint pvAuxInfo,
                [<InAttribute; MarshalAs(UnmanagedType.SysInt)>] nativeint pInfo,
                [<InAttribute; OutAttribute; MarshalAs(UnmanagedType.U4)>] uint32& pcbInfo
            );