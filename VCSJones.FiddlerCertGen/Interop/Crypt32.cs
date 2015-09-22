using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VCSJones.FiddlerCertGen.Interop
{
    internal static class AdvApi32
    {
        private const string ADVAPI32 = "advapi32.dll";

        [method: DllImport(ADVAPI32, EntryPoint = "CryptReleaseContext", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern bool CryptReleaseContext(
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pContext,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags
        );

        [method: DllImport(ADVAPI32, EntryPoint = "CryptAcquireContext", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CryptAcquireContext
            (
                [param: Out] out NCryptKeyOrCryptProviderSafeHandle phProv,
                [param: In, MarshalAs(UnmanagedType.LPTStr)] string pszContainer,
                [param: In, MarshalAs(UnmanagedType.LPTStr)] string pszProvider,
                [param: In, MarshalAs(UnmanagedType.U4)] ProviderType dwProvType,
                [param: In, MarshalAs(UnmanagedType.U4)] CryptAcquireContextFlags dwFlags
            );

        [method: DllImport(ADVAPI32, EntryPoint = "CryptGenKey", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CryptGenKey
            (
                [param: In] NCryptKeyOrCryptProviderSafeHandle hProv,
                [param: In, MarshalAs(UnmanagedType.U4)] KeySpec Algid,
                [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags,
                [param: Out] out CryptKeySafeHandle phKey
            );


        [method: DllImport(ADVAPI32, EntryPoint = "CryptGetProvParam", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CryptGetProvParam
            (
                [param: In] NCryptKeyOrCryptProviderSafeHandle hProv,
                [param: In, MarshalAs(UnmanagedType.U4)] uint dwParam,
                [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pbData,
                [param: In, Out, MarshalAs(UnmanagedType.U4)] ref uint pdwDataLen,
                [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags
            );

        [method: DllImport(ADVAPI32, EntryPoint = "CryptGetProvParam", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern unsafe bool CryptGetProvParam
            (
                [param: In] NCryptKeyOrCryptProviderSafeHandle hProv,
                [param: In, MarshalAs(UnmanagedType.U4)] uint dwParam,
                [param: In] void* pbData,
                [param: In, Out, MarshalAs(UnmanagedType.U4)] ref uint pdwDataLen,
                [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags
            );

        [method: DllImport(ADVAPI32, EntryPoint = "CryptDestroyKey", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CryptDestroyKey
            (
                [param: In] IntPtr hKey
            );
    }

    internal static class Crypt32
    {
        private const string CRYPT32 = "crypt32.dll";

        [method: DllImport(CRYPT32, CallingConvention = CallingConvention.Winapi, EntryPoint = "CertCreateSelfSignCertificate", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CertCreateSelfSignCertificate
            (
            [param: In] NCryptKeyOrCryptProviderSafeHandle nCryptKey,
            [param: In, MarshalAs(UnmanagedType.Struct)] ref NATIVE_CRYPTOAPI_BLOB pSubjectIssuerBlob,
            [param: In, MarshalAs(UnmanagedType.U4)] SelfSignFlags dwFlags,
            [param: In, MarshalAs(UnmanagedType.Struct)] ref CRYPT_KEY_PROV_INFO pKeyProvInfo,
            [param: In, MarshalAs(UnmanagedType.Struct)] ref CRYPT_ALGORITHM_IDENTIFIER pSignatureAlgorithm,
            [param: In, MarshalAs(UnmanagedType.LPStruct)] SYSTEMTIME pStartTime,
            [param: In, MarshalAs(UnmanagedType.LPStruct)] SYSTEMTIME pEndTime,
            [param: In, MarshalAs(UnmanagedType.Struct)] ref CERT_EXTENSIONS pExtensions
            );

        [method: DllImport(CRYPT32, CallingConvention = CallingConvention.Winapi, EntryPoint = "CryptEncodeObjectEx", SetLastError = true)]
        public static extern bool CryptEncodeObjectEx
            (
            [param: In, MarshalAs(UnmanagedType.U4)] EncodingType dwCertEncodingType,
            [param: In, MarshalAs(UnmanagedType.LPStr)] string lpszStructType,
            [param: In, MarshalAs(UnmanagedType.Struct)] ref CERT_ALT_NAME_INFO pvStructInfo,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pEncodePara,
            [param: Out, MarshalAs(UnmanagedType.SysInt)] out IntPtr pvEncoded,
            [param: In, Out, MarshalAs(UnmanagedType.U4)] ref uint pcbEncoded
            );

        [method: DllImport(CRYPT32, CallingConvention = CallingConvention.Winapi, EntryPoint = "CryptEncodeObjectEx", SetLastError = true)]
        public static extern bool CryptEncodeObjectEx
            (
            [param: In, MarshalAs(UnmanagedType.U4)] EncodingType dwCertEncodingType,
            [param: In, MarshalAs(UnmanagedType.LPStr)] string lpszStructType,
            [param: In, MarshalAs(UnmanagedType.Struct)] ref CERT_AUTHORITY_KEY_ID2_INFO pvStructInfo,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pEncodePara,
            [param: Out, MarshalAs(UnmanagedType.SysInt)] out IntPtr pvEncoded,
            [param: In, Out, MarshalAs(UnmanagedType.U4)] ref uint pcbEncoded
            );

        [method: DllImport(CRYPT32, CallingConvention = CallingConvention.Winapi, EntryPoint = "CryptExportPublicKeyInfoEx", SetLastError = true)]
        public static extern bool CryptExportPublicKeyInfoEx
            (
            [param: In] NCryptKeyOrCryptProviderSafeHandle hCryptProvOrNCryptKey,
            [param: In, MarshalAs(UnmanagedType.U4)] KeySpec dwKeySpec,
            [param: In, MarshalAs(UnmanagedType.U4)] EncodingType dwCertEncodingType,
            [param: In, MarshalAs(UnmanagedType.LPStr)] string pszPublicKeyObjId,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pvAuxInfo,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pInfo,
            [param: In, Out, MarshalAs(UnmanagedType.U4)] ref uint pcbInfo
            );

        [method: DllImport(CRYPT32, CallingConvention = CallingConvention.Winapi, EntryPoint = "CryptSignAndEncodeCertificate", SetLastError = true)]
        public static extern bool CryptSignAndEncodeCertificate
            (
            [param: In] NCryptKeyOrCryptProviderSafeHandle hCryptProvOrNCryptKey,
            [param: In, MarshalAs(UnmanagedType.U4)] KeySpec dwKeySpec,
            [param: In, MarshalAs(UnmanagedType.U4)] EncodingType dwCertEncodingType,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr lpszStructType,
            [param: In, MarshalAs(UnmanagedType.Struct)] ref CERT_INFO pvStructInfo,
            [param: In, MarshalAs(UnmanagedType.Struct)] ref CRYPT_ALGORITHM_IDENTIFIER pSignatureAlgorithm,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pvHashAuxInfo,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pbEncoded,
            [param: In, Out, MarshalAs(UnmanagedType.U4)] ref uint pcbEncoded
            );

        [method: DllImport(CRYPT32, EntryPoint = "CryptAcquireCertificatePrivateKey", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern bool CryptAcquireCertificatePrivateKey(
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pCert,
            [param: In, MarshalAs(UnmanagedType.U4)] AcquirePrivateKeyFlags dwFlags,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pvParameters,
            [param: Out] out NCryptKeyOrCryptProviderSafeHandle phCryptProvOrNCryptKey,
            [param: Out, MarshalAs(UnmanagedType.U4)] out KeySpec pdwKeySpec,
            [param: Out, MarshalAs(UnmanagedType.Bool)] out bool pfCallerFreeProvOrNCryptKey
        );

        [method: DllImport("crypt32.dll", EntryPoint = "CryptBinaryToString", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public static extern bool CryptBinaryToString(
            [param: In, MarshalAs(UnmanagedType.LPArray)] byte[] pbBinary,
            [param: In, MarshalAs(UnmanagedType.U4)] uint cbBinary,
            [param: In, MarshalAs(UnmanagedType.U4)] CryptBinaryStringFlags dwFlags,
            [param: In, Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszString,
            [param: In, Out, MarshalAs(UnmanagedType.U4)] ref uint pcchString
        );

        [method: DllImport("crypt32.dll", EntryPoint = "CryptBinaryToString", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public static extern bool CryptBinaryToString(
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pbBinary,
            [param: In, MarshalAs(UnmanagedType.U4)] uint cbBinary,
            [param: In, MarshalAs(UnmanagedType.U4)] CryptBinaryStringFlags dwFlags,
            [param: In, Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszString,
            [param: In, Out, MarshalAs(UnmanagedType.U4)] ref uint pcchString
        );

        [method: DllImport("crypt32.dll", EntryPoint = "CertSetCertificateContextProperty", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public static extern bool CertSetCertificateContextProperty(
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pCertContext,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwPropId,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags,
            [param: In] NCryptKeyOrCryptProviderSafeHandle pvData
        );
        [method: DllImport("crypt32.dll", EntryPoint = "CertSetCertificateContextProperty", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public static extern bool CertSetCertificateContextProperty(
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pCertContext,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwPropId,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags,
            [param: In, MarshalAs(UnmanagedType.Struct)] ref CRYPT_KEY_PROV_INFO pvData
        );
    }
}
