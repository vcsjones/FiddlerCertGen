namespace VCSJones.FiddlerCertGen
open System.Runtime.InteropServices

module InteropMemoryFree =
    [<DllImportAttribute("kernel32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "LocalFree", ExactSpelling = true)>]
    extern [<return: MarshalAsAttribute(UnmanagedType.SysInt)>]nativeint LocalFree(
            [<InAttribute; MarshalAs(UnmanagedType.SysInt)>]nativeint hMem
        );
        
    [<DllImportAttribute("kernel32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "FreeLibrary", ExactSpelling = true)>]
    extern [<return: MarshalAsAttribute(UnmanagedType.Bool)>]bool FreeLibrary(
            [<InAttribute; MarshalAs(UnmanagedType.SysInt)>] nativeint hModule
        );

    [<DllImportAttribute("ncrypt.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "NCryptFreeObject", ExactSpelling = true)>]
    extern [<return: MarshalAsAttribute(UnmanagedType.U4)>]uint32 NCryptFreeObject(
            [<InAttribute; MarshalAs(UnmanagedType.SysInt)>] nativeint hObject
        );
        
    [<DllImportAttribute("ncrypt.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "NCryptIsKeyHandle", ExactSpelling = true)>]
    extern [<return: MarshalAsAttribute(UnmanagedType.Bool)>]bool NCryptIsKeyHandle(
            [<InAttribute; MarshalAs(UnmanagedType.SysInt)>] nativeint hKey
        );

    [<DllImportAttribute("advapi32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "CryptReleaseContext", SetLastError = true, ExactSpelling = true)>]
    extern [<return: MarshalAsAttribute(UnmanagedType.Bool)>]bool CryptReleaseContext(
            [<InAttribute; MarshalAs(UnmanagedType.SysInt)>] nativeint pContext,
            [<InAttribute; MarshalAs(UnmanagedType.U4)>] uint32 dwFlags
        );


    [<DllImportAttribute("advapi32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "CryptDestroyKey", SetLastError = true, ExactSpelling = true)>]
    extern [<return: MarshalAsAttribute(UnmanagedType.Bool)>]bool CryptDestroyKey(
            [<InAttribute; MarshalAs(UnmanagedType.SysInt)>] nativeint hKey
        );