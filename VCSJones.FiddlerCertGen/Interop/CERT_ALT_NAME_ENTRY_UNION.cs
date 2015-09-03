using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Explicit)]
    internal struct CERT_ALT_NAME_ENTRY_UNION
    {
        [field: FieldOffset(0)]
        public IntPtr pOtherName;
        [field: FieldOffset(0)]
        public IntPtr pwszRfc822Name;
        [field: FieldOffset(0)]
        public IntPtr pwszDNSName;
        [field: FieldOffset(0)]
        public CRYPTOAPI_BLOB DirectoryName;
        [field: FieldOffset(0)]
        public IntPtr pwszURL;
        [field: FieldOffset(0)]
        public CRYPTOAPI_BLOB IPAddress;
        [field: FieldOffset(0)]
        public IntPtr pszRegisteredID;

    }
}