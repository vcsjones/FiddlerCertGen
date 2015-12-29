using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    internal static class Kernel32
    {
        private const string KERNEL32 = "kernel32.dll";

        [method: DllImport(KERNEL32, EntryPoint = "LoadLibraryEx", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public static extern LibrarySafeHandle LoadLibraryEx
            (
                [param: In, MarshalAs(UnmanagedType.LPTStr)] string lpFileName,
                [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr hFile,
                [param: In, MarshalAs(UnmanagedType.U4)] LoadLibraryFlags dwFlags
            );

        [return: MarshalAs(UnmanagedType.Bool)]
        [method: DllImport(KERNEL32, EntryPoint = "FreeLibrary", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern bool FreeLibrary
            (
                [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr hModule
            );

        [return: MarshalAs(UnmanagedType.SysInt)]
        [method: DllImport(KERNEL32, EntryPoint = "GetProcAddress", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern IntPtr GetProcAddress
            (
                [param: In] LibrarySafeHandle hModule,
                [param: In, MarshalAs(UnmanagedType.LPStr)] string lpProcName
            );

        [return: MarshalAs(UnmanagedType.SysInt)]
        [method: DllImport(KERNEL32, EntryPoint = "LocalFree", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern IntPtr LocalFree
            (
                [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr hMem
            );

        [return: MarshalAs(UnmanagedType.I4)]
        [method: DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int memcmp(
            [param: In] byte[] b1,
            [param: In] byte[] b2,
            [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr count);
    }

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
    }

    [type: Flags]
    public enum KeySpec : uint
    {
        NONE = 0,
        AT_KEYEXCHANGE = 1,
        AT_SIGNATURE = 2,
        NCRYPT = 0xFFFFFFFF
    }

    internal enum NCryptCreatePersistedKeyFlags : uint
    {
        NONE = 0,
        NCRYPT_MACHINE_KEY_FLAG = 0x00000020,
        NCRYPT_OVERWRITE_KEY_FLAG = 0x00000080,
    }

}