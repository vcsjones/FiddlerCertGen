using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    internal static class NCrypt
    {
        private const string NCRYPT = "ncrypt.dll";

        [return: MarshalAs(UnmanagedType.Bool)]
        [method: DllImport(NCRYPT, EntryPoint = "NCryptIsKeyHandle", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern bool NCryptIsKeyHandle
            (
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr hKey
            );

        [return: MarshalAs(UnmanagedType.Error)]
        [method: DllImport(NCRYPT, EntryPoint = "NCryptFreeObject", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern uint NCryptFreeObject
            (
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr hObject
            );

        [return: MarshalAs(UnmanagedType.Error)]
        [method: DllImport(NCRYPT, EntryPoint = "NCryptOpenStorageProvider", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern uint NCryptOpenStorageProvider
            (
            [param: Out] out NCryptStorageProviderSafeHandle phProvider,
            [param: In, MarshalAs(UnmanagedType.LPWStr)] string pszProviderName,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags
            );

        [return: MarshalAs(UnmanagedType.Error)]
        [method: DllImport(NCRYPT, EntryPoint = "NCryptCreatePersistedKey", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern uint NCryptCreatePersistedKey
            (
            [param: In] NCryptStorageProviderSafeHandle hProvider,
            [param: Out] out NCryptKeyOrCryptProviderSafeHandle phKey,
            [param: In, MarshalAs(UnmanagedType.LPWStr)] string pszAlgId,
            [param: In, MarshalAs(UnmanagedType.LPWStr)] string pszKeyName,
            [param: In, MarshalAs(UnmanagedType.U4)] KeySpec dwLegacyKeySpec,
            [param: In, MarshalAs(UnmanagedType.U4)] NCryptCreatePersistedKeyFlags dwFlags
            );

        [return: MarshalAs(UnmanagedType.Error)]
        [method: DllImport(NCRYPT, EntryPoint = "NCryptGetProperty", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern uint NCryptGetProperty
            (
            [param: In] NCryptHandleBase hObject,
            [param: In, MarshalAs(UnmanagedType.LPWStr)] string pszProperty,
            [param: In] IntPtr pbOutput,
            [param: In, MarshalAs(UnmanagedType.U4)] uint cbOutput,
            [param: Out, MarshalAs(UnmanagedType.U4)] out uint pcbResult,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags
            );

        [return: MarshalAs(UnmanagedType.Error)]
        [method: DllImport(NCRYPT, EntryPoint = "NCryptSetProperty", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern uint NCryptSetProperty
            (
            [param: In] NCryptHandleBase hObject,
            [param: In, MarshalAs(UnmanagedType.LPWStr)] string pszProperty,
            [param: In] IntPtr pbInput,
            [param: In, MarshalAs(UnmanagedType.U4)] uint cbInput,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags
            );

        [return: MarshalAs(UnmanagedType.Error)]
        [method: DllImport(NCRYPT, EntryPoint = "NCryptFinalizeKey", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern uint NCryptFinalizeKey
            (
            [param: In] NCryptKeyOrCryptProviderSafeHandle hKey,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags
            );

        [return: MarshalAs(UnmanagedType.Error)]
        [method: DllImport(NCRYPT, EntryPoint = "NCryptOpenKey", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern uint NCryptOpenKey
            (
            [param: In] NCryptStorageProviderSafeHandle hProvider,
            [param: Out] out NCryptKeyOrCryptProviderSafeHandle phKey,
            [param: In, MarshalAs(UnmanagedType.LPWStr)] string pszKeyName,
            [param: In, MarshalAs(UnmanagedType.U4)] KeySpec dwLegacyKeySpec,
            [param: In, MarshalAs(UnmanagedType.U4)] uint dwFlags
            );

        [return: MarshalAs(UnmanagedType.Error)]
        [method: DllImport(NCRYPT, EntryPoint = "NCryptExportKey")]
        public static extern uint NCryptExportKey
            (
            [param: In] NCryptKeyOrCryptProviderSafeHandle hKey,
            [param: In] NCryptKeyOrCryptProviderSafeHandle hExportKey,
            [param: In, MarshalAs(UnmanagedType.LPWStr)] string pszBlobType,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pParameterList,
            [param: In] LocalBufferSafeHandle pbOutput,
            [param: In, MarshalAs(UnmanagedType.U4)]uint cbOutput,
            [param: Out, MarshalAs(UnmanagedType.U4)] out uint pcbResult,
            [param: In, MarshalAs(UnmanagedType.U4)]uint dwFlags
            );
    }
}