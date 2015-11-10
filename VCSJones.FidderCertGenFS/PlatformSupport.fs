namespace VCSJones.FiddlerCertGen
open System.Runtime.InteropServices

module internal PlatformSupportInterop =
    [<DllImport("kernel32.dll", EntryPoint = "LoadLibraryEx", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, ExactSpelling = true)>]
    extern LibrarySafeHandle internal LoadLibraryEx(
            [<InAttribute; MarshalAs(UnmanagedType.LPTStr)>] string lpFileName,
            [<InAttribute; MarshalAs(UnmanagedType.SysInt)>]nativeint hFile,
            [<InAttribute; MarshalAs(UnmanagedType.U4)>]LoadLibraryFlags dwFlags
        );

    [<DllImport("kernel32.dll", EntryPoint = "GetProcAddress", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)>]
    extern [<return: MarshalAs(UnmanagedType.SysInt)>] nativeint GetProcAddress(
            [<InAttribute>]LibrarySafeHandle hModule,
            [<InAttribute; MarshalAs(UnmanagedType.LPStr)>]string lpProcName
        );
module PlatformSupport =

    let HasCngSupport = 
        use handle = PlatformSupportInterop.LoadLibraryEx("ncrypt.dll", 0n, LoadLibraryFlags.NONE)
        not(handle.IsInvalid) && PlatformSupportInterop.GetProcAddress(handle, "NCryptOpenKey") <> 0n 

    let UseLegacyKeyStoreName = System.Environment.OSVersion.Version < new System.Version(5, 2)
