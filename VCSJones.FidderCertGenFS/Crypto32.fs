namespace VCSJones.FiddlerCertGen
open System.Runtime.InteropServices

module internal Crypto32 =
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