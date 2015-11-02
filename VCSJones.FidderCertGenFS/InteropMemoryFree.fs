namespace VCSJones.FiddlerCertGen
open System.Runtime.InteropServices

module InteropMemoryFree =
    [<DllImportAttribute("kernel32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "LocalFree", ExactSpelling = true)>]
    extern [<return: MarshalAsAttribute(UnmanagedType.SysInt)>]nativeint LocalFree(
            [<InAttribute; MarshalAs(UnmanagedType.SysInt)>]nativeint hMem
        );