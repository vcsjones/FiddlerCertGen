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

        [return: MarshalAs(UnmanagedType.SysInt)]
        [method: DllImport(KERNEL32, EntryPoint = "LocalAlloc", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        public static extern IntPtr LocalAlloc
            (
                [param: In, MarshalAs(UnmanagedType.U4)] uint uFlags,
                [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr uBytes
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