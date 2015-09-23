using System;
using System.Runtime.InteropServices;

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
            [param: In, MarshalAs(UnmanagedType.U4)] ProvParam dwParam,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pbData,
            [param: In, Out, MarshalAs(UnmanagedType.U4)] ref uint pdwDataLen,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags
            );

        [method: DllImport(ADVAPI32, EntryPoint = "CryptDestroyKey", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CryptDestroyKey
            (
            [param: In] IntPtr hKey
            );
    }
}