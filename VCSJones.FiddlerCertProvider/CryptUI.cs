using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertProvider
{

    internal static class NativeMethods
    {
        [method: DllImport("cryptui.dll", EntryPoint = "CryptUIWizExport", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CryptUIWizExport
            (
                [param: In, MarshalAs(UnmanagedType.U4)] CryptUIWizExportFlags dwFlags,
                [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr hwndParent,
                [param: In, MarshalAs(UnmanagedType.LPWStr)] string pwszWizardTitle,
                [param: In] ref CRYPTUI_WIZ_EXPORT_INFO pExportInfo,
                [param: In, MarshalAs(UnmanagedType.SysInt)] IntPtr pvoid
            );
    }

    [type: StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct CRYPTUI_WIZ_EXPORT_INFO
    {
        public uint dwSize;
        public string pwszExportFileName;
        public CryptUIExportInfoSubjectType dwSubjectType;
        public CRYPTUI_WIZ_EXPORT_INFO_UNION context;
        public uint cStores;
        public IntPtr rghStores;
    }

    [type: StructLayout(LayoutKind.Explicit)]
    internal struct CRYPTUI_WIZ_EXPORT_INFO_UNION
    {
        [field: FieldOffset(0)]
        public IntPtr pCertContext;
        [field: FieldOffset(0)]
        public IntPtr pCTLContext;
        [field: FieldOffset(0)]
        public IntPtr pCRLContext;
        [field: FieldOffset(0)]
        public IntPtr hCertStore;
    }

    [type: Flags]
    internal enum CryptUIWizExportFlags : uint
    {
        CRYPTUI_WIZ_NO_UI = 0x0001,
        CRYPTUI_WIZ_IGNORE_NO_UI_FLAG_FOR_CSPS = 0x0002,
        CRYPTUI_WIZ_NO_UI_EXCEPT_CSP = 0x0003,
        CRYPTUI_WIZ_EXPORT_PRIVATE_KEY = 0x0100,
        CRYPTUI_WIZ_EXPORT_NO_DELETE_PRIVATE_KEY = 0x200
    }

    internal enum CryptUIExportInfoSubjectType : uint
    {
        CRYPTUI_WIZ_EXPORT_CERT_CONTEXT = 1,
        CRYPTUI_WIZ_EXPORT_CTL_CONTEXT = 2,
        CRYPTUI_WIZ_EXPORT_CRL_CONTEXT = 3,
        CRYPTUI_WIZ_EXPORT_CERT_STORE = 4,
        CRYPTUI_WIZ_EXPORT_CERT_STORE_CERTIFICATES_ONLY = 5,
        CRYPTUI_WIZ_EXPORT_FORMAT_CRL = 6,
        CRYPTUI_WIZ_EXPORT_FORMAT_CTL = 7,
    }
}