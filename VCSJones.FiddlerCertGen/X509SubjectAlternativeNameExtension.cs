using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    public class X509SubjectAlternativeNameExtension : X509Extension
    {
        public X509SubjectAlternativeNameExtension(IList<X509AlternativeName> altNames, bool critical) : base(new Oid(OIDs.szOID_SUBJECT_ALT_NAME2), EncodeExtension(altNames), critical)
        {

        }

        private static byte[] EncodeExtension(IList<X509AlternativeName> altNames)
        {
            var certAltName = new CERT_ALT_NAME_INFO();
            certAltName.cAltEntry = (uint)altNames.Count;
            var structSize = Marshal.SizeOf(typeof(CERT_ALT_NAME_ENTRY));
            var altNamesBuffer = Marshal.AllocHGlobal(structSize * altNames.Count);
            var unionValues = new List<IntPtr>();
            try
            {
                for (int index = 0, offset = 0; index < altNames.Count; index++, offset += structSize)
                {
                    var altName = new CERT_ALT_NAME_ENTRY();
                    altName.dwAltNameChoice = (CertAltNameChoice)altNames[index].Type;
                    switch (altName.dwAltNameChoice)
                    {
                        case CertAltNameChoice.CERT_ALT_NAME_DNS_NAME:
                            altName.Value = new CERT_ALT_NAME_ENTRY_UNION
                            {
                                pwszDNSName = Marshal.StringToHGlobalUni(altNames[index].Value)
                            };
                            unionValues.Add(altName.Value.pwszDNSName);
                            break;
                        case CertAltNameChoice.CERT_ALT_NAME_URL:
                            altName.Value = new CERT_ALT_NAME_ENTRY_UNION
                            {
                                pwszURL = Marshal.StringToHGlobalUni(altNames[index].Value)
                            };
                            unionValues.Add(altName.Value.pwszURL);
                            break;
                        case CertAltNameChoice.CERT_ALT_NAME_IP_ADDRESS:
                            var ip = IPAddress.Parse(altNames[index].Value);
                            var addressBytes = ip.GetAddressBytes();
                            var ipBytes = Marshal.AllocHGlobal(addressBytes.Length);
                            Marshal.Copy(addressBytes, 0, ipBytes, addressBytes.Length);
                            altName.Value = new CERT_ALT_NAME_ENTRY_UNION
                            {
                                IPAddress = new CRYPTOAPI_BLOB
                                {
                                    cbData = (uint) addressBytes.Length,
                                    pbData = ipBytes
                                }
                            };
                            unionValues.Add(ipBytes);
                            break;
                    }
                    Marshal.StructureToPtr(altName, IntPtrArithmetic.Add(altNamesBuffer, offset), false);
                }
                certAltName.rgAltEntry = altNamesBuffer;
                uint dataSize = 0;
                IntPtr data;
                if (!Crypt32.CryptEncodeObjectEx(EncodingType.X509_ASN_ENCODING, OIDs.szOID_SUBJECT_ALT_NAME2, ref certAltName, 0x8000, IntPtr.Zero, out data, ref dataSize))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                var buffer = new byte[dataSize];
                Marshal.Copy(data, buffer, 0, (int)dataSize);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(altNamesBuffer);
                unionValues.ForEach(Marshal.FreeHGlobal);
            }
        }
    }
}