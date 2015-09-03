using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    [type: StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct CERT_EXTENSION
    {
        public string pszObjId;
        public bool fCritical;
        public CRYPT_OBJID_BLOB Value;
    }
}